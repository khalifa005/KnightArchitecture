import { Routes } from "@angular/router";
import { ListUserProfilesComponent } from "./list-user-profiles/list-user-profiles.component";
import { AddUserProfileComponent } from "./add-user-profile/add-user-profile.component";
import { UserPermissionsComponent } from "./user-permissions/user-permissions.component";

export const routes: Routes = [
   { path: '', redirectTo: 'list-user-profiles', pathMatch: 'full' },
   { path: 'list-user-profiles', component: ListUserProfilesComponent },
   { path: 'add-user-profile', component: AddUserProfileComponent },
   { path: 'user-permissions', component: UserPermissionsComponent },
   // { path: 'user-profile-details/:id', component: UserProfileDetailsComponent },
];