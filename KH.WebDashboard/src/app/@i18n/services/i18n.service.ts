import { Injectable } from '@angular/core';
import { Subscription } from 'rxjs';
import { TranslateService,
   LangChangeEvent } from '@ngx-translate/core';
import { Logger } from '../../@core/utils.ts/logger.service';
import { LanguageEnum } from '../models/language.enum';

const log = new Logger('I18nService');
const LANGUAGE_KEY = 'language';

@Injectable({
  providedIn: 'root'
})
export class I18nService {
  private defaultLanguage: string = 'en-US';
  private supportedLanguages: string[] = [];

  private langChangeSubscription!: Subscription;

  constructor(private translateService: TranslateService) {
    // Embed languages to avoid extra HTTP requests
    // translateService.setTranslation('en-US', enUS);
    // translateService.setTranslation('ar-SA', arSa);
  }

  /**
   * Initializes i18n for the application.
   * Loads language from local storage if present, or sets default language.
   * @param defaultLanguage The default language to use.
   * @param supportedLanguages The list of supported languages.
   */
  init(defaultLanguage: string, supportedLanguages: string[]): void {
    this.defaultLanguage = defaultLanguage;
    this.supportedLanguages = supportedLanguages;

    const storedLanguage = localStorage.getItem(LANGUAGE_KEY) || '';
    this.language = storedLanguage;
    this.translateService.use(this.language ?? this.defaultLanguage);

    this.langChangeSubscription = this.translateService.onLangChange.subscribe((event: LangChangeEvent) => {
      localStorage.setItem(LANGUAGE_KEY, event.lang);
    });
  }

  destroy() {
    if (this.langChangeSubscription) {
      this.langChangeSubscription.unsubscribe();
    }
  }

  set language(lang: string) {
    let resolvedLanguage = this.resolveLanguage(lang);
    this.translateService.use(resolvedLanguage);
    log.debug(`Language set to ${resolvedLanguage}`);
  }

  get language(): string {
    return this.translateService.currentLang ?? localStorage.getItem(LANGUAGE_KEY);;
  }

  private resolveLanguage(lang: string): string {
    const browserLang = this.translateService.getBrowserCultureLang();
    let resolvedLang = lang || browserLang || this.defaultLanguage;

    if (!this.supportedLanguages.includes(resolvedLang)) {
      const regionFreeLang = resolvedLang.split('-')[0];
      resolvedLang = this.supportedLanguages.find((supportedLang) => supportedLang.startsWith(regionFreeLang)) 
                     || this.defaultLanguage;
    }

    return resolvedLang;
  }

  get appLanguageApiKey(): string {
    return this.translateService.currentLang === LanguageEnum.Ar ? 'A' : 'L';
  }
}
