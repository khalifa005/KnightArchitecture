import { Component, inject } from '@angular/core';
import { STColumn } from '@delon/abc/st';
import { SFSchema } from '@delon/form';
import { SHARED_IMPORTS } from '@shared';
import { I18NService } from '@core';

@Component({
  selector: 'app-delon-form',
  templateUrl: './form.component.html',
  imports: SHARED_IMPORTS
})
export class DelonFormComponent {
  private readonly i18nSrv = inject(I18NService);
  params: any = {};
  url = `/user`;
  searchSchema: SFSchema = {
    properties: {
      no: {
        type: 'string',
        title: this.i18nSrv.fanyi('form.id')
      }
    }
  };
  columns: STColumn[] = [
    { title: this.i18nSrv.fanyi('form.id'), index: 'no' },
    { title: this.i18nSrv.fanyi('form.call_count'), type: 'number', index: 'callNo' },
    { title: this.i18nSrv.fanyi('form.avatar'), type: 'img', width: '50px', index: 'avatar' },
    { title: this.i18nSrv.fanyi('form.time'), type: 'date', index: 'updatedAt' }
  ];
}
