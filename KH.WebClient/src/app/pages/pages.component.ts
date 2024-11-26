import { Component, OnInit, OnDestroy } from "@angular/core";
import { TranslateService } from "@ngx-translate/core";
import { ar_EG, en_US, NzI18nService } from "ng-zorro-antd/i18n";
import { Directionality } from '@angular/cdk/bidi';
import { NzConfigService } from "ng-zorro-antd/core/config";

@Component({
  selector: 'app-pages',
  templateUrl: './pages.component.html',
  styleUrls: ['./pages.component.css']
})
export class PagesComponent implements OnInit, OnDestroy {
  isCollapsed = false;

  constructor(
    private i18n: NzI18nService,
    private translate: TranslateService,
    private nzConfigService: NzConfigService,
    private dir: Directionality
  ) {
    this.translate.addLangs(['en_US', 'ar_EG']);
    this.translate.setDefaultLang('en_US');

    // this.translate.onLangChange.subscribe((event) => {
    //   this.dir.value = event.lang === 'ar' ? 'rtl' : 'ltr';
    // });
  }

  switchLanguage(lang: string) {
    this.i18n.setLocale(lang === 'ar_EG' ? ar_EG : en_US);
    this.translate.use(lang);
    localStorage.setItem('language', lang);
    
    // Change layout direction dynamically
    const direction = lang === 'ar_EG' ? 'rtl' : 'ltr';

     // Update NG-ZORRO configuration
     this.nzConfigService.set('message', { nzDirection: direction });
     this.nzConfigService.set('notification', { nzDirection: direction });
    //  this.nzConfigService.set('rtl', true);

     // Update global layout direction
    this.dir.change.emit(direction);
     document.body.dir = direction;
     document.dir = direction;

  }
  logout(): void {
    console.log('Logging out...');
    // Add your logout logic here, such as clearing tokens or redirecting
  }
  
  ngOnInit(): void {
  }

  ngOnDestroy(): void {
  }

}

