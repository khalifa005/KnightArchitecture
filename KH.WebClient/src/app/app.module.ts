import { inject, LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { en_US, fr_FR, NZ_DATE_LOCALE, NZ_I18N, NzI18nService } from 'ng-zorro-antd/i18n';
import { ar_EG } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import ar from '@angular/common/locales/ar';
import en from '@angular/common/locales/en';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { IconsProviderModule } from './icons-provider.module';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateLoader, TranslateModule, TranslateService } from '@ngx-translate/core';
// import { provideNzI18n, en_US } from 'ng-zorro-antd/i18n';
import { BidiModule } from '@angular/cdk/bidi';
import { CoreModule } from './core/core.module';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzConfig } from 'ng-zorro-antd/core/config';

// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient): TranslateHttpLoader {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}



// override internationalization configuration
const customEnUS = {
  ...en_US,
  Pagination: {
    items_per_page: 'items/page',
    jump_to: 'Jump to',
  },
};

const customArEG = {
  ...ar_EG,
  Pagination: {
    items_per_page: 'عناصر/صفحة',
    jump_to: 'انتقل إلى',
  },
};
registerLocaleData(ar);
// registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    NzButtonModule,
    NzSelectModule,
    
    BidiModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
    }),
    IconsProviderModule,
    NzLayoutModule,
    NzMenuModule,

    NzIconModule.forRoot([]), // Add icons here if needed

  ],
  providers: [
    // Set the value of NZ_DATE_LOCALE in the application root module to activate date-fns mode
    { provide: NZ_DATE_LOCALE, useValue: ar_EG },

    // { provide: NZ_I18N, useValue: customLanguagePack },
    // { provide: NZ_I18N, useValue: ar_EG },

    {
      provide: NZ_I18N,
      useFactory: (translate: TranslateService) => {
        const lang = localStorage.getItem('language') || 'en';
        return lang === 'ar_EG' ? customArEG : customEnUS;
      },
      deps: [TranslateService],
    },

    // {
    //   provide: NZ_I18N,
    //   useFactory: () => {
    //     const localId = inject(LOCALE_ID);
    //     switch (localId) {
    //       case 'en':
    //         return en_US;
    //       /** keep the same with angular.json/i18n/locales configuration **/
    //       case 'ar_EG':
    //         return ar_EG;
    //       default:
    //         return en_US;
    //     }
    //   }
    // }

  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  constructor(private i18n: NzI18nService) { }

  // ...

  switchLanguage() {
    // this.i18n.setDateLocale(ja); // Switch language to Japanese at runtime
  }
}
