/* eslint-disable @typescript-eslint/no-inferrable-types */
import { Component, Inject, Input, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { NbLayoutDirection, NbLayoutDirectionService } from '@nebular/theme';
import { TranslateService } from '@ngx-translate/core';
import { Subject, takeUntil } from 'rxjs';
import { I18nService } from '../../../../@i18n/services/i18n.service';
import { LanguageEnum } from '../../../../@i18n/models/language.enum';
// import { DateAdapter, MAT_DATE_LOCALE } from '@angular/material/core';

@Component({
  selector: 'app-layout-direction-switcher',
  templateUrl: './layout-direction-switcher.component.html',
  styleUrls: ['./layout-direction-switcher.component.scss'],
})
export class LayoutDirectionSwitcherComponent implements OnDestroy {

  protected destroy$ = new Subject<void>();

  directions = NbLayoutDirection;
  currentDirection: NbLayoutDirection = NbLayoutDirection.LTR;

  @Input() vertical: boolean = false;

  constructor(private directionService: NbLayoutDirectionService,
    private i18nService: I18nService,
    // private _adapter: DateAdapter<any>,
    // @Inject(MAT_DATE_LOCALE) private _locale: string,
    private router: Router,
    // public translate: TranslateService
  ) {

    this.directionService.onDirectionChange()
      .pipe(takeUntil(this.destroy$))
      .subscribe(newDirection => this.currentDirection = newDirection);

    if (this.i18nService.language) {
      if (this.i18nService.language === LanguageEnum.Ar) {
        this.directionService.setDirection(this.directions.RTL);
      }else{
        this.directionService.setDirection(this.directions.LTR);
      }
    } 
    else {
      this.directionService.setDirection(this.directions.LTR);
    }

  }

  toggleDirection(newDirection: any) {

    this.directionService.setDirection(newDirection);
    //keep this as it will change the lang
    if (newDirection == this.directions.RTL) {
      this.i18nService.language = LanguageEnum.Ar;
      // this._adapter.setLocale('ar-EG'); // Switch to Arabic locale.
      // this._adapter.setLocale(this._locale);
    }
    else {
      
      // this._adapter.setLocale('en-GB');
      // this.i18nService.language = 'en-US';
      this.i18nService.language = LanguageEnum.En;
      
    }
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
