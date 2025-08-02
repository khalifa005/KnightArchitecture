import { Component, inject } from '@angular/core';
import { Lodop, LodopService } from '@delon/abc/lodop';
import { SHARED_IMPORTS } from '@shared';
import { NzMessageService } from 'ng-zorro-antd/message';
import { I18NService } from '@core';

@Component({
  selector: 'app-print',
  templateUrl: './print.component.html',
  imports: SHARED_IMPORTS
})
export class PrintComponent {
  private readonly lodopSrv = inject(LodopService);
  private readonly msg = inject(NzMessageService);
  private readonly i18nSrv = inject(I18NService);

  constructor() {
    this.lodopSrv.lodop.subscribe(({ lodop, ok }) => {
      if (!ok) {
        this.error = true;
        return;
      }
      this.error = false;
      this.msg.success(this.i18nSrv.fanyi('print.printer_loaded'));
      this.lodop = lodop as Lodop;
      this.pinters = this.lodopSrv.printer;
    });
  }

  cog: any = {
    url: 'https://localhost:8443/CLodopfuncs.js',
    printer: '',
    paper: '',
    html: `
      <h1>Title</h1>
      <p>This~!@#$%^&*()——sdilfjnvn</p>
      <p>This~!@#$%^&*()——sdilfjnvn</p>
      <p>This~!@#$%^&*()——sdilfjnvn</p>
      <p>This~!@#$%^&*()——sdilfjnvn</p>
      <p>This~!@#$%^&*()——sdilfjnvn</p>
    `
  };
  error = false;
  lodop: Lodop | null = null;
  pinters: any[] = [];
  papers: string[] = [];

  printing = false;

  reload(options: { url: string } | null = { url: 'https://localhost:8443/CLodopfuncs.js' }): void {
    this.pinters = [];
    this.papers = [];
    this.cog.printer = '';
    this.cog.paper = '';

    this.lodopSrv.cog = { ...this.cog, ...options };
    this.error = false;
    if (options === null) {
      this.lodopSrv.reset();
    }
  }

  changePinter(name: string): void {
    if (this.lodop == null) {
      return;
    }
    this.papers = this.lodop.GET_PAGESIZES_LIST(name, '\n').split('\n');
  }
  print(isPrivew = false): void {
    const LODOP = this.lodop as Lodop;
    LODOP.PRINT_INITA(10, 20, 810, 610, 'Test C-Lodop Remote Print Four Steps');
    LODOP.SET_PRINTER_INDEXA(this.cog.printer);
    LODOP.SET_PRINT_PAGESIZE(0, 0, 0, this.cog.paper);
    LODOP.ADD_PRINT_TEXT(1, 1, 300, 200, 'Below is the source code of this page and its display effect:');
    LODOP.ADD_PRINT_TEXT(20, 10, '90%', '95%', this.cog.html);
    LODOP.SET_PRINT_STYLEA(0, 'ItemType', 4);
    LODOP.NEWPAGEA();
    LODOP.ADD_PRINT_HTM(20, 10, '90%', '95%', this.cog.html);
    if (isPrivew) {
      LODOP.PREVIEW();
    } else {
      LODOP.PRINT();
    }
  }
}
