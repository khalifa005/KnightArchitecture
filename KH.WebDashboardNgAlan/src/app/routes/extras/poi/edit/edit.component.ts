import { Component, OnInit, inject } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { SHARED_IMPORTS } from '@shared';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-extras-poi-edit',
  templateUrl: './edit.component.html',
  imports: SHARED_IMPORTS
})
export class ExtrasPoiEditComponent implements OnInit {
  readonly msgSrv = inject(NzMessageService);
  private readonly modal = inject(NzModalRef);
  readonly http = inject(_HttpClient);
  private readonly i18nSrv = inject(ALAIN_I18N_TOKEN);

  i: any;
  cat: string[] = ['poi.food', 'poi.food_cantonese', 'poi.food_cantonese_zhanjiang'];

  ngOnInit(): void {
    if (this.i.id > 0) {
      this.http.get('/pois').subscribe(res => (this.i = res.list[0]));
    }
  }

  save(): void {
    this.http.get('/pois').subscribe(() => {
      this.msgSrv.success(this.i18nSrv.fanyi('poi.save_success'));
      this.modal.destroy(true);
    });
  }

  close(): void {
    this.modal.destroy();
  }
}
