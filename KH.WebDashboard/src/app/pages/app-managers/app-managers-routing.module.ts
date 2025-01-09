import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppManagersComponent } from './app-managers.component';
import { PermissoinsManagerComponent } from './components/permissoins-manager/permissoins-manager.component';
import { PermissionsGuard } from '../permission-guard.service';
import { RolesManagerComponent } from './components/roles-manager/roles-manager.component';
import { DepartmentManagerComponent } from './components/department-manager/department-manager.component';
import { AuditManagerComponent } from './components/audit-manager/audit-manager.component';
import { UserManagerComponent } from './components/user-manager/user-manager.component';


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
    {
      path: 'audit',
      component: AuditManagerComponent,
      canActivate: [PermissionsGuard],
      data: { permissions: ['manage-permissions'] },
    },
    {
      path: 'users',
      component: UserManagerComponent,
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
