import { NgModule } from '@angular/core';
import { BackButtonDirective } from './back-button.directive';
import { StopPropgationDirective } from './stop-propgation.directive';


@NgModule({
  imports: [],
  declarations: [ BackButtonDirective,StopPropgationDirective  ],
  exports: [ BackButtonDirective, StopPropgationDirective]
})
export class CustomDirectivesModule { }
