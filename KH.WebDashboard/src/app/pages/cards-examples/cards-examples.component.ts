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

  constructor(
    private i18nService: I18nService,
    private translateService: TranslateService,
  ) {
  
    // this.formRandomNumber = generateRandomNumberBasedOnDate();
    // this.loadExample(this.examples[0]);
    }
  
    statusColumns = [
      { key: 1, title: this.translateService.instant('columns.status.assigned') },
      { key: 2, title: this.translateService.instant('columns.status.inProgress') },
      { key: 3, title: this.translateService.instant('columns.status.suspended') },
    ];
    
    zonesColumns = [
      { key: 1, title: this.translateService.instant('columns.zones.riyadh') },
      { key: 2, title: this.translateService.instant('columns.zones.jeedah') },
      { key: 3, title: this.translateService.instant('columns.zones.mekka') },
      { key: 4, title: this.translateService.instant('columns.zones.taif') },
    ];
    
apiDataForStatus: PieChartTotal[] = [
  { Id: 1, Total: 100 },
  { Id: 2, Total: 200 },
  { Id: 3, Total: 250 }
];
apiDataForZones: PieChartTotal[] = [
  { Id: 1, Total: 100 },
  { Id: 2, Total: 200 },
  { Id: 3, Total: 300 },
  { Id: 4, Total: 500 }
];


  name: string = '';

  submit() {
    console.log('Submitted name:', this.name);
    alert(`Hello, ${this.name}!`);
  }

}
