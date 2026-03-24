import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  standalone: true,
  selector: 'app-upload-center',
  imports: [CommonModule, TranslocoModule, ButtonModule, InputTextModule],
  templateUrl: './upload-center.component.html',
})
export class UploadCenterComponent {
  isDragging = signal(false);
  selectedCategory = signal<'lab' | 'imaging' | 'prescription'>('lab');

  recentUploads = [
    { patient: 'Alex M*****s', category: 'Lab Test', categoryClass: 'bg-tertiary-fixed text-on-tertiary-fixed-variant', time: 'Oct 24, 14:22', status: 'AI-Analyzed', statusIcon: '', pulse: true },
    { patient: 'Sarah K*****n', category: 'X-Ray', categoryClass: 'bg-secondary-fixed text-on-secondary-fixed-variant', time: 'Oct 24, 13:45', status: 'Verified', statusIcon: 'check_circle', pulse: false },
    { patient: 'David R*****z', category: 'Prescription', categoryClass: 'bg-primary-fixed text-on-primary-fixed-variant', time: 'Oct 24, 13:10', status: 'Processing', statusIcon: 'progress_activity', pulse: false },
  ];

  selectCategory(cat: string) { this.selectedCategory.set(cat as 'lab' | 'imaging' | 'prescription'); }
  onDragOver(e: DragEvent) { e.preventDefault(); this.isDragging.set(true); }
  onDragLeave() { this.isDragging.set(false); }
  onDrop(e: DragEvent) { e.preventDefault(); this.isDragging.set(false); }
}
