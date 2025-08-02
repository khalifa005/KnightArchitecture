import { Injectable, inject } from '@angular/core';
import { ALAIN_I18N_TOKEN } from '@delon/theme';

@Injectable()
export class TransferService {
  private readonly i18nSrv = inject(ALAIN_I18N_TOKEN);
  step = 1;

  /**
   * Payment Account
   */
  pay_account = '';

  /**
   * Receiver Account Type
   */
  receiver_type: 'alipay' | 'bank' = 'alipay';

  get receiver_type_str(): string {
    return this.receiver_type === 'alipay' ? this.i18nSrv.fanyi('transfer.alipay') : this.i18nSrv.fanyi('transfer.bank');
  }

  /**
   * Receiver Account
   */
  receiver_account = '';

  /**
   * Receiver Name
   */
  receiver_name = '';

  /**
   * Amount
   */
  amount = 500;

  /**
   * Payment Password
   */
  password = '123456';

  again(): void {
    this.step = 0;
    this.pay_account = 'ant-design@alipay.com';
    this.receiver_type = 'alipay';
    this.receiver_account = 'test@example.com';
    this.receiver_name = 'asdf';
    this.amount = 500;
  }

  constructor() {
    this.again();
  }
}
