import { Component, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { GlobalFooterModule } from '@delon/abc/global-footer';
import { DA_SERVICE_TOKEN } from '@delon/auth';
import { ThemeBtnComponent } from '@delon/theme/theme-btn';
import { NzIconModule } from 'ng-zorro-antd/icon';

import { HeaderI18nComponent } from '../basic/widgets/i18n.component';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { selectDirection } from 'src/app/store/direction/direction.selector';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'layout-passport',
  template: `
    <div class="container">
      <header-i18n showLangText="false" class="langs" />
      <div class="wrap">
        <div class="top">
          <div class="head">
        
            <img class="logo" src="./assets/logo-color.svg" />
            <span class="title">Knight Architecture</span>
          </div>
          <div class="desc">By Alan updated by Khalifa</div>
        </div>
        
        <router-outlet />


        <global-footer [links]="links">
          Copyright
          <i nz-icon nzType="copyright"></i> 2023 <a href="//github.com/cipchk" target="_blank">Khalifa</a>
        </global-footer>
      </div>
    </div>
    <theme-btn />
  `,
  styleUrls: ['./passport.component.less'],
  imports: [RouterOutlet, HeaderI18nComponent, GlobalFooterModule, NzIconModule, ThemeBtnComponent]
  // AsyncPipe,
})
export class LayoutPassportComponent implements OnInit {
  private tokenService = inject(DA_SERVICE_TOKEN);

  direction: Observable<string>;
  // dir: string = ""
  private storeService = inject(Store);
  /**
   *
   */
  constructor() {
    // this.direction = this.storeService.select("directionSwitcherxx");
    this.direction = this.storeService.select(selectDirection);
    // direction = {{this.direction | async}}
    // this.direction.subscribe((newValue) => {
    //   this.dir = newValue;
    // });

  }
  links = [
    {
      title: 'Help',
      href: ''
    },
    {
      title: 'Privacy',
      href: ''
    },
    {
      title: 'Terms',
      href: ''
    }
  ];

  ngOnInit(): void {
    this.tokenService.clear();
  }
}
