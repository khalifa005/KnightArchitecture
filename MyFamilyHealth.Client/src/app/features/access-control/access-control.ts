import { Component, signal } from '@angular/core';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DialogModule } from 'primeng/dialog';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-access-control',
  imports: [TranslocoModule, ButtonModule, InputTextModule, DialogModule, FormsModule],
  templateUrl: './access-control.html',
  styleUrl: './access-control.scss',
})
export class AccessControl {
  displayAddModal = signal(false);

  openAddModal() {
    this.displayAddModal.set(true);
  }
}
