import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ALAIN_I18N_TOKEN } from '@delon/theme';
import { Subject, takeUntil } from 'rxjs';
import { SHARED_IMPORTS } from '@shared';
import { UserService, User } from '../user.service';

@Component({
   selector: 'app-user-details',
   imports: [SHARED_IMPORTS],
   templateUrl: './user-details.component.html',
   styleUrl: './user-details.component.less'
})
export class UserDetailsComponent implements OnInit, OnDestroy {
   private ngUnsubscribe: Subject<void> = new Subject<void>();

   private readonly modalRef = inject(NzModalRef);
   private readonly msg = inject(NzMessageService);
   private readonly i18nSrv = inject(ALAIN_I18N_TOKEN);
   private readonly userService = inject(UserService);

   // Input parameters
   userId?: number;
   user?: User;
   loading = false;

   ngOnInit(): void {
      // Get modal data from parent component
      const modalData = this.modalRef.getConfig().nzData;
      console.log('User Details Modal data received:', modalData);

      if (modalData && modalData.userId) {
         this.userId = modalData.userId;
         this.loadUserDetails();
      } else {
         this.msg.error('No user ID provided');
         this.modalRef.close();
      }
   }

   ngOnDestroy(): void {
      this.ngUnsubscribe.next();
      this.ngUnsubscribe.complete();
   }

   private loadUserDetails(): void {
      if (this.userId) {
         this.loading = true;
         this.userService.getUserById(this.userId).subscribe({
            next: (user: User | null) => {
               if (user) {
                  this.user = user;
                  this.loading = false;
               } else {
                  this.msg.error('User not found');
                  this.loading = false;
                  this.modalRef.close();
               }
            },
            error: (error: any) => {
               console.error('Error loading user details:', error);
               this.msg.error('Failed to load user details');
               this.loading = false;
               this.modalRef.close();
            }
         });
      }
   }

   getRoleColor(role: string): string {
      const roleColors: { [key: string]: string } = {
         'Doctor': 'blue',
         'Nurse': 'green',
         'Receptionist': 'orange',
         'Pharmacist': 'purple',
         'Lab Technician': 'cyan',
         'Radiologist': 'magenta',
         'Administrator': 'red',
         'Accountant': 'gold',
         'Security Guard': 'lime',
         'Cleaner': 'default'
      };
      return roleColors[role] || 'default';
   }

   close(): void {
      this.modalRef.close();
   }

   editUser(): void {
      // Close current modal and open edit modal
      this.modalRef.close({ action: 'edit', userId: this.userId });
   }

   activateUser(): void {
      if (this.userId) {
         this.userService.updateUserStatus(this.userId, true).subscribe({
            next: (success) => {
               if (success) {
                  this.msg.success('User activated successfully');
                  this.loadUserDetails(); // Reload user data
               } else {
                  this.msg.error('Failed to activate user');
               }
            },
            error: (error) => {
               console.error('Error activating user:', error);
               this.msg.error('Failed to activate user');
            }
         });
      }
   }

   deactivateUser(): void {
      if (this.userId) {
         this.userService.updateUserStatus(this.userId, false).subscribe({
            next: (success) => {
               if (success) {
                  this.msg.success('User deactivated successfully');
                  this.loadUserDetails(); // Reload user data
               } else {
                  this.msg.error('Failed to deactivate user');
               }
            },
            error: (error) => {
               console.error('Error deactivating user:', error);
               this.msg.error('Failed to deactivate user');
            }
         });
      }
   }
} 