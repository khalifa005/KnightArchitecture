import { DOCUMENT } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, booleanAttribute, inject } from '@angular/core';
import { I18NService } from '@core';
import { ALAIN_I18N_TOKEN, I18nPipe, SettingsService, RTLService } from '@delon/theme';
import { Store } from '@ngrx/store';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { decrement, increment } from 'src/app/store/counter/counter.actions';
import { switchArDirection } from 'src/app/store/direction/direction.action';
@Component({
  selector: 'header-i18n',
  template: `
    @if (showLangText) {
      <div nz-dropdown [nzDropdownMenu]="langMenu" nzPlacement="bottomRight">
        <i nz-icon nzType="global"></i>
        {{ 'menu.lang' | i18n }}
        <i nz-icon nzType="down"></i>
      </div>
    } @else {
     
  
      <!-- <button class="btn btn-primary" (click)="changeLanguage('en')">direction switcher</button>
      <button class="btn btn-primary" (click)="changeLanguage('ar')">direction switcher</button>
      <button class="btn btn-primary" (click)="decraeseCounter()">Counter decrase</button>
      <button class="btn btn-primary" (click)="increaseCounter()">Counter increase </button> -->
      <i nz-dropdown [nzDropdownMenu]="langMenu" nzPlacement="bottomRight" nz-icon nzType="global"></i>
    }
    <nz-dropdown-menu #langMenu="nzDropdownMenu">
      <ul nz-menu>
        @for (item of langs; track $index) {
          <li nz-menu-item [nzSelected]="item.code === curLangCode" (click)="change(item.code)">
            <span role="img" [attr.aria-label]="item.text" class="pr-xs">{{ item.abbr }}</span>
            {{ item.text }}
          </li>
        }
      </ul>
    </nz-dropdown-menu>
  `,
  host: {
    '[class.flex-1]': 'true'
  },
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [I18nPipe, NzDropDownModule, NzIconModule, NzMenuModule]
})
export class HeaderI18nComponent {
  private readonly settings = inject(SettingsService);
  private readonly i18n = inject<I18NService>(ALAIN_I18N_TOKEN);
  private readonly doc = inject(DOCUMENT);
  private readonly rtl = inject(RTLService);
  private readonly store = inject(Store);
  /** Whether to display language text */
  @Input({ transform: booleanAttribute }) showLangText = true;

  get langs(): Array<{ code: string; text: string; abbr: string }> {
    return this.i18n.getLangs();
  }

  get curLangCode(): string {
    return this.settings.layout.lang;
  }

  changeLanguage(lang:string) {
    //for testing the state management
    // <button class="btn btn-primary" (click)="changeLanguage()">direction switcher</button>
    this.store.dispatch(switchArDirection({lang:lang}));
  }


  decraeseCounter() {
    this.store.dispatch(decrement())
  }
  increaseCounter() {
    //for testing the state management
    this.store.dispatch(increment())
  }
  change(lang: string): void {
    const spinEl = this.doc.createElement('div');
    spinEl.setAttribute('class', `page-loading ant-spin ant-spin-lg ant-spin-spinning`);
    spinEl.innerHTML = `<span class="ant-spin-dot ant-spin-dot-spin"><i></i><i></i><i></i><i></i></span>`;
    this.doc.body.appendChild(spinEl);

    this.i18n.loadLangData(lang).subscribe(res => {
      this.i18n.use(lang, res);
      this.settings.setLayout('lang', lang);
      if (lang === 'ar' || lang === 'en-US') {
        this.rtl.toggle();
      }
      setTimeout(() => this.doc.location.reload());
    });
  }
}
