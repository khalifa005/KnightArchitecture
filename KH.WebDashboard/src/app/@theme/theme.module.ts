import { CommonModule } from "@angular/common";
import { ModuleWithProviders, NgModule } from "@angular/core";
import { CORPORATE_THEME } from "./styles/theme.corporate";
import {
    NbActionsModule,
    NbLayoutModule,
    NbMenuModule,
    NbSearchModule,
    NbSidebarModule,
    NbUserModule,
    NbContextMenuModule,
    NbButtonModule,
    NbSelectModule,
    NbIconModule,
    NbThemeModule,
    NbCardModule,
    NbAlertModule,
    NbProgressBarModule,
    NbTabsetModule,
    NbAccordionModule,
    NbInputModule,
    NbCheckboxModule,
    NbDatepickerModule,
  } from '@nebular/theme';
import { NbSecurityModule } from "@nebular/security";
import { NbEvaIconsModule } from "@nebular/eva-icons";

import {
    FooterComponent,
    HeaderComponent,
    HelpComponent,
    GoBackComponent,
    NotFoundComponent
  } from './components';

  import {
    WaitDialogComponent,
    ConfirmDialogComponent,
  } from './dialogs';

  import {
    CapitalizePipe,
    PluralPipe,
    RoundPipe,
    TimingPipe,
    NumberWithCommasPipe,
    ArrayToStringPipe,
    NumberToStringPipe,
    SafePipe,
    StringToNumberPipe,
    SplitPipe,
    SecurePipe,
    RemoveExtension,
    SanitizeHtmlPipe,
    CastPipe
  } from './pipes';

  import {
    OneColumnLayoutComponent,
    ThreeColumnsLayoutComponent,
    TwoColumnsLayoutComponent,
  } from './layouts';
import { TranslateModule } from "@ngx-translate/core";
import { CustomDirectivesModule } from "./directives/directives.modules";
import { SwitcherComponent } from "./components/header/switcher/switcher.component";
import { LanguageSelectorComponent } from "./components/language-selector/language-selector.component";
import { LayoutDirectionSwitcherComponent } from "./components/header/layout-direction-switcher/layout-direction-switcher.component";
import { DEFAULT_THEME } from "./styles/theme.default";
import { YesNoPipe } from "./pipes/yes-no.pipe";

const NB_MODULES : any= [
    NbLayoutModule,
    NbMenuModule,
    NbUserModule,
    NbActionsModule,
    NbButtonModule,
    NbSearchModule,
    NbSidebarModule,
    NbContextMenuModule,
    NbAlertModule,
    NbCardModule,
    NbSecurityModule,
    NbSelectModule,
    NbIconModule,
    NbEvaIconsModule,

    NbCardModule,
    NbAlertModule,
    NbIconModule,
    NbSelectModule,
    NbAccordionModule,
    NbTabsetModule,
    NbProgressBarModule,

    NbInputModule,
    NbCheckboxModule,
    NbDatepickerModule,
  ];
  const COMPONENTS : any = [
    HeaderComponent,
    SwitcherComponent,
    FooterComponent,
    OneColumnLayoutComponent,
    ThreeColumnsLayoutComponent,
    TwoColumnsLayoutComponent,
    HelpComponent,
    ConfirmDialogComponent,
    WaitDialogComponent,
    NotFoundComponent,
    LanguageSelectorComponent,
    LayoutDirectionSwitcherComponent,
    GoBackComponent
  ];

const PIPES = [
  CapitalizePipe,
  PluralPipe,
  RoundPipe,
  TimingPipe,
  YesNoPipe,
  ArrayToStringPipe,
  NumberWithCommasPipe,
  NumberToStringPipe,
  StringToNumberPipe,
  SplitPipe,
  SecurePipe,
  RemoveExtension,
  SanitizeHtmlPipe,
  CastPipe,
  SafePipe
];

@NgModule({
    imports: [CommonModule, TranslateModule, ...NB_MODULES, CustomDirectivesModule],
    exports: [CommonModule, ...NB_MODULES, ...PIPES, ...COMPONENTS, CustomDirectivesModule],
    declarations: [...COMPONENTS, ...PIPES],
  })
  export class ThemeModule {
    static forRoot(): ModuleWithProviders<ThemeModule> {
      return {
        ngModule: ThemeModule,
        providers: [
          ...NbThemeModule.forRoot(
            {
              name: 'default',
            },
            [ DEFAULT_THEME ],
          ).providers || [],
        ],
      };
    }
  }