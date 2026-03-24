import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';

@Component({
  selector: 'app-upload-imaging',
  imports: [CommonModule, ReactiveFormsModule, TranslocoModule, ButtonModule, InputTextModule, SelectModule, TextareaModule],
  templateUrl: './upload-imaging.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: { class: 'block w-full' }
})
export class UploadImagingComponent {
  private fb = new (class { group = (c: object) => c; })();

  isDragging = signal(false);
  uploadedFiles = signal([
    { name: 'Chest_Xray_PA_002.dcm', size: '42.8 MB', progress: 75 }
  ]);

  bodyParts = ['Chest / Thorax', 'Abdomen', 'Cranial', 'Upper Limb', 'Lower Limb'];
  viewTypes = ['Posterior-Anterior (PA)', 'Anterior-Posterior (AP)', 'Lateral', 'Oblique'];

  selectedBodyPart = signal('Chest / Thorax');
  selectedViewType = signal('Posterior-Anterior (PA)');
  radiologistName = signal('');
  notes = signal('');

  onDragOver(event: DragEvent) {
    event.preventDefault();
    this.isDragging.set(true);
  }

  onDragLeave() {
    this.isDragging.set(false);
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    this.isDragging.set(false);
  }

  removeFile(index: number) {
    this.uploadedFiles.update(files => files.filter((_, i) => i !== index));
  }
}
