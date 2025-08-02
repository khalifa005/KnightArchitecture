import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { STColumn } from '@delon/abc/st';
import { _HttpClient } from '@delon/theme';
import { SHARED_IMPORTS } from '@shared';
import { NzMessageService } from 'ng-zorro-antd/message';
import { tap } from 'rxjs';

@Component({
  selector: 'app-profile-basic',
  templateUrl: './basic.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: SHARED_IMPORTS
})
export class ProProfileBaseComponent {
  private readonly http = inject(_HttpClient);
  private readonly msg = inject(NzMessageService);

  basicNum = 0;
  amountNum = 0;
  goods = this.http.get('/profile/goods').pipe(
    tap((list: Array<{ num: number; amount: number }>) => {
      list.forEach(item => {
        this.basicNum += Number(item.num);
        this.amountNum += Number(item.amount);
      });
    })
  );
  goodsColumns: STColumn[] = [
    {
      title: 'profile.product_number',
      index: 'id',
      type: 'link',
      click: item => this.msg.success(`show ${item.id}`)
    },
    { title: 'profile.product_name', index: 'name' },
    { title: 'profile.product_barcode', index: 'barcode' },
    { title: 'profile.unit_price', index: 'price', type: 'currency' },
    { title: 'profile.quantity', index: 'num', className: 'text-right' },
    { title: 'profile.amount', index: 'amount', type: 'currency' }
  ];
  progress = this.http.get('/profile/progress');
  progressColumns: STColumn[] = [
    { title: 'profile.time', index: 'time' },
    { title: 'profile.current_progress', index: 'rate' },
    {
      title: 'profile.status',
      index: 'status',
      type: 'badge',
      badge: {
        success: { text: 'profile.success', color: 'success' },
        processing: { text: 'profile.processing', color: 'processing' }
      }
    },
    { title: 'profile.operator_id', index: 'operator' },
    { title: 'profile.time_consumed', index: 'cost' }
  ];
}
