import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AppStore } from '../state/app.store';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslocoModule, ButtonModule, InputTextModule],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  store = inject(AppStore);

  toggleLanguage() {
    const newLang = this.store.lang() === 'en-US' ? 'ar-SA' : 'en-US';
    this.store.setLanguage(newLang);
  }
}
