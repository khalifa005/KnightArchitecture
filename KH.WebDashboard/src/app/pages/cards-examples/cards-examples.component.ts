import { Component } from '@angular/core';
import { I18nService } from '../../@i18n/services/i18n.service';
import { LanguageEnum } from '../../@i18n/models/language.enum';
import { PieChartTotal } from '../../@core/models/client/echart';

@Component({
  selector: 'app-cards-examples',
  templateUrl: './cards-examples.component.html',
  styleUrl: './cards-examples.component.scss'
})
export class CardsExamplesComponent {

  constructor(
    private i18nService: I18nService,
  ) {
  
    // this.formRandomNumber = generateRandomNumberBasedOnDate();
    // this.loadExample(this.examples[0]);
    }
  
statusColumns = [
  { key: 1, title: this.i18nService.language==  LanguageEnum.En ? 'Assigned' : 'مسجلة' },
  { key: 2, title: this.i18nService.language==  LanguageEnum.En ? 'In progress' : 'جاري العمل عليها' },
  { key: 3, title: this.i18nService.language==  LanguageEnum.En ? 'Suspended' : 'موقوفة' },
];
apiDataForStatus: PieChartTotal[] = [
  { Id: 1, Total: 100 },
  { Id: 2, Total: 200 },
  { Id: 3, Total: 250 }
];

zonesColumns = [
  { key: 1, title: this.i18nService.language==  LanguageEnum.En ? 'Riyadh' : 'الرياض' },
  { key: 2, title: this.i18nService.language==  LanguageEnum.En ? 'Jeedah' : 'جدة' },
  { key: 3, title: this.i18nService.language==  LanguageEnum.En ? 'Mekka' : 'مكة المكرمة' },
  { key: 4, title: this.i18nService.language==  LanguageEnum.En ? 'Taif' : 'لطائف' },
];

apiDataForZones: PieChartTotal[] = [
  { Id: 1, Total: 100 },
  { Id: 2, Total: 200 },
  { Id: 3, Total: 300 },
  { Id: 4, Total: 500 }
];
}
