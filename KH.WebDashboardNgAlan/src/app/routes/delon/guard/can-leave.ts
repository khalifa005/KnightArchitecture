import { inject } from '@angular/core';
import { CanDeactivateFn } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd/modal';
import { Observable } from 'rxjs';
import { I18NService } from '@core';

import { GuardComponent } from './guard.component';

export const canLeave: CanDeactivateFn<GuardComponent> = (): Observable<boolean> => {
  const srv = inject(NzModalService);
  const i18nSrv = inject(I18NService);
  return new Observable(observer => {
    srv.confirm({
      nzTitle: i18nSrv.fanyi('guard.confirm_leave'),
      nzContent: i18nSrv.fanyi('guard.leave_warning'),
      nzOkText: i18nSrv.fanyi('guard.leave'),
      nzCancelText: i18nSrv.fanyi('guard.cancel'),
      nzOnOk: () => {
        observer.next(true);
        observer.complete();
      },
      nzOnCancel: () => {
        observer.next(false);
        observer.complete();
      }
    });
  });
};
