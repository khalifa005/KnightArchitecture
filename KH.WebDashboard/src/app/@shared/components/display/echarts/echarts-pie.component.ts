import { AfterViewInit, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NbThemeService } from '@nebular/theme';
import { TranslateService } from '@ngx-translate/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { NumberOfDaysPieChartTotal, PieChartTotal } from '../../../../@core/models/client/echart';

@Component({
  selector: 'ngx-echarts-pie',
  template: `
    <div echarts [options]="options" class="echart-container"></div>
  `,
  styles: [
    `
      .echart-container {
        width: 100%;
        height: 500px;
      }
    `,
  ],
})
export class EchartsPieComponent implements OnInit, AfterViewInit, OnDestroy {
  options: any = {};
  themeSubscription: any;

  @Input() apiData: PieChartTotal[];
  @Input() passedColumns: any;
  @Input() passedDaysColumns: NumberOfDaysPieChartTotal[];

  private ngUnsubscribe: Subject<void> = new Subject<void>();

  constructor(public localizationService: TranslateService,
    private theme: NbThemeService) {
  }

  ngOnInit() {
    this.buildThePieChart();
  }

  buildThePieChart() {
    this.themeSubscription = this.theme.getJsTheme().subscribe(config => {

      if (this.apiData && this.passedColumns.length > 0) {


        let finalStatus = this.passedColumns.map((x:any) => x.title);

        let data = this.apiData.map(x => ({
          name: this.passedColumns.find((c:any) => c.key === x.Id).title,
          value: x.Total
        }));

        const colors: any = config.variables;
        // const echarts: any = config?.variables?.echarts;
        const echarts: any = config?.variables;

        this.options = {
          backgroundColor: echarts.bg,
          color: [colors?.warningLight, colors?.infoLight, colors?.dangerLight, colors?.successLight, colors?.primaryLight, '#deff59', '#e2ebff'],
          tooltip: {
            trigger: 'item',
            formatter: '{a} <br/>{b} : {c} ({d}%)',
          },
          legend: {
            orient: 'vertical',
            left: 'left',
            data: finalStatus,
            // data: ['Pending', 'In progress', 'Closed', 'Completed', 'Other'],
            textStyle: {
              color: echarts.textColor,
            },
          },
          series: [
            {
              name: 'Status',
              type: 'pie',
              radius: '50%',
              center: ['50%', '50%'],
              data: data,
              itemStyle: {
                emphasis: {
                  shadowBlur: 10,
                  shadowOffsetX: 0,
                  shadowColor: echarts.itemHoverShadowColor,
                },
              },
              label: {
                normal: {
                  textStyle: {
                    color: echarts.textColor,
                  },
                },
              },
              labelLine: {
                normal: {
                  lineStyle: {
                    color: echarts.axisLineColor,
                  },
                },
              },
            },
          ],
        };

      }

    });
  }

  ngAfterViewInit() {

  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
    this.themeSubscription.unsubscribe();
  }
}
