import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';

@Component({
  selector: 'app-upload-center',
  imports: [CommonModule, ReactiveFormsModule, TranslocoModule, ButtonModule, InputTextModule, SelectModule],
  templateUrl: './upload-center.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: { class: 'block w-full' }
})
export class UploadCenterComponent {
  isDragging = signal(false);
  selectedCategory = signal<'lab' | 'imaging' | 'prescription'>('lab');

  recentUploads = [
    { patient: 'Alex M*****s', category: 'Lab Test', categoryStyle: 'bg-tertiary-fixed text-on-tertiary-fixed-variant', time: 'Oct 24, 14:22', status: 'AI-Analyzed', statusIcon: null, pulse: true },
    { patient: 'Sarah K*****n', category: 'X-Ray', categoryStyle: 'bg-secondary-fixed text-on-secondary-fixed-variant', time: 'Oct 24, 13:45', status: 'Verified', statusIcon: 'check_circle', pulse: false },
    { patient: 'David R*****z', category: 'Prescription', categoryStyle: 'bg-primary-fixed text-on-primary-fixed-variant', time: 'Oct 24, 13:10', status: 'Processing', statusIcon: 'progress_activity', pulse: false },
  ];

  selectCategory(cat: 'lab' | 'imaging' | 'prescription') {
    this.selectedCategory.set(cat);
  }

  onDragOver(e: DragEvent) { e.preventDefault(); this.isDragging.set(true); }
  onDragLeave() { this.isDragging.set(false); }
  onDrop(e: DragEvent) { e.preventDefault(); this.isDragging.set(false); }
}
