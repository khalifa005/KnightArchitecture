import { Component, inject } from '@angular/core';
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { SHARED_IMPORTS } from '@shared';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-helpcenter',
  templateUrl: './helpcenter.component.html',
  imports: SHARED_IMPORTS
})
export class HelpCenterComponent {
  readonly msg = inject(NzMessageService);
  private readonly i18nSrv = inject(ALAIN_I18N_TOKEN);
  type = '';
  q = '';

  quick(key: string): void {
    this.q = key;
    this.search();
  }

  search(): void {
    this.msg.success(this.i18nSrv.fanyi('helpcenter.search').replace('{query}', this.q));
  }
}
