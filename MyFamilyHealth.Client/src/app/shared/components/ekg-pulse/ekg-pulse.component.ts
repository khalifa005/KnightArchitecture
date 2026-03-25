import { Component, ChangeDetectionStrategy, input } from '@angular/core';

/**
 * Reusable EKG Pulse Animation Component
 * Supports multiple animation types: 'pulsar', 'jugular', 'bleed', 'flat', 'pulse-concept'
 */
@Component({
  selector: 'app-ekg-pulse',
  standalone: true,
  templateUrl: './ekg-pulse.component.html',
  styleUrl: './ekg-pulse.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EkgPulseComponent {
  type = input<'pulsar' | 'jugular' | 'bleed' | 'flat' | 'pulse-concept'>('pulse-concept');
}
