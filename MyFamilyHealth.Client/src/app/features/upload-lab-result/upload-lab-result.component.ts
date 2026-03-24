import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';

@Component({
  standalone: true,
  selector: 'app-upload-lab-result',
  imports: [CommonModule, ReactiveFormsModule, TranslocoModule, ButtonModule, InputTextModule, SelectModule],
  templateUrl: './upload-lab-result.component.html',
})
export class UploadLabResultComponent {
  private fb = inject(FormBuilder);
  isDragging = signal(false);

  form = this.fb.group({
    nationalId: ['', Validators.required],
    labSource: ['Aether Central Diagnostics'],
    collectionDate: ['', Validators.required],
  });

  labSources = [
    { label: 'Aether Central Diagnostics', value: 'aether' },
    { label: 'Metropolitan Medical Labs', value: 'metro' },
    { label: 'Priority Pathology Group', value: 'priority' },
  ];

  parameters = signal([
    { name: 'Glucose, Fasting', result: '98', range: '70 - 99', unit: 'mg/dL', high: false },
    { name: 'Hemoglobin A1c',   result: '6.1', range: '< 5.7',  unit: '%',     high: true  },
  ]);

  addRow() { this.parameters.update(p => [...p, { name: '', result: '', range: '', unit: '', high: false }]); }
  removeRow(index: number) { this.parameters.update(p => p.filter((_, i) => i !== index)); }
  onDragOver(e: DragEvent) { e.preventDefault(); this.isDragging.set(true); }
  onDragLeave() { this.isDragging.set(false); }
  onDrop(e: DragEvent) { e.preventDefault(); this.isDragging.set(false); }
}
