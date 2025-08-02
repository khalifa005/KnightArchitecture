import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { _HttpClient, ALAIN_I18N_TOKEN, SettingsService } from '@delon/theme';
import { ACLService } from '@delon/acl';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Subject, takeUntil } from 'rxjs';
import { SHARED_IMPORTS } from '@shared';

@Component({
   selector: 'app-user-permissions',
   imports: [SHARED_IMPORTS],
   templateUrl: './user-permissions.component.html',
   styleUrl: './user-permissions.component.less'
})
export class UserPermissionsComponent implements OnInit, OnDestroy {
   private ngUnsubscribe: Subject<void> = new Subject<void>();

   private readonly http = inject(_HttpClient);
   private readonly msg = inject(NzMessageService);
   private readonly i18nSrv = inject(ALAIN_I18N_TOKEN);
   private readonly aclService = inject(ACLService);
   private readonly settingsService = inject(SettingsService);

   ngOnInit(): void {
      this.logUserPermissions();
   }

   ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
   }

   // Debug method to log user permissions and role
   private logUserPermissions(): void {
      console.log('Current user:', this.settingsService.user);
      console.log('User role:', this.settingsService.user.role);
      console.log('User permissions:', this.settingsService.user.permissions);
      console.log('Can USER_MANAGEMENT:', this.aclService.can('USER_MANAGEMENT'));
      console.log('Can ADMIN_DASHBOARD:', this.aclService.can('ADMIN_DASHBOARD'));
      console.log('Can MANAGER_DASHBOARD:', this.aclService.can('MANAGER_DASHBOARD'));
      console.log('Can USER_DASHBOARD:', this.aclService.can('USER_DASHBOARD'));
      console.log('Can REPORTS_VIEW:', this.aclService.can('REPORTS_VIEW'));
      console.log('Can SYSTEM_SETTINGS:', this.aclService.can('SYSTEM_SETTINGS'));
      console.log('Can DATA_EXPORT:', this.aclService.can('DATA_EXPORT'));
      console.log('Can AUDIT_LOGS:', this.aclService.can('AUDIT_LOGS'));
      console.log('Can BACKUP_RESTORE:', this.aclService.can('BACKUP_RESTORE'));
      console.log('Can SNAPSHOTS_VIEW:', this.aclService.can('SNAPSHOTS_VIEW'));
      console.log('Role ID:', this.getCurrentUserRoleId());
      console.log('Can show admin title:', this.canShowAdminTitle());
      console.log('Can show manager title:', this.canShowManagerTitle());
      console.log('Can show user title:', this.canShowUserTitle());
   }

   // ACL Permission checking methods - Using only ACL permissions
   canShowAdminTitle(): boolean {
      return this.aclService.can('ADMIN_DASHBOARD') && this.aclService.can('USER_MANAGEMENT');
   }

   canShowManagerTitle(): boolean {
      return this.aclService.can('MANAGER_DASHBOARD') && this.aclService.can('USER_MANAGEMENT');
   }

   canShowUserTitle(): boolean {
      return this.aclService.can('USER_DASHBOARD') && this.aclService.can('USER_MANAGEMENT');
   }

   // Additional permission-based title methods
   canShowReportsTitle(): boolean {
      return this.aclService.can('REPORTS_VIEW');
   }

   canShowSystemSettingsTitle(): boolean {
      return this.aclService.can('SYSTEM_SETTINGS');
   }

   canShowDataExportTitle(): boolean {
      return this.aclService.can('DATA_EXPORT');
   }

   canShowAuditLogsTitle(): boolean {
      return this.aclService.can('AUDIT_LOGS');
   }

   canShowBackupRestoreTitle(): boolean {
      return this.aclService.can('BACKUP_RESTORE');
   }

   canShowSnapshotsTitle(): boolean {
      return this.aclService.can('SNAPSHOTS_VIEW');
   }

   // Get current user role ID (assuming role is stored as string, convert to number if needed)
   getCurrentUserRoleId(): number {
      const role = this.settingsService.user.role;
      const roleIdMap: { [key: string]: number } = {
         'super-admin': 1,
         'admin': 2,
         'manager': 3,
         'user': 4,
         'guest': 5
      };
      return roleIdMap[role] || 0;
   }

   // Check if user has specific role ID
   hasRoleId(roleId: number): boolean {
      return this.getCurrentUserRoleId() === roleId;
   }

   // Make ACL service accessible in template
   get acl() {
      return this.aclService;
   }

   // Make settings service accessible in template
   get settings() {
      return this.settingsService;
   }
} 