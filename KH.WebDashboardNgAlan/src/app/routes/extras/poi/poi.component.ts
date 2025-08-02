import { Component, ViewChild, inject } from '@angular/core';
import { STColumn, STComponent } from '@delon/abc/st';
import { ModalHelper } from '@delon/theme';
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { SHARED_IMPORTS } from '@shared';
import { NzMessageService } from 'ng-zorro-antd/message';

import { ExtrasPoiEditComponent } from './edit/edit.component';

@Component({
  selector: 'app-extras-poi',
  templateUrl: './poi.component.html',
  imports: SHARED_IMPORTS
})
export class ExtrasPoiComponent {
  private readonly msg = inject(NzMessageService);
  private readonly modal = inject(ModalHelper);
  private readonly i18nSrv = inject(ALAIN_I18N_TOKEN);

  @ViewChild('st', { static: true })
  st!: STComponent;
  s = {
    pi: 1,
    ps: 10,
    user_id: '',
    s: '',
    q: ''
  };
  url = '/pois';
  columns: STColumn[] = [
    { title: this.i18nSrv.fanyi('poi.id'), index: 'id', width: '100px' },
    { title: this.i18nSrv.fanyi('poi.store_name'), index: 'name' },
    { title: this.i18nSrv.fanyi('poi.branch_name'), index: 'branch_name' },
    { title: this.i18nSrv.fanyi('poi.status'), index: 'status_str', width: '100px' },
    {
      title: this.i18nSrv.fanyi('poi.operation'),
      width: '180px',
      buttons: [
        {
          text: this.i18nSrv.fanyi('poi.edit'),
          type: 'modal',
          modal: {
            component: ExtrasPoiEditComponent,
            paramsName: 'i'
          },
          click: () => this.msg.info(this.i18nSrv.fanyi('poi.callback_refresh'))
        },
        { text: this.i18nSrv.fanyi('poi.photo'), click: () => this.msg.info('click photo') },
        { text: this.i18nSrv.fanyi('poi.sku'), click: () => this.msg.info('click sku') }
      ]
    }
  ];

  add(): void {
    this.modal.createStatic(ExtrasPoiEditComponent, { i: { id: 0 } }).subscribe(() => {
      this.st.load();
      this.msg.info(this.i18nSrv.fanyi('poi.callback_refresh'));
    });
  }
}
