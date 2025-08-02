import { Component, ElementRef, OnInit, Renderer2, inject } from '@angular/core';
import { NavigationEnd, NavigationError, RouteConfigLoadStart, Router, RouterOutlet } from '@angular/router';
import { TitleService, VERSION as VERSION_ALAIN, stepPreloader, ALAIN_I18N_TOKEN, AlainI18NService } from '@delon/theme';
import { environment } from '@env/environment';
import { NzModalService } from 'ng-zorro-antd/modal';
import { VERSION as VERSION_ZORRO } from 'ng-zorro-antd/version';
import { StartupService } from '@core';

@Component({
  selector: 'app-root',
  template: `<router-outlet />`,
  imports: [RouterOutlet]
})
export class AppComponent implements OnInit {
  private readonly router = inject(Router);
  private readonly titleSrv = inject(TitleService);
  private readonly modalSrv = inject(NzModalService);
  private readonly i18nSrv = inject<AlainI18NService>(ALAIN_I18N_TOKEN);
  private readonly startupSrv = inject(StartupService);

  private donePreloader = stepPreloader();

  constructor(el: ElementRef, renderer: Renderer2) {
    renderer.setAttribute(el.nativeElement, 'ng-alain-version', VERSION_ALAIN.full);
    renderer.setAttribute(el.nativeElement, 'ng-zorro-version', VERSION_ZORRO.full);
  }

  ngOnInit(): void {
    let configLoad = false;
    this.router.events.subscribe(ev => {
      if (ev instanceof RouteConfigLoadStart) {
        configLoad = true;
      }
      if (configLoad && ev instanceof NavigationError) {
        // Debug: Check if translations are available
        console.log('Current lang:', this.i18nSrv.currentLang);
        console.log('Translation test:', this.i18nSrv.fanyi('error.route.load.reminder'));
        console.log('Known translation test:', this.i18nSrv.fanyi('menu.dashboard'));

        // Use fallback values if translations are not loaded yet
        const title = this.i18nSrv.fanyi('error.route.load.reminder') || 'Reminder';
        const content = environment.production
          ? (this.i18nSrv.fanyi('error.route.load.production.message') || 'The application may have released a new version, please click refresh to take effect.')
          : `${this.i18nSrv.fanyi('error.route.load.failed') || 'Reminder: Unable to load route:'}${ev.url}`;
        const okText = this.i18nSrv.fanyi('error.route.load.refresh') || 'Refresh';
        const cancelText = this.i18nSrv.fanyi('error.route.load.ignore') || 'Ignore';

        this.modalSrv.confirm({
          nzTitle: title,
          nzContent: content,
          nzCancelDisabled: false,
          nzOkText: okText,
          nzCancelText: cancelText,
          nzOnOk: () => location.reload()
        });
      }
      if (ev instanceof NavigationEnd) {
        this.donePreloader();
        this.titleSrv.setTitle();
        this.modalSrv.closeAll();
      }
    });
  }
}
