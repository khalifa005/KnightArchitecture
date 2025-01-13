import { Component } from '@angular/core';
import { I18nService } from '../../@i18n/services/i18n.service';
import { LanguageEnum } from '../../@i18n/models/language.enum';
import { PieChartTotal } from '../../@core/models/client/echart';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-cards-examples',
  templateUrl: './cards-examples.component.html',
  styleUrl: './cards-examples.component.scss'
})
export class CardsExamplesComponent {

  

  name: string = '';

  submit() {
    console.log('Submitted name:', this.name);
    alert(`Hello, ${this.name}!`);
  }

}
