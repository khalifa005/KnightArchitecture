import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { CheckboxModule } from 'primeng/checkbox';

@Component({
  standalone: true,
  selector: 'app-medication-manager',
  imports: [CommonModule, TranslocoModule, ButtonModule, ToggleSwitchModule, DialogModule, InputTextModule, FormsModule, CheckboxModule],
  templateUrl: './medication-manager.html',
  styleUrl: './medication-manager.scss',
})
export class MedicationManager {
  remindersActive = signal(true);
  displayAddModal = signal(false);
  isBlacklisted = signal(false);

  openAddModal() {
    this.displayAddModal.set(true);
  }
}
