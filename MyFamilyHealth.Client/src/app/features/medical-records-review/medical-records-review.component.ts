import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-medical-records-review',
  imports: [CommonModule, TranslocoModule, ButtonModule, TableModule],
  templateUrl: './medical-records-review.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: { class: 'block w-full' }
})
export class MedicalRecordsReviewComponent {
  patient = {
    name: 'Eleanor Vance',
    id: '#AE-992-01',
    age: 68,
    sex: 'Female',
    bloodType: 'O Positive',
    weight: '64.5 kg',
    height: '162 cm',
    image: 'https://lh3.googleusercontent.com/aida-public/AB6AXuDn3Lj-of7O9g2VSjOBTCRyVFzyVlECHpr3tjKnwUK23ror3dAq33kvP-w9eo_mdDwUVZa0NHJ0EFJUmM1CkBxwYCYy8Y_lQzeUZWIqg2p_-HFemzeHZypa1-6z1WXjr8trvhfrHl7nQ6Fc14xIDuTOZ6cWRtrtB8uZPVDI1nbTxLj3A57ecH4qcyfVSfG0sT8FTeQE5GUeV6cujZqeD2vD4YZXOdFja3XCv6eJQM7qLmqKsUombEv1hq1EG32DWSh37M8njnTVxmAX'
  };

  vitals = [
    { labelKey: 'medical-records-review.vitals.heart_rate', value: '72', unit: 'bpm', statusKey: null },
    { labelKey: 'medical-records-review.vitals.blood_pressure', value: '118/79', unit: 'mmHg', statusKey: 'medical-records-review.vitals.optimal' },
    { labelKey: 'medical-records-review.vitals.spo2', value: '98', unit: '%', statusKey: 'medical-records-review.vitals.normal' },
  ];

  labResults = [
    { test: 'Glucose', result: '92 mg/dL', range: '65 - 99 mg/dL', statusKey: 'medical-records-review.status.normal', high: false },
    { test: 'Creatinine', result: '1.4 mg/dL', range: '0.7 - 1.3 mg/dL', statusKey: 'medical-records-review.status.high', high: true },
    { test: 'Potassium', result: '4.2 mmol/L', range: '3.5 - 5.1 mmol/L', statusKey: 'medical-records-review.status.normal', high: false },
  ];

  tasks = [
    'Order follow-up Creatinine blood work in 14 days',
    'Consult with Nephrology regarding GFR trend'
  ];
}
