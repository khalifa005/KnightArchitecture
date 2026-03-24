import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslocoModule, TranslocoService } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';

@Component({
  standalone: true,
  selector: 'app-landing',
  imports: [CommonModule, RouterModule, TranslocoModule, ButtonModule],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss'
})
export class LandingComponent implements OnInit, OnDestroy {
  isSoundEnabled = false;
  private audioCtx: AudioContext | null = null;
  private beepInterval: any;

  constructor(private translocoService: TranslocoService) {}
  
  ngOnInit() {
    // Await manual user interaction per DOM policy restrictions
  }

  ngOnDestroy() {
    this.stopHeartbeat();
  }

  toggleLanguage() {
    const currentLang = this.translocoService.getActiveLang();
    const targetLang = currentLang === 'en-US' ? 'ar-SA' : 'en-US';
    this.translocoService.setActiveLang(targetLang);
    document.documentElement.dir = targetLang === 'ar-SA' ? 'rtl' : 'ltr';
  }

  toggleSound() {
    this.isSoundEnabled = !this.isSoundEnabled;
    if (this.isSoundEnabled) {
      this.startHeartbeat();
    } else {
      this.stopHeartbeat();
    }
  }

  private startHeartbeat() {
    try {
      if (!this.audioCtx) {
        this.audioCtx = new (window.AudioContext || (window as any).webkitAudioContext)();
      }
      if (this.audioCtx.state === 'suspended') {
        this.audioCtx.resume();
      }
    } catch(e) { return; }

    const playBeep = () => {
      if (!this.audioCtx || !this.isSoundEnabled) return;
      const osc = this.audioCtx.createOscillator();
      const gainNode = this.audioCtx.createGain();
      
      osc.type = 'sine';
      osc.frequency.setValueAtTime(1000, this.audioCtx.currentTime); 
      
      // Envelope array
      gainNode.gain.setValueAtTime(0, this.audioCtx.currentTime);
      gainNode.gain.linearRampToValueAtTime(0.04, this.audioCtx.currentTime + 0.01);
      gainNode.gain.exponentialRampToValueAtTime(0.001, this.audioCtx.currentTime + 0.15);
      
      osc.connect(gainNode);
      gainNode.connect(this.audioCtx.destination);
      
      osc.start();
      osc.stop(this.audioCtx.currentTime + 0.15);
    };

    playBeep();
    this.beepInterval = setInterval(playBeep, 1200);
  }

  private stopHeartbeat() {
    if (this.beepInterval) {
      clearInterval(this.beepInterval);
      this.beepInterval = null;
    }
  }
}
