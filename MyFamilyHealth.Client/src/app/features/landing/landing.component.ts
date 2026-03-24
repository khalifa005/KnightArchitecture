import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslocoModule, TranslocoService } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';

@Component({
  standalone: true,
  selector: 'app-landing',
  imports: [CommonModule, RouterModule, TranslocoModule, ButtonModule],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.scss'
})
export class LandingComponent {
  constructor(private translocoService: TranslocoService) {}
  
  toggleLanguage() {
    const currentLang = this.translocoService.getActiveLang();
    const targetLang = currentLang === 'en-US' ? 'ar-SA' : 'en-US';
    this.translocoService.setActiveLang(targetLang);
    document.documentElement.dir = targetLang === 'ar-SA' ? 'rtl' : 'ltr';
  }
}
