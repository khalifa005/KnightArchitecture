import { CommonModule } from '@angular/common';
import {
  ModuleWithProviders,
  NgModule,
  Optional,
  SkipSelf,
} from '@angular/core';
import {
  NbAuthJWTToken,
  NbAuthModule,
  NbPasswordAuthStrategy,
} from '@nebular/auth';
import { NbRoleProvider, NbSecurityModule } from '@nebular/security';

import { throwIfAlreadyLoaded } from './module-import-guard';
import {
  NavigationService,
  LayoutService,
  StateService,
  RoleProvider,
  StorageService,
} from './utils.ts';
import { AutocompleteTypeComponent } from './ngx-formly/components/autocomplete-type.component';
import { AutocompleteemTypeComponent } from './ngx-formly/components/autocompleteem-type.component';
import { NbInputFormlyComponent } from './ngx-formly/components/nb-input-formly';
import { FormlyFieldFileComponent } from './ngx-formly/components/formly-field-file.component';
import { FileInputComponent } from './ngx-formly/components/file/file-input/file-input.component';
import { FileTypeComponent } from './ngx-formly/components/file/file-type/file-type.component';
import { FileUploadComponent } from './ngx-formly/components/file/file-upload/file-upload.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { FormlyModule } from '@ngx-formly/core';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTreeModule } from '@angular/material/tree';
import { FormlyMatDatepickerModule } from '@ngx-formly/material/datepicker';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { FileSizePipe } from './ngx-formly/pipes/file-size.pipe';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { TimepickerTypeComponent } from './ngx-formly/components/timepicker-type.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { FormlyDateComponent } from './ngx-formly/components/formly-date/formly-date.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NbDatepickerModule, NbIconModule, NbInputModule } from '@nebular/theme';
import { NbEvaIconsModule } from '@nebular/eva-icons';
import { CustomErrorMessageComponent } from './ngx-formly/components/file/custom-error-message/custom-error-message.component';
import { FileUploadErrorMessagePipe } from './ngx-formly/pipes/file-upload-error-message.pipe';

export const NB_CORE_PROVIDERS = [
  { provide: NbRoleProvider, useClass: RoleProvider }, // provide the class
  ...(NbAuthModule.forRoot({
    strategies: [
      NbPasswordAuthStrategy.setup({
        name: 'email',
        baseEndpoint: 'api/v1',
        login: {
          endpoint: '/auth/sign-in',
          method: 'post',
        },
        register: {
          endpoint: '/auth/sign-up',
          method: 'post',
        },
        logout: {
          endpoint: '/auth/sign-out',
          redirect: {
            success: '/auth/login',
            failure: null,
          },
          method: 'post',
        },
        requestPass: {
          endpoint: '/auth/request-pass',
          method: 'post',
        },
        resetPass: {
          endpoint: '/auth/reset-pass',
          method: 'post',
        },
        refreshToken: {
          endpoint: '/auth/refresh-token',
          method: 'post',
          redirect: {
            success: '/auth/login',
            failure: null,
          },
        },
        token: {
          class: NbAuthJWTToken,
        },
      }),
    ],
    forms: {
      login: {
        strategy: 'email',
      },
      register: {},
    },
  }).providers || []),

  LayoutService,
  StateService,
  StorageService,
  NavigationService,
];


const COMPONENTS: any = [
  AutocompleteTypeComponent,
  AutocompleteemTypeComponent,
  TimepickerTypeComponent,
  NbInputFormlyComponent,
  FormlyFieldFileComponent,
  FileSizePipe,
  FileUploadErrorMessagePipe,

  FormlyDateComponent,
  FileInputComponent,
  FileUploadComponent,
  FileTypeComponent,
  CustomErrorMessageComponent
];

const Common_MODULES = [
  ReactiveFormsModule,
  FormsModule,
  TranslateModule,
  FormlyModule.forChild(),
  FormlyModule,
  // BsDatepickerModule.forRoot(), // Import ngx-bootstrap Datepicker
  MatFormFieldModule,
  MatInputModule,
  MatNativeDateModule,
  MatFormFieldModule,
  NbDatepickerModule,
  MatInputModule,
  MatNativeDateModule,
  TimepickerModule,
  NbIconModule,
  NbEvaIconsModule,
  // NgbModule,
  NbInputModule,
  MatTooltipModule,
  MatFormFieldModule,
  MatTabsModule,
  MatPaginatorModule,
  MatButtonModule,
  MatMenuModule,
  MatIconModule,
  MatInputModule,
  MatAutocompleteModule,
  MatNativeDateModule,
  FormlyMatDatepickerModule,
  MatDatepickerModule,
  NgxMaterialTimepickerModule,
  MatProgressSpinnerModule,
  DragDropModule,

  ScrollingModule,

  MatTreeModule,
  MatGridListModule,
  MatListModule,
  MatProgressBarModule,
  MatTooltipModule,
  // TableModule,
  // FontAwesomeModule,
];

@NgModule({
  imports: [
    CommonModule,
    ...Common_MODULES,
    NbSecurityModule.forRoot({
      accessControl: {
        guest: {
          edit: ['role_guest'],
        },
        user: {
          parent: 'guest',
          edit: ['role_user'],
        },
        reseller: {
          parent: 'user',
          edit: ['role_reseller'],
        },
        admin: {
          parent: 'reseller',
          edit: ['role_admin', 'ftp_credential_role', 'user_coupon'],
        },
      },
    }),
  ],
  exports: [NbAuthModule,...Common_MODULES, ...COMPONENTS],
  declarations: [...COMPONENTS],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }

  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [...NB_CORE_PROVIDERS],
    };
  }
}
