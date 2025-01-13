import { AfterViewInit, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { NbIconConfig } from '@nebular/theme';
import { Config } from 'ngx-easy-table';
import { apiColumns } from '../../@core/fakeApiData/ApiColumns';
import { apiDataItems } from '../../@core/fakeApiData/ApiDataItems';
import { PieChartTotal } from '@app/@core/models/client/echart';
import { I18nService } from '@app/@i18n/services/i18n.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit, AfterViewInit {

  constructor(
    private i18nService: I18nService,
    private translateService: TranslateService,
  ) {

    // this.formRandomNumber = generateRandomNumberBasedOnDate();
    // this.loadExample(this.examples[0]);
  }
  ngOnInit(): void {
  }

  ngAfterViewInit(): void {

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



}

