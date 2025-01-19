import { NgModule } from '@angular/core';
import { NbAlertModule, NbCardModule, NbFormFieldModule, NbIconModule, NbMenuModule, NbProgressBarModule, NbSelectModule } from '@nebular/theme';

import { ThemeModule } from '../@theme/theme.module';
import { PagesComponent } from './pages.component';

import { PagesRoutingModule } from './pages-routing.module';
import { TranslateModule } from '@ngx-translate/core';
import { HomeComponent } from './home/home.component';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../@shared/shared.module';
import { FormlyFormComponent } from './formly-form/formly-form.component';
import { FormlyModule } from '@ngx-formly/core';
import { NgxDyTableComponent } from './ngx-dy-table/ngx-dy-table.component';
import { BootstrapExamplesComponent } from './bootstrap-examples/bootstrap-examples.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { CardsExamplesComponent } from './cards-examples/cards-examples.component';
import { IconsExamplesComponent } from './icons-examples/icons-examples.component';
import { InputsExamplesComponent } from './inputs-examples/inputs-examples.component';
import { ReactiveFormExamplesComponent } from './reactive-form-examples/reactive-form-examples.component';
import { BasicFormExampleComponent } from './reactive-form-examples/components/basic-form-example/basic-form-example.component';
import { CleanFormExampleComponent } from './reactive-form-examples/components/clean-form-example/clean-form-example.component';
import { AdvancedFormExampleComponent } from './reactive-form-examples/components/advanced-form-example/advanced-form-example.component';
import { WorkingDaysFormComponent } from './reactive-form-examples/components/advanced-form-example/components/working-days-form/working-days-form.component';
import { PermissionsExampleComponent } from './permissions-example/permissions-example.component';
import { NgxPermissionsModule } from 'ngx-permissions';
import { ChatComponent } from './chat/chat.component';
import { CChatComponent } from './cchat/cchat.component';
import { TabsComponent } from './tabs/tabs.component';
import { ButtonsComponent } from './buttons/buttons.component';
import { PipesComponent } from './pipes/pipes.component';
import { SpinnerComponent } from './spinner/spinner.component';
import { GoogleMapsModule } from '@angular/google-maps';
import { MapsComponent } from './maps/maps.component';
import { PlaygroundComponent } from './playground/playground.component';
import { TaskListHeaderComponent } from './playground/components/task-list-header/task-list-header.component';
import { TaskListNavComponent } from './playground/components/task-list-nav/task-list-nav.component';
import { TaskListComponent } from './playground/components/task-list/task-list.component';
import { TaskListFiltersComponent } from './playground/components/task-list-filters/task-list-filters.component';

@NgModule({
  imports: [
    PagesRoutingModule,
    ThemeModule,
    NbMenuModule,
    TranslateModule,
    FormsModule,
    FormlyModule,
    NbFormFieldModule,
    GoogleMapsModule,
    SharedModule,
    NgxPermissionsModule.forChild(),
    MatButtonModule,
    MatToolbarModule,
    MatIconModule,
    MatSidenavModule,
    MatListModule,
    MatCardModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,

    MatFormFieldModule, MatInputModule, MatDatepickerModule

  ],
  declarations: [
    PagesComponent,
    HomeComponent,
    FormlyFormComponent,
    NgxDyTableComponent,
    BootstrapExamplesComponent,
    CardsExamplesComponent,
    IconsExamplesComponent,
    InputsExamplesComponent,
    ReactiveFormExamplesComponent,
    BasicFormExampleComponent,
    CleanFormExampleComponent,
    AdvancedFormExampleComponent,
    WorkingDaysFormComponent,
    PermissionsExampleComponent,
    ChatComponent,
    CChatComponent,
    TabsComponent,
    ButtonsComponent,
    PipesComponent,
    SpinnerComponent,
    MapsComponent,
    PlaygroundComponent,
    TaskListHeaderComponent,
    TaskListNavComponent,
    TaskListComponent,
    TaskListFiltersComponent

  ],
})
export class PagesModule {
}
