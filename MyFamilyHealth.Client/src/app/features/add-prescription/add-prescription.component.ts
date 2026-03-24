import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';
import { inject } from '@angular/core';

@Component({
  selector: 'app-add-prescription',
  imports: [CommonModule, ReactiveFormsModule, TranslocoModule, ButtonModule, InputTextModule, SelectModule, TextareaModule],
  templateUrl: './add-prescription.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: { class: 'block w-full' }
})
export class AddPrescriptionComponent {
  private fb = inject(FormBuilder);

  form: FormGroup = this.fb.group({
    patientId: ['', Validators.required],
    medicationName: ['', Validators.required],
    dosage: ['500mg - Twice daily'],
    frequency: ['BID (Twice daily)'],
    duration: [7, [Validators.required, Validators.min(1)]],
    specialInstructions: ['']
  });

  dosageOptions = ['500mg - Twice daily', '250mg - Morning only', '1000mg - Loading dose'];
  frequencyOptions = ['BID (Twice daily)', 'TID (Thrice daily)', 'PRN (As needed)'];

  interactions = [
    { type: 'error', icon: 'warning', titleKey: 'add-prescription.interactions.conflict_title', bodyKey: 'add-prescription.interactions.conflict_body' },
    { type: 'success', icon: 'check_circle', titleKey: 'add-prescription.interactions.allergy_title', bodyKey: 'add-prescription.interactions.allergy_body' },
  ];

  pendingReviews = [
    { icon: 'pill', iconBg: 'bg-secondary-fixed text-on-secondary-fixed', name: 'Amoxicillin (500mg)', meta: 'Patient ID: #4401-229 • Dr. Aris', statusKey: 'add-prescription.status.stable', statusStyle: 'bg-secondary-fixed text-on-secondary-fixed-variant', time: '2h ago' },
    { icon: 'vaccines', iconBg: 'bg-tertiary-fixed text-on-tertiary-fixed', name: 'Insulin Glargine', meta: 'Patient ID: #9001-382 • Dr. Varma', statusKey: 'add-prescription.status.priority', statusStyle: 'bg-primary-fixed text-on-primary-fixed-variant', time: '45m ago' },
    { icon: 'warning', iconBg: 'bg-error-container text-on-error-container', name: 'Lisinopril (10mg)', meta: 'Patient ID: #2103-991 • Dr. Aris', statusKey: 'add-prescription.status.flag', statusStyle: 'bg-error text-on-error', time: 'Now' },
  ];

  submit() { if (this.form.valid) { /* submit logic */ } }
}
