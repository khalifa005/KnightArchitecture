import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { PermissionsApiService } from '@app/@core/api-services/permissions-api.service';
import { RoleApiService } from '@app/@core/api-services/role-api.service';
import { AppDefaultValues } from '@app/@core/const/default-values';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { LookupResponse } from '@app/@core/models/base/response/lookup.model';
import { PermissionFilterRequest } from '@app/@core/models/requests/permission-filter-request';
import { RoleFilterRequest } from '@app/@core/models/requests/role-filter-request';
import { UpdateRolePermissionsRequest } from '@app/@core/models/requests/update-role-permissions-request';
import { PermissionResponse } from '@app/@core/models/responses/permission-response';
import { RolesResponse } from '@app/@core/models/responses/roles-response';
import { Logger } from '@app/@core/utils.ts/logger.service';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { TreeviewComponent } from '@app/@external-lib-override/treeview/components/treeview/treeview.component';
import { TreeviewConfig } from '@app/@external-lib-override/treeview/models/treeview-config';
import { TreeviewItem, TreeItem } from '@app/@external-lib-override/treeview/models/treeview-item';

import { TranslateService } from '@ngx-translate/core';
// import { TreeviewComponent, TreeviewItem, TreeviewConfig, TreeItem } from '@samotics/ngx-treeview';
import { id } from 'date-fns/locale';
import { Subject, Subscription, takeUntil } from 'rxjs';

@Component({
  selector: 'app-permissoins-manager',
  templateUrl: './permissoins-manager.component.html',
  styleUrls: ['./permissoins-manager.component.scss']
})
export class PermissoinsManagerComponent implements OnInit, OnDestroy {
  private log = new Logger(PermissoinsManagerComponent.name);

  @ViewChild(TreeviewComponent, { static: false }) treeviewComponent!: TreeviewComponent;
  private subs: Subscription[] = [];
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  showSubmitButton = false;
  currentRoleId = AppDefaultValues.DropDownAllOption;
  searchTerm = '';
  filteredItems: TreeviewItem[] = [];
  isLoading = false;

  roleFilterRequest: RoleFilterRequest = {};
  permissionFilterRequest: PermissionFilterRequest = {};

  rolesAsLookups: LookupResponse[] = [];
  allRolesResponse: RolesResponse[] = [];

  selectedRolePermissions: PermissionResponse[] = [];
  selectedRolePermissionsIds: number[] = [];
  allPermissions: PermissionResponse[] = [];
  


  items: TreeviewItem[] = [];
  config = TreeviewConfig.create({
    hasAllCheckBox: true,
    hasFilter: true,
    hasCollapseExpand: true,
    decoupleChildFromParent: false,
    maxHeight: 400
  });
  selectedParentPermissionId = AppDefaultValues.DropDownAllOption; // Initialize with default
  parentPermissionsAsLookups: LookupResponse[] = []; // To store parent permissions as lookups
  
  constructor(
    private readonly cdr: ChangeDetectorRef,
    private readonly roleApiService: RoleApiService,
    private readonly permissionsApiService: PermissionsApiService,
    public translate: TranslateService,
    private toastNotificationService: ToastNotificationService
  ) {}

