import { Component, inject } from '@angular/core';
import { SFSchema } from '@delon/form';
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { SHARED_IMPORTS } from '@shared';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-basic-list-edit',
  templateUrl: './edit.component.html',
  imports: SHARED_IMPORTS
})
export class ProBasicListEditComponent {
  private readonly modal = inject(NzModalRef);
  private readonly msgSrv = inject(NzMessageService);
  private readonly i18nSrv = inject(ALAIN_I18N_TOKEN);

  record: any = {};
  schema: SFSchema = {
    properties: {
      title: { type: 'string', title: this.i18nSrv.fanyi('list.task_name'), maxLength: 50 },
      createdAt: { type: 'string', title: this.i18nSrv.fanyi('list.start_time'), format: 'date' },
      owner: {
        type: 'string',
        title: this.i18nSrv.fanyi('list.task_owner'),
        enum: [
          { value: 'asdf', label: 'asdf' },
          { value: 'kase', label: 'Kase' },
          { value: 'cipchk', label: 'cipchk' }
        ]
      },
      subDescription: {
        type: 'string',
        title: this.i18nSrv.fanyi('list.product_description'),
        ui: {
          widget: 'textarea',
          autosize: { minRows: 2, maxRows: 6 }
        }
      }
    },
    required: ['title', 'createdAt', 'owner'],
    ui: {
      spanLabelFixed: 150,
      grid: { span: 24 }
    }
  };

  save(value: any): void {
    this.msgSrv.success(this.i18nSrv.fanyi('list.save_success'));
    this.modal.close(value);
  }

  close(): void {
    this.modal.destroy();
  }
}
