import { NgModule } from '@angular/core';

import { WelcomeRoutingModule } from './welcome-routing.module';

import { WelcomeComponent } from './welcome.component';
import { TranslateModule } from '@ngx-translate/core';


@NgModule({
  imports: [
    WelcomeRoutingModule,
    TranslateModule,
  ],
  declarations: [WelcomeComponent],
  exports: [WelcomeComponent]
})
export class WelcomeModule { }
