import { NgxLoginComponent } from './login/login.component';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';


import { NgxAuthRoutingModule } from './auth-routing.module';
import { NbAuthModule } from '@nebular/auth';
import { 
  NbAlertModule,
  NbButtonModule,
  NbCardModule,
  NbCheckboxModule,
  NbIconModule,
  NbInputModule,
  NbLayoutModule,
  NbSelectModule
} from '@nebular/theme';
import { TranslateModule } from '@ngx-translate/core';
import { NgxResetPasswordComponent } from './reset-password/reset-password.component';
import { NgxAuthComponent } from './ngx-auth/ngx-auth.component';
import { LogoutComponent } from './logout/logout.component';
import { NgxPermissionsModule } from 'ngx-permissions';


@NgModule({
  imports: [
    CommonModule,
    NbCardModule,
    NbLayoutModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    NbAlertModule,
    NbSelectModule,
    NbInputModule,
    NbButtonModule,
    NbCheckboxModule,
    NbAlertModule,
    TranslateModule,
    NgxAuthRoutingModule,
    NbIconModule,
    NgxPermissionsModule,
    TranslateModule,
    NbAuthModule,

  ],
  declarations: [
    NgxAuthComponent,
    NgxLoginComponent,
    NgxResetPasswordComponent,
    LogoutComponent
  ],
})
export class NgxAuthModule {
}