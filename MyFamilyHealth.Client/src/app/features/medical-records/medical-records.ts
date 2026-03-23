import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { Component } from '@angular/core';

@Component({
  selector: 'app-medical-records',
  imports: [ButtonModule, InputTextModule],
  templateUrl: './medical-records.html',
  styleUrl: './medical-records.scss',
})
export class MedicalRecords {}
