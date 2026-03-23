import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';

@Component({
  selector: 'app-test-page',
  standalone: true,
  imports: [CommonModule, ButtonModule, CardModule],
  templateUrl: './test-page.component.html',
  styleUrl: './test-page.component.scss'
})
export class TestPageComponent {
  title = 'MyFamilyHealth.Client';
  isLoaded = true;

  features = [
    { id: 1, name: 'Standalone Components' },
    { id: 2, name: 'PrimeNG Integration' },
    { id: 3, name: 'i18n Support' },
    { id: 4, name: 'Modern Control Flow (@if, @for)' }
  ];

  toggleLoad() {
    this.isLoaded = !this.isLoaded;
  }
}
