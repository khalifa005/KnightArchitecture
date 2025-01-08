import { Component, OnDestroy, OnInit } from '@angular/core';
import { MENU_ITEMS } from './pages-menu';
import { StorageService } from '../@core/utils.ts';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from '@app/@auth/services/custom-auth-service';
import { UserRoleEnum } from '@app/@core/enums/roles.enum';
import { UserRole } from '@app/@auth/models/user-role-response';
import { NgxPermissionsService, NgxRolesService } from 'ngx-permissions';
import { loadPermissions } from '@app/@auth/services/permission-management.service';
import { NbMenuItem } from '@nebular/theme';
import { UserApiService } from '@app/@core/api-services/user-api.service';
import { PermissionResponse } from '@app/@core/models/responses/permission-response';
import { ApiResponse } from '@app/@core/models/base/response/custom-api-response';

@Component({
  selector: 'ngx-pages',
  styleUrls: ['pages.component.scss'],
  templateUrl: './pages.component.html',
})
export class PagesComponent implements OnInit, OnDestroy {
  // menu = MENU_ITEMS;
  menu = [];
  allUserPermissions: PermissionResponse[] = [];

  STATIC_PERMISSIONS = [
    'super-admin',
    'view-permissions',
    'view-dynamic-table',
    'view-cards-examples',
    'manage-dynamic-form',
    
    'view-bootstrap-examples',
    'view-icons-examples',
    'manage-inputs-examples',
    'view-form-examples',
    'access-auth',
    'view-not-found',
    'manage-user-management',
    'manage-permissions',
    'manage-users',
  ];

  constructor(
    private storageService: StorageService,
    private authService: AuthService,
    private permissionsService: NgxPermissionsService,
    private rolesService: NgxRolesService,
    private translate: TranslateService) {

    //get from api user roles with permissions
    // const userPermissions = ["view-home"]; // Mock permissions
    // const additionalPermissions = this.extractPermissionKeys(MENU_ITEMS);
    // const extendedPermissions = Array.from(new Set([...userPermissions, ...this.STATIC_PERMISSIONS]));

   
    // Allow access if the user is a Super Admin
    // if (allUserRoles.includes(UserRoleEnum.SuperAdmin.toString())) {
    //   // Super Admin Role
    //   this.rolesService.addRole('SUPER_ADMIN', () => true); // Super admin bypasses permissions
    // }

    //load all user permission and save it in store
    // loadPermissions(this.permissionsService, extendedPermissions ?? []);

    const userData = this.authService.getUser();
    const allUserRoles = userData.roles;
    const selectedUserRole = this.authService.getSelectedRole();
    const userPermsions = authService.getUserPermissions();
    this.menu = this.filterMenuItemsWithMultipleUserRoles(MENU_ITEMS, userPermsions, allUserRoles);

    this.translate.onLangChange.subscribe(() => {
      this.updateMenuItems();
    });

    this.updateMenuItems(); //Initialize menu items
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
  }



  filterMenuItemsWithMultipleUserRoles(items: NbMenuItem[], permissions: string[], roles: string[]): NbMenuItem[] {
    return items.reduce<NbMenuItem[]>((visibleItems, item) => {
      // Recursively filter children
      const children = item.children ? this.filterMenuItemsWithMultipleUserRoles(item.children, permissions, roles) : null;

      // Extract the permissionKey from the data object
      const permissionKey = item.data?.permissionKey;

      // Determine if the item is visible
      const isVisible =
        roles.includes(UserRoleEnum.SuperAdmin.toString()) || // Super-admin can see everything
        permissionKey === 'view-home' || // Items with `view-home` are always visible
        permissionKey === '' || // Items with `view-home` are always visible
        (permissionKey && permissions.includes(permissionKey)) || // User has permission for this item
        (children && children.length > 0); // Parent is visible if it has visible children

      // Include the item if it's visible
      if (isVisible) {
        visibleItems.push({ ...item, children });
      }

      return visibleItems;
    }, []);
  }

  updateMenuItems(): void {
    this.menu = this.menu.map(item => ({
      ...item,
      title: this.translate.instant(item.data?.translationKey || item.title), // Use translationKey from data object
      children: item.children?.map(child => ({
        ...child,
        title: this.translate.instant(child.data?.translationKey || child.title),
      })),
    }));
  }

}