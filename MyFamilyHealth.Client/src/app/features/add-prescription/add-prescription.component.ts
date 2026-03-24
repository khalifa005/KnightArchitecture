import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SelectModule } from 'primeng/select';
import { TextareaModule } from 'primeng/textarea';

@Component({
  standalone: true,
  selector: 'app-add-prescription',
  imports: [CommonModule, ReactiveFormsModule, TranslocoModule, ButtonModule, InputTextModule, SelectModule, TextareaModule],
  templateUrl: './add-prescription.component.html',
})
export class AddPrescriptionComponent {
  private fb = inject(FormBuilder);

  form = this.fb.group({
    patientId: ['', Validators.required],
    medicationName: ['', Validators.required],
    dosage: ['500mg - Twice daily'],
    frequency: ['BID (Twice daily)'],
    duration: [7, [Validators.required, Validators.min(1)]],
    specialInstructions: ['']
  });

  dosageOptions = [
    { labelKey: 'addPrescription.dose_500_bid', value: '500_bid' },
    { labelKey: 'addPrescription.dose_250_am',  value: '250_am' },
    { labelKey: 'addPrescription.dose_1000_load', value: '1000_load' },
  ];

  frequencyOptions = [
    { labelKey: 'addPrescription.freq_bid', value: 'BID' },
    { labelKey: 'addPrescription.freq_tid', value: 'TID' },
    { labelKey: 'addPrescription.freq_prn', value: 'PRN' },
  ];

  interactions = [
    { type: 'error', icon: 'warning', titleKey: 'addPrescription.conflict_title', bodyKey: 'addPrescription.conflict_body' },
    { type: 'success', icon: 'check_circle', titleKey: 'addPrescription.allergy_title', bodyKey: 'addPrescription.allergy_body' },
  ];

  pendingReviews = [
    { icon: 'pill',    iconBg: 'bg-secondary-fixed text-on-secondary-fixed',  name: 'Amoxicillin (500mg)',   meta: '#4401-229 • Dr. Aris',  statusKey: 'addPrescription.status_stable',    statusClass: 'bg-secondary-fixed text-on-secondary-fixed-variant', time: '2h ago' },
    { icon: 'vaccines', iconBg: 'bg-tertiary-fixed text-on-tertiary-fixed',   name: 'Insulin Glargine',       meta: '#9001-382 • Dr. Varma', statusKey: 'addPrescription.status_priority',  statusClass: 'bg-primary-fixed text-on-primary-fixed-variant',    time: '45m ago' },
    { icon: 'warning',  iconBg: 'bg-error-container text-on-error-container', name: 'Lisinopril (10mg)',      meta: '#2103-991 • Dr. Aris',  statusKey: 'addPrescription.status_flag',      statusClass: 'bg-error text-white',                               time: 'Now' },
  ];

  submit() { if (this.form.valid) { /* submit */ } }
}
