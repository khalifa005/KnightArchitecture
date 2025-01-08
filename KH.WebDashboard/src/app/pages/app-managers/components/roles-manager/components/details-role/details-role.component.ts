import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { RoleApiService } from '@app/@core/api-services/role-api.service';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { RolesResponse } from '@app/@core/models/responses/roles-response';
import { Logger } from '@app/@core/utils.ts/logger.service';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { TranslateService } from '@ngx-translate/core';
import { Subject, takeUntil } from 'rxjs';
import { RoleForm } from '../add-role/role.form';

@Component({
  selector: 'app-details-role',
  templateUrl: './details-role.component.html',
  styleUrl: './details-role.component.scss'
})
export class DetailsRoleComponent  implements OnInit, OnDestroy {
  private log = new Logger(DetailsRoleComponent.name);

  @Input() roleIdInput: number;
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  roleDetails: RolesResponse = new RolesResponse();
  isLoading: boolean = false;

  constructor(
    private apiService: RoleApiService,
    public translationService: TranslateService,
    private toastNotificationService: ToastNotificationService,
  ) {
  }

  ngOnInit(): void {
    if (this.roleIdInput) {
      this.fetchRole()
    }
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }



  private fetchRole(): void {
    this.isLoading = true;
    this.apiService.getRole(this.roleIdInput)
      .pipe(takeUntil(this.ngUnsubscribe)) // Unsubscribe automatically on component destroy
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            this.roleDetails = response.data;
            
            console.log('Role data received:', this.roleDetails);
          }
          else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('role.get'),
              this.translationService.instant('role.get-erorr'));
            this.log.error('No roles data received.');
          }
        },
        error: (err) => this.log.error('Error fetching role:', err),
        complete: () => (this.isLoading = false)
      });
  }

  findParentPermission(parentId: number | null) {
    return this.roleDetails.permissions.find((perm: any) => perm.id === parentId);
  }
  
}