import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ar_EG, en_US, NzI18nService } from 'ng-zorro-antd/i18n';

@Component({
  selector: 'app-language-switcher',
  templateUrl: './language-switcher.component.html',
  styleUrls: ['./language-switcher.component.css']
})
export class LanguageSwitcherComponent {
  languages = [
    { code: 'en', name: 'English' },
    { code: 'ar', name: 'العربية' },
  ];
  selectedLanguage = localStorage.getItem('language') || 'en';

  constructor(
    private translate: TranslateService,
    private nzI18n: NzI18nService
  ) {
    this.translate.setDefaultLang(this.selectedLanguage);
    this.switchLanguage(this.selectedLanguage);
  }

  switchLanguage(lang: string): void {
    this.translate.use(lang);
    localStorage.setItem('language', lang);
    this.nzI18n.setLocale(lang === 'ar' ? ar_EG : en_US);
  }
}