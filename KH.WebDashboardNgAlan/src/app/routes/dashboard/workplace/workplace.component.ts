import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { G2RadarModule } from '@delon/chart/radar';
import { _HttpClient } from '@delon/theme';
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { SHARED_IMPORTS } from '@shared';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzMessageService } from 'ng-zorro-antd/message';
import { zip } from 'rxjs';

@Component({
  selector: 'app-dashboard-workplace',
  templateUrl: './workplace.component.html',
  styleUrls: ['./workplace.component.less'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [...SHARED_IMPORTS, NzAvatarModule, G2RadarModule]
})
export class DashboardWorkplaceComponent implements OnInit {
  private readonly http = inject(_HttpClient);
  readonly msg = inject(NzMessageService);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly i18nSrv = inject(ALAIN_I18N_TOKEN);

  notice: any[] = [];
  activities: any[] = [];
  radarData!: any[];
  loading = true;

  links = [
    {
      title: this.i18nSrv.fanyi('workplace.operation_one'),
      href: ''
    },
    {
      title: this.i18nSrv.fanyi('workplace.operation_two'),
      href: ''
    },
    {
      title: this.i18nSrv.fanyi('workplace.operation_three'),
      href: ''
    },
    {
      title: this.i18nSrv.fanyi('workplace.operation_four'),
      href: ''
    },
    {
      title: this.i18nSrv.fanyi('workplace.operation_five'),
      href: ''
    },
    {
      title: this.i18nSrv.fanyi('workplace.operation_six'),
      href: ''
    }
  ];
  members = [
    {
      id: 'members-1',
      title: this.i18nSrv.fanyi('workplace.science_brick_group'),
      logo: 'https://gw.alipayobjects.com/zos/rmsportal/WdGqmHpayyMjiEhcKoVE.png',
      link: ''
    },
    {
      id: 'members-2',
      title: this.i18nSrv.fanyi('workplace.programmer_daily'),
      logo: 'https://gw.alipayobjects.com/zos/rmsportal/zOsKZmFRdUtvpqCImOVY.png',
      link: ''
    },
    {
      id: 'members-3',
      title: this.i18nSrv.fanyi('workplace.design_team'),
      logo: 'https://gw.alipayobjects.com/zos/rmsportal/dURIMkkrRFpPgTuzkwnB.png',
      link: ''
    },
    {
      id: 'members-4',
      title: this.i18nSrv.fanyi('workplace.secondary_girl_group'),
      logo: 'https://gw.alipayobjects.com/zos/rmsportal/sfjbOqnsXXJgNCjCzDBL.png',
      link: ''
    },
    {
      id: 'members-5',
      title: this.i18nSrv.fanyi('workplace.learn_computer'),
      logo: 'https://gw.alipayobjects.com/zos/rmsportal/siCrBXXhmvTQGWPNLBow.png',
      link: ''
    }
  ];

  ngOnInit(): void {
    zip(this.http.get('/chart'), this.http.get('/api/notice'), this.http.get('/api/activities')).subscribe(
      ([chart, notice, activities]: [any, any, any]) => {
        this.radarData = chart.radarData;
        this.notice = notice;
        this.activities = activities.map((item: any) => {
          item.template = item.template.split(/@\{([^{}]*)\}/gi).map((key: string) => {
            if (item[key]) {
              return `<a>${item[key].name}</a>`;
            }
            return key;
          });
          return item;
        });
        this.loading = false;
        this.cdr.detectChanges();
      }
    );
  }
}
