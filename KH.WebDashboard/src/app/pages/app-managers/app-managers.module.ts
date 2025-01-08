import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AppManagersRoutingModule } from './app-managers-routing.module';
import { AppManagersComponent } from './app-managers.component';
import { SharedModule } from '@app/@shared/shared.module';
import { ThemeModule } from '@app/@theme/theme.module';
import { TranslateModule } from '@ngx-translate/core';
import { NgxPermissionsModule } from 'ngx-permissions';
import { PermissoinsManagerComponent } from './components/permissoins-manager/permissoins-manager.component';
import { RolesManagerComponent } from './components/roles-manager/roles-manager.component';
import { ListRolesComponent } from './components/roles-manager/components/list-roles/list-roles.component';
import { AddRoleComponent } from './components/roles-manager/components/add-role/add-role.component';
import { DetailsRoleComponent } from './components/roles-manager/components/details-role/details-role.component';
import { ListRolesBasicTableComponent } from './components/roles-manager/components/list-roles-basic-table/list-roles-basic-table.component';
import { DepartmentManagerComponent } from './components/department-manager/department-manager.component';
import { AddDepartmentComponent } from './components/department-manager/components/add-department/add-department.component';
import { DetailsDepartmentComponent } from './components/department-manager/components/details-department/details-department.component';
import { ListDepartmentComponent } from './components/department-manager/components/list-department/list-department.component';
import { TreeviewModule } from '@app/@external-lib-override/treeview/treeview.module';

@NgModule({
  declarations: [
    AppManagersComponent,
    PermissoinsManagerComponent,
    RolesManagerComponent,
    ListRolesComponent,
    AddRoleComponent,
    DetailsRoleComponent,
    ListRolesBasicTableComponent,
    DepartmentManagerComponent,
    AddDepartmentComponent,
    DetailsDepartmentComponent,
    ListDepartmentComponent,
  ],
  imports: [
    CommonModule,
    AppManagersRoutingModule,

    ThemeModule,
    TranslateModule,
    SharedModule,
    NgxPermissionsModule,

    TreeviewModule,
  ]
})
export class AppManagersModule { }
