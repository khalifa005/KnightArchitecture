import { Component, inject } from '@angular/core';
import { copy } from '@delon/util/browser';
import { format } from '@delon/util/format';
import { SHARED_IMPORTS, yuan } from '@shared';
import { NzMessageService } from 'ng-zorro-antd/message';
import { I18NService } from '@core';

@Component({
  selector: 'app-util',
  templateUrl: './util.component.html',
  imports: SHARED_IMPORTS
})
export class UtilComponent {
  readonly messageSrv = inject(NzMessageService);
  private readonly i18nSrv = inject(I18NService);

  format_str = 'this is ${name}';
  format_res = '';
  format_obj = JSON.stringify({ name: 'asdf' });

  // yuan
  yuan_str: any;
  yuan_res!: string;

  content = `time ${+new Date()}

    ${this.i18nSrv.fanyi('util.chinese_test')}`;
  onFormat(): void {
    let obj = null;
    try {
      obj = JSON.parse(this.format_obj);
    } catch {
      this.messageSrv.error(this.i18nSrv.fanyi('util.json_parse_error'));
      return;
    }
    this.format_res = format(this.format_str, obj, true);
  }
  onYuan(value: string): void {
    this.yuan_res = yuan(value);
  }
  onCopy(): void {
    copy(`time ${+new Date()}`).then(() => this.messageSrv.success(`success`));
  }
}
