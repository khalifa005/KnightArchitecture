import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslocoModule, TranslocoService } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { AppStore } from '../../core/state/app.store';

/** Volume levels — intro narration is louder than the ambient heartbeat effect */
const NARRATION_VOLUME = 0.9; // 90% — prominent narration
const HEARTBEAT_PEAK_GAIN = 0.04; // audible heartbeat thump

const AUDIO_FILES: Record<string, string> = {
  'en-US': 'assets/audio/victoria english intro for landing page .mp3',
  'ar-SA': 'assets/audio/oliver arabic intro for landing page .mp3',
};

@Component({
  selector: 'app-landing',
  imports: [CommonModule, RouterModule, TranslocoModule, ButtonModule],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LandingComponent implements OnInit {
  private readonly translocoService = inject(TranslocoService);
  private readonly destroyRef = inject(DestroyRef);
  readonly store = inject(AppStore);

  isSoundEnabled = signal(true);

  private audioCtx: AudioContext | null = null;
  private beepInterval: ReturnType<typeof setInterval> | null = null;
  private narrationAudio: HTMLAudioElement | null = null;
  private audioUnlocked = false;

  ngOnInit(): void {
    this.startHeartbeat();
    this.setupAudioUnlock();

    // Re-play narration when language changes
    this.translocoService.langChanges$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe(lang => {
        if (this.audioUnlocked && this.isSoundEnabled()) {
          this.playNarration(lang);
        }
      });
  }

  toggleLanguage(): void {
    const currentLang = this.translocoService.getActiveLang();
    const targetLang = currentLang === 'en-US' ? 'ar-SA' : 'en-US';
    this.translocoService.setActiveLang(targetLang);
    document.documentElement.dir = targetLang === 'ar-SA' ? 'rtl' : 'ltr';
  }

  toggleSound(): void {
    this.isSoundEnabled.update(v => !v);
    if (this.isSoundEnabled()) {
      this.startHeartbeat();
      if (this.audioUnlocked) {
        this.playNarration(this.translocoService.getActiveLang());
      }
    } else {
      this.stopHeartbeat();
      this.stopNarration();
    }
  }

  toggleDarkMode(): void {
    this.store.toggleDarkMode();
  }

  // ──────────────────────────────────────────────
  //  Narration (language-specific MP3)
  // ──────────────────────────────────────────────

  private playNarration(lang: string): void {
    this.stopNarration();
    const src = AUDIO_FILES[lang] ?? AUDIO_FILES['en-US'];
    this.narrationAudio = new Audio(src);
    this.narrationAudio.volume = NARRATION_VOLUME;
    this.narrationAudio.play().catch(() => {
      // Browser may still block — will retry on next user interaction
    });
  }

  private stopNarration(): void {
    if (this.narrationAudio) {
      this.narrationAudio.pause();
      this.narrationAudio.currentTime = 0;
      this.narrationAudio = null;
    }
  }

  // ──────────────────────────────────────────────
  //  AudioContext unlock on first interaction
  // ──────────────────────────────────────────────

  private setupAudioUnlock(): void {
    const unlock = () => {
      if (this.audioUnlocked) return;
      this.audioUnlocked = true;

      if (this.audioCtx?.state === 'suspended') {
        this.audioCtx.resume();
      }

      if (this.isSoundEnabled()) {
        this.playNarration(this.translocoService.getActiveLang());
      }

      document.removeEventListener('click', unlock);
      document.removeEventListener('touchstart', unlock);
      document.removeEventListener('keydown', unlock);
    };

    document.addEventListener('click', unlock);
    document.addEventListener('touchstart', unlock);
    document.addEventListener('keydown', unlock);
  }

  // ──────────────────────────────────────────────
  //  Heartbeat effect (Web Audio API — subtle)
  // ──────────────────────────────────────────────

  private startHeartbeat(): void {
    try {
      if (!this.audioCtx) {
        this.audioCtx = new (
          window.AudioContext ||
          (window as unknown as { webkitAudioContext: typeof AudioContext })
            .webkitAudioContext
        )();
      }
    } catch {
      return;
    }

    const playBeep = () => {
      if (!this.audioCtx || !this.isSoundEnabled()) return;
      if (this.audioCtx.state === 'suspended') return;

      const t = this.audioCtx.currentTime;

      // Realistic "Lub-Dub" Heartbeat
      const playThump = (time: number, freq: number, gainVal: number, duration: number) => {
        const osc = this.audioCtx!.createOscillator();
        const gain = this.audioCtx!.createGain();
        const filter = this.audioCtx!.createBiquadFilter();
        
        // Low-pass filter to give it that "muffled/inside-chest" feel
        filter.type = 'lowpass';
        filter.frequency.setValueAtTime(150, time);
        filter.Q.setValueAtTime(1, time);

        osc.type = 'sine';
        // Frequency sweep for "thump" impact
        osc.frequency.setValueAtTime(freq * 1.5, time);
        osc.frequency.exponentialRampToValueAtTime(freq, time + 0.05);
        
        gain.gain.setValueAtTime(0, time);
        gain.gain.linearRampToValueAtTime(gainVal, time + 0.01);
        gain.gain.exponentialRampToValueAtTime(0.001, time + duration);
        
        osc.connect(filter);
        filter.connect(gain);
        gain.connect(this.audioCtx!.destination);
        
        osc.start(time);
        osc.stop(time + duration + 0.1);
      };

      // Lub (S1) - Lower, longer
      playThump(t, 45, HEARTBEAT_PEAK_GAIN * 3.5, 0.25);
      // Dub (S2) - Higher, sharper
      playThump(t + 0.25, 55, HEARTBEAT_PEAK_GAIN * 2.5, 0.15);
    };

    this.stopHeartbeat();
    playBeep();
    this.beepInterval = setInterval(playBeep, 1400); // Slightly slower, more relaxed rate
  }

  private stopHeartbeat(): void {
    if (this.beepInterval !== null) {
      clearInterval(this.beepInterval);
      this.beepInterval = null;
    }
  }
}
