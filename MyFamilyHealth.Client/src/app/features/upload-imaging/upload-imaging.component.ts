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

  bodyParts = [
    { labelKey: 'uploadImaging.part_chest', value: 'chest' },
    { labelKey: 'uploadImaging.part_abdomen', value: 'abdomen' },
    { labelKey: 'uploadImaging.part_cranial', value: 'cranial' },
    { labelKey: 'uploadImaging.part_upper_limb', value: 'upper_limb' },
    { labelKey: 'uploadImaging.part_lower_limb', value: 'lower_limb' }
  ];
  viewTypes = [
    { labelKey: 'uploadImaging.view_pa', value: 'pa' },
    { labelKey: 'uploadImaging.view_ap', value: 'ap' },
    { labelKey: 'uploadImaging.view_lateral', value: 'lateral' },
    { labelKey: 'uploadImaging.view_oblique', value: 'oblique' }
  ];

  selectedBodyPart = 'chest';
  selectedViewType = 'pa';
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
