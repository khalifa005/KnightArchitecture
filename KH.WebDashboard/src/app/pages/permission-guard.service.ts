import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { NgxPermissionsService } from 'ngx-permissions';
import { Router } from '@angular/router';
import { AuthService } from '@app/@auth/services/custom-auth-service';
import { UserRoleEnum } from '@app/@core/enums/roles.enum';

export const PermissionsGuard: CanActivateFn = (route, state) => {
  const permissionsService = inject(NgxPermissionsService);
  const router = inject(Router);
  const authService = inject(AuthService);

  const requiredPermissions = route.data['permissions'];
  const userData = authService.getUser();
  const allUserRoles = userData.roles;

  // Allow access if the user is a Super Admin
  if (allUserRoles.includes(UserRoleEnum.SuperAdmin.toString())) {
    return true;
  }

  // Check for other required permissions
  return permissionsService.hasPermission(requiredPermissions).then((hasPermission) => {
    if (!hasPermission) {
      router.navigate(['/unauthorized']); // Redirect to an unauthorized page
      return false;
    }
    return true;
  });
};