  ngOnInit(): void {
    this.fetchRoles();
    this.fetchPermissions();
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  private fetchRoles(): void {
    this.isLoading = true;
    this.roleApiService.listAllRoles(this.roleFilterRequest)
     .pipe(takeUntil(this.ngUnsubscribe)) // Unsubscribe automatically on component destroy
    .subscribe({
      next: (response) => {
        if (response?.statusCode === 200 && response.data) {
          this.allRolesResponse = response.data;
          this.rolesAsLookups = response.data.map(role => new LookupResponse(role.id, role.nameAr, '', role.nameEn));
        } else {
          this.log.error('No roles data received.');
        }
      },
      error: (err) => this.log.error('Error fetching roles:', err),
      complete: () => (this.isLoading = false)
    });
  }

  private fetchPermissions(): void {
    this.isLoading = true;
    this.permissionsApiService.listAllPermissions()
    .pipe(takeUntil(this.ngUnsubscribe)) // Unsubscribe automatically on component destroy
     .subscribe({
      next: (response) => {
        if (response?.statusCode === 200 && response.data) {
          
          this.allPermissions = this.groupChildren(response.data);
          // Populate parent permissions for the dropdown
          this.parentPermissionsAsLookups = [
            // new LookupResponse(AppDefaultValues.DropDownAllOption, this.translate.instant('form.dropdown.showAll'), '', this.translate.instant('form.dropdown.showAll')),
            ...this.allPermissions
                .filter(permission => permission.childrens && permission.childrens.length > 0)
                .map(permission => new LookupResponse(permission.id, permission.nameAr, '', permission.nameEn))
        ];

        // Set default filtered items to all permissions
        this.filteredItems = this.items = this.formateTreeResponse(this.allPermissions);


        } else {
          this.log.error('No permissions data received.');
        }
      },
      error: (err) => this.log.error('Error fetching permissions:', err),
      complete: () => (this.isLoading = false)
    });
  }
 
// Filter parent permissions based on the selected dropdown value
filterParentPermissions(selectedParentId: number): void {
  this.selectedParentPermissionId = selectedParentId;

  if (selectedParentId === AppDefaultValues.DropDownAllOption) {
      this.filteredItems = this.items; // Show all permissions
  } else {
      this.filteredItems = this.items.filter(item => item.value === selectedParentId);
  }

  this.cdr.markForCheck(); // Trigger change detection
}
  private groupChildren(items: PermissionResponse[]): PermissionResponse[] {
    const grouped = new Map<number, PermissionResponse>();

    items.forEach(item => grouped.set(item.id, new PermissionResponse(item)));

    items.forEach(item => {
      if (item.parentId !== null) {
        const parent = grouped.get(item.parentId);
        parent?.childrens.push(grouped.get(item.id)!);
      }
    });

    return Array.from(grouped.values()).filter(item => item.parentId === null);
  }


  // Handle role changes
onSelectedRoleChanged(roleId: number): void {
  this.currentRoleId = roleId;
  const selectedRole = this.allRolesResponse.find(role => role.id === roleId);

  if (selectedRole) {
      this.selectedRolePermissions = selectedRole.permissions;
      this.selectedRolePermissionsIds = selectedRole.permissions.map(permission => permission.id);
      console.log("all selected RolePermissions Ids",this.selectedRolePermissionsIds);
  }

  // Reinitialize data and apply the selected filter
  this.getData(roleId);
  this.filterParentPermissions(this.selectedParentPermissionId);
}

  private getData(roleId: number): void {
    if (roleId === AppDefaultValues.DropDownAllOption) {
      this.resetSelection();
      return;
    }

    this.items = this.formateTreeResponse(this.allPermissions);
    this.updateTreeWithSelectedPermissions();
    this.showSubmitButton = this.selectedRolePermissionsIds.length > 0;
    this.cdr.markForCheck();
  }

  private resetSelection(): void {
    this.items = [];
    this.selectedRolePermissions = [];
    this.selectedRolePermissionsIds = [];
    this.showSubmitButton = false;
    this.cdr.markForCheck();
  }

  private formateTreeResponse(permissions: PermissionResponse[]): TreeviewItem[] {
    return permissions.map(permission => {
      const children = this.buildChildTree(permission.childrens);
      const text = this.translate.currentLang === 'ar-SA' ? permission.nameAr : permission.nameEn;
      const treeItem = new TreeviewItem({ text, value: permission.id, checked: permission.checked, children });
      treeItem.correctChecked();
      return treeItem;
    });
  }

  private buildChildTree(children: PermissionResponse[]): TreeItem[] {
    return children.map(child => ({
      text: this.translate.currentLang === 'ar-SA' ? child.nameAr : child.nameEn,
      value: child.id,
      checked: child.checked,
      children: this.buildChildTree(child.childrens)
    }));
  }

  onSelectedTreeChange(node: any): void {
    this.updateChildrenState(node, node.internalChecked);
    const selectedPermissions: number[] = [];
    this.collectSelectedTreeIds(this.items, selectedPermissions);
    this.showSubmitButton = selectedPermissions.length > 0;

    console.log("new selected Role Permissions Ids", selectedPermissions);

  }

  private updateChildrenState(node: any, isChecked: boolean): void {
    node.internalChecked = isChecked;
    node.internalChildren?.forEach((child: any) => this.updateChildrenState(child, isChecked));
  }

  private updateTreeWithSelectedPermissions(): void {
    this.items.forEach(item => {
      this.setDefaultChecked([item], this.selectedRolePermissionsIds);
    });
    this.cdr.markForCheck(); // Trigger change detection
  }
  
  private collectSelectedTreeIds(items: TreeviewItem[], selectedIds: number[]): void {
    items.forEach(item => {
      if (item.checked || item.indeterminate) {
        selectedIds.push(item.value);
      }
      this.collectSelectedTreeIds(item.children || [], selectedIds);
    });
  }
  private setDefaultChecked(items: TreeviewItem[], selectedIds: number[]): void {
    items.forEach(item => {
      const isChecked = selectedIds.includes(item.value);
      item.checked = isChecked;
  
      // Update children based on their individual inclusion in the selected IDs
      if (item.children) {
        this.setDefaultChecked(item.children, selectedIds);
      }
  
      // Correct the checked state of the parent based on its children's state
      item.correctChecked();
    });
  }
  

  savePermission(): void {
    if (this.currentRoleId <= 0) {
      this.showErrorToast('form.validation.selectRole');
      return;
    }

    const selectedPermissions: number[] = [];
    this.collectSelectedTreeIds(this.items, selectedPermissions);

    if (selectedPermissions.length <= 0) {
      this.showErrorToast('form.validation.selectAtLeastOnePermission');
      return;
    }

    let finalDto:UpdateRolePermissionsRequest = {id:this.currentRoleId, rolePermissionsIds:selectedPermissions}
    this.saveRolesPermissions(finalDto);
    this.log.info('Saving Permissions', { roleId: this.currentRoleId, selectedPermissions });
    this.log.info('Saving finalDto', finalDto);
  }


  private saveRolesPermissions(request:UpdateRolePermissionsRequest): void {
    this.isLoading = true;
    this.roleApiService.UpdateRolePermissions(request).subscribe({
      next: (response) => {
        if (response?.statusCode === 200 && response.data) {
          this.fetchRoles();
          this.showSuccessToast('form.validation.saveSuccess');
          
        } else {
          this.log.error('No roles data received.');
        }
      },
      error: (err) => this.log.error('Error fetching roles:', err),
      complete: () => (this.isLoading = false)
    });
  }


  private showErrorToast(messageKey: string): void {
    this.toastNotificationService.showToast(NotitficationsDefaultValues.Danger, this.translate.instant('usermanagement.roles-management'), this.translate.instant(messageKey));
  }

  private showSuccessToast(messageKey: string): void {
    this.toastNotificationService.showToast(NotitficationsDefaultValues.Success, this.translate.instant('usermanagement.roles-management'), this.translate.instant(messageKey));
  }
}
