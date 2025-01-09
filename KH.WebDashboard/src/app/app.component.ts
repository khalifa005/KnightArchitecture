import { Component } from '@angular/core';
import { I18nService } from './@i18n/services/i18n.service';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'nebular-dashboard';
  constructor(
    // private rolesService: RolesService,
    private i18nService: I18nService,
  ) {
    this.i18nService.init(environment.defaultLanguage, environment.supportedLanguages);

    // the lang to use, if the lang isn't available, it will use the current loader to get them
  }

  ngOnInit(): void {
    // const test  = this.rolesService.apiVversionRolesIdGet();
  }

}
