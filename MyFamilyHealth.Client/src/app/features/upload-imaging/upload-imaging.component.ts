import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-upload-imaging',
  imports: [CommonModule, FormsModule, TranslocoModule, ButtonModule, InputTextModule, SelectModule, TextareaModule],
  templateUrl: './upload-imaging.component.html',
})
export class UploadImagingComponent {
  isDragging = signal(false);

  uploadedFiles = signal([
    { name: 'Chest_Xray_PA_002.dcm', size: '42.8 MB', progress: 75 }
  ]);

  bodyParts = ['Chest / Thorax', 'Abdomen', 'Cranial', 'Upper Limb', 'Lower Limb'];
  viewTypes = ['Posterior-Anterior (PA)', 'Anterior-Posterior (AP)', 'Lateral', 'Oblique'];

  selectedBodyPart = 'Chest / Thorax';
  selectedViewType = 'Posterior-Anterior (PA)';
  radiologistName = '';
  notes = '';

  annotationTools = [
    { icon: 'zoom_in' }, { icon: 'zoom_out' }, { icon: 'draw', active: true },
    { icon: 'square' }, { icon: 'straighten' }
  ];

  onDragOver(event: DragEvent) { event.preventDefault(); this.isDragging.set(true); }
  onDragLeave() { this.isDragging.set(false); }
  onDrop(event: DragEvent) { event.preventDefault(); this.isDragging.set(false); }
  removeFile(index: number) { this.uploadedFiles.update(f => f.filter((_, i) => i !== index)); }
}
