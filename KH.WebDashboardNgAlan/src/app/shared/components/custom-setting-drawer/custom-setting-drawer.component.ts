import { Component, ChangeDetectionStrategy, inject, ChangeDetectorRef } from '@angular/core';
import { NzMessageService } from 'ng-zorro-antd/message';
import { SettingsService } from '@delon/theme';
import { I18nPipe } from '@delon/theme';

@Component({
   selector: 'custom-setting-drawer',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [I18nPipe],
   template: `
    <div class="custom-setting-drawer">
      <h3>{{ 'setting_drawer.theme_color' | i18n }}</h3>
      <h3>{{ 'setting_drawer.settings' | i18n }}</h3>
      <h3>{{ 'setting_drawer.header' | i18n }}</h3>
      <h3>{{ 'setting_drawer.sidebar' | i18n }}</h3>
      <h3>{{ 'setting_drawer.content' | i18n }}</h3>
      <h3>{{ 'setting_drawer.other' | i18n }}</h3>
      <h3>{{ 'setting_drawer.fixed_header_sidebar' | i18n }}</h3>
      <h3>{{ 'setting_drawer.color_weak_mode' | i18n }}</h3>
      <button>{{ 'setting_drawer.preview' | i18n }}</button>
      <button>{{ 'setting_drawer.reset' | i18n }}</button>
      <button>{{ 'setting_drawer.copy' | i18n }}</button>
      <p>{{ 'setting_drawer.dev_warning' | i18n }}</p>
    </div>
  `
})
export class CustomSettingDrawerComponent {
   private readonly cdr = inject(ChangeDetectorRef);
   private readonly msg = inject(NzMessageService);
   private readonly settingSrv = inject(SettingsService);
} 