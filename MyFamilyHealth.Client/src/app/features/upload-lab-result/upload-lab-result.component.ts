import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { inject } from '@angular/core';

@Component({
  selector: 'app-upload-lab-result',
  imports: [CommonModule, ReactiveFormsModule, TranslocoModule, ButtonModule, InputTextModule, SelectModule],
  templateUrl: './upload-lab-result.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: { class: 'block w-full' }
})
export class UploadLabResultComponent {
  private fb = inject(FormBuilder);
  isDragging = signal(false);

  form: FormGroup = this.fb.group({
    nationalId: ['', Validators.required],
    labSource: ['Aether Central Diagnostics'],
    collectionDate: ['', Validators.required],
  });

  labSources = ['Aether Central Diagnostics', 'Metropolitan Medical Labs', 'Priority Pathology Group'];

  parameters = signal([
    { name: 'Glucose, Fasting', result: '98', range: '70 - 99', unit: 'mg/dL', high: false },
    { name: 'Hemoglobin A1c', result: '6.1', range: '< 5.7', unit: '%', high: true },
  ]);

  addRow() {
    this.parameters.update(p => [...p, { name: '', result: '', range: '', unit: '', high: false }]);
  }

  removeRow(index: number) {
    this.parameters.update(p => p.filter((_, i) => i !== index));
  }

  onDragOver(e: DragEvent) { e.preventDefault(); this.isDragging.set(true); }
  onDragLeave() { this.isDragging.set(false); }
  onDrop(e: DragEvent) { e.preventDefault(); this.isDragging.set(false); }
}
