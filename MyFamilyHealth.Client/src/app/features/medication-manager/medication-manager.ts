import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { Component } from '@angular/core';

@Component({
  standalone: true,
  selector: 'app-medication-manager',
  imports: [TranslocoModule, ButtonModule, InputTextModule],
  templateUrl: './medication-manager.html',
  styleUrl: './medication-manager.scss',
})
export class MedicationManager {}
