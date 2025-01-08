import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable, catchError, map, of } from 'rxjs';
import { AuthService } from './@auth/services/custom-auth-service';
import { UserApiService } from './@core/api-services/user-api.service';
import { ApiResponse } from './@core/models/base/response/custom-api-response';
import { PermissionResponse } from './@core/models/responses/permission-response';
import { NgxPermissionsService } from 'ngx-permissions';
import { loadPermissions } from './@auth/services/permission-management.service';

export const AuthGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
):
  | Observable<boolean | UrlTree>
  | Promise<boolean | UrlTree>
  | boolean
  | UrlTree => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const permissionsService = inject(NgxPermissionsService);

  return authService.getAuthStatus().pipe(
    map((isAuthenticated) => {
      const tokenExpired = authService.isTokenExpired();

      if (!isAuthenticated || tokenExpired) {
        authService.logout();
        return router.createUrlTree(['/auth/login']);
      }

      const permissions = permissionsService.getPermissions();
      
      if (Object.keys(permissions).length === 0) {
        const userPermsions = authService.getUserPermissions();
        console.log('user permissions :', userPermsions);
        loadPermissions(permissionsService, userPermsions ?? []);
      }



      const requiredRoles = route.data['roles'] as string[] | undefined;
      // // If roles are required, validate the user's roles
      // if (requiredRoles && !authService.doesUserHasRquiredRoles(requiredRoles)) {
      //   authService.logout();
      //   return router.createUrlTree(['/auth/login']);
      //   // return router.createUrlTree(['/access-denied']);
      // router.navigate(['/unauthorized']); // Redirect to an unauthorized page

      // }




    }),
    catchError((error) => {
      console.error('Error in AuthGuard:', error);
      authService.logout();
      return of(router.createUrlTree(['/auth/login']));
    })
  );

};
