import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PagesComponent } from './pages.component';
import { NotFoundComponent } from '../@theme/components';
import { HomeComponent } from './home/home.component';
import { FormlyFormComponent } from './formly-form/formly-form.component';
import { NgxDyTableComponent } from './ngx-dy-table/ngx-dy-table.component';
import { BootstrapExamplesComponent } from './bootstrap-examples/bootstrap-examples.component';
import { CardsExamplesComponent } from './cards-examples/cards-examples.component';
import { IconsExamplesComponent } from './icons-examples/icons-examples.component';
import { InputsExamplesComponent } from './inputs-examples/inputs-examples.component';
import { ReactiveFormExamplesComponent } from './reactive-form-examples/reactive-form-examples.component';
import { PermissionsGuard } from './permission-guard.service';
import { PermissionsExampleComponent } from './permissions-example/permissions-example.component';
import { ChatComponent } from './chat/chat.component';
import { CChatComponent } from './cchat/cchat.component';
import { TabsComponent } from './tabs/tabs.component';
import { ButtonsComponent } from './buttons/buttons.component';
import { PipesComponent } from './pipes/pipes.component';
import { SpinnerComponent } from './spinner/spinner.component';
import { MapsComponent } from './maps/maps.component';
import { PlaygroundComponent } from './playground/playground.component';

const routes: Routes = [{
  path: '',
  component: PagesComponent,
  children: [
    {
      path: '',
      redirectTo: 'home',
      pathMatch: 'full',
    },
    {
      path: 'home',
      component: HomeComponent
    },

    {
      path: 'manage',
      loadChildren: () => import('./app-managers/app-managers.module')
        .then(m => m.AppManagersModule),
    },
    {
      path: 'chat',
      component: ChatComponent,
    },
    {
      path: 'cchat',
      component:CChatComponent,
    },
    {
      path: 'dynamic-form',
      component: FormlyFormComponent,
      canActivate: [PermissionsGuard],//after the module has been loaded, making it less secure for scenarios where the module itself contains sensitive logic or data that should not be downloaded by unauthorized users.
      data: { permissions: ['manage-dynamic-form'] }, // Only users with permission can access
    },
    {
      path: 'permissions',
      component: PermissionsExampleComponent,
      canActivate: [PermissionsGuard],
      data: { permissions: ['view-permissions'] }, // Only users with  permission can access
    },
    {
      path: 'dynamic-table',
      component: NgxDyTableComponent,
      canActivate: [PermissionsGuard],
      data: {
        permissions: ['manage-users', 'view-reports'], // Example permissions
      }, // Only users with ADMIN permission can access
      
    },
    {
      path: 'bootstrap-examples',
      component: BootstrapExamplesComponent,
      data: {
        permissions: ['view-bootstrap-examples'], // Example permissions
      }, // Only users with ADMIN permission can access
    },
    {
      path: 'cards-examples',
      component: CardsExamplesComponent
    },
    {
      path: 'icons-examples',
      component: IconsExamplesComponent
    },
    {
      path: 'inputs-examples',
      component: InputsExamplesComponent
    },
    {
      path: 'form-examples',
      component: ReactiveFormExamplesComponent
    },
    {
      path: 'tabs',
      component:TabsComponent 
    },
    {
      path: 'buttons',
      component: ButtonsComponent
    },
    {
      path: 'pipes',
      component: PipesComponent
    },
    {
      path: 'spinner',
      component: SpinnerComponent
    },
    {
      path: 'maps',
      component: MapsComponent
    },
    {
      path: 'Playground',
      component: PlaygroundComponent
    },
    {
      path: '**',
      component: NotFoundComponent,
    },

  ],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PagesRoutingModule {
}
