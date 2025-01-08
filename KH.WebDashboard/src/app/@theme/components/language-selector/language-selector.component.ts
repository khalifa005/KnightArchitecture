import { Component, OnInit } from '@angular/core';
import { I18nService } from '../../../@i18n/services/i18n.service';

@Component({
  selector: 'app-language-selector',
  templateUrl: './language-selector.component.html'
})
export class LanguageSelectorComponent implements OnInit {
  constructor(
    private i18nService: I18nService,
  ) {}

  ngOnInit() {

    // this.currentLanguage$.subscribe((language) => {
    //   this.translate.use(language);
    // });
    
  }

  setLanguage(language: string) {
    this.i18nService.language = language;
    // this.store.dispatch(LanguageActions.set({ language }));
  }

  
}
