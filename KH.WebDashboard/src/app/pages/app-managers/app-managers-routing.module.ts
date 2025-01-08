import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppManagersComponent } from './app-managers.component';
import { PermissoinsManagerComponent } from './components/permissoins-manager/permissoins-manager.component';
import { PermissionsGuard } from '../permission-guard.service';
import { RolesManagerComponent } from './components/roles-manager/roles-manager.component';
import { DepartmentManagerComponent } from './components/department-manager/department-manager.component';


const routes: Routes = [{
  path: '',
  component: AppManagersComponent,
  children: [
    {
      path: '',
      component: PermissoinsManagerComponent,
      canActivate: [PermissionsGuard],
      data: { permissions: ['manage-permissions'] },
    },
    {
      path: 'permissions',
      component: PermissoinsManagerComponent,
      canActivate: [PermissionsGuard],
      data: { permissions: ['manage-permissions'] },
    },
    {
      path: 'roles',
      component: RolesManagerComponent,
      canActivate: [PermissionsGuard],
      data: { permissions: ['manage-permissions'] },
    },
    {
      path: 'departments',
      component: DepartmentManagerComponent,
      canActivate: [PermissionsGuard],
      data: { permissions: ['manage-permissions'] },
    },

],
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppManagersRoutingModule { }
