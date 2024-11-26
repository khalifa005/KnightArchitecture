import { Component } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  isCollapsed = false;
  // constructor(private translate: TranslateService) {
  //   this.translate.addLangs(['en_US', 'ar_EG']);
  //   this.translate.setDefaultLang('en_US');
  // }

  // switchLanguage(lang: string) {
  //   this.translate.use(lang);
  // }
}
