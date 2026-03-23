import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { Component } from '@angular/core';

@Component({
  selector: 'app-medication-manager',
  imports: [ButtonModule, InputTextModule],
  templateUrl: './medication-manager.html',
  styleUrl: './medication-manager.scss',
})
export class MedicationManager {}
