import { Routes } from "@angular/router";
import { ListRoleComponent } from "./list-role/list-role.component";
import { RoleDetailsComponent } from "./role-details/role-details.component";
import { AddRoleComponent } from "./add-role/add-role.component";

export const routes: Routes = [
   { path: '', redirectTo: 'list-roles', pathMatch: 'full' },
   { path: 'list-roles', component: ListRoleComponent },
   { path: 'role-details', component: RoleDetailsComponent }, //if it will be open in a window remove the route
   { path: 'add-role', component: AddRoleComponent },
   { path: 'role-history/:id', loadComponent: () => import('./role-history/role-history.component').then(m => m.RoleHistoryComponent) },

]