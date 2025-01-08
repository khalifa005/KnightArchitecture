import { Component, Input, input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { RoleForm } from './role.form';
import { CraeteLookupRequest } from '@app/@core/models/base/request/create-lookup.request';
import { NbWindowRef } from '@nebular/theme';
import { RoleApiService } from '@app/@core/api-services/role-api.service';
import { Subject, takeUntil } from 'rxjs';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { TranslateService } from '@ngx-translate/core';
import { Logger } from '@app/@core/utils.ts/logger.service';

@Component({
  selector: 'app-add-role',
  templateUrl: './add-role.component.html',
  styleUrl: './add-role.component.scss'
})

export class AddRoleComponent implements OnInit, OnDestroy {
  private log = new Logger(AddRoleComponent.name);

  @Input() roleIdInput: number;
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  myForm: RoleForm;
  model: CraeteLookupRequest = new CraeteLookupRequest();
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private apiService: RoleApiService,
    public translationService: TranslateService,
    private toastNotificationService: ToastNotificationService,
    public windowRef: NbWindowRef<AddRoleComponent>,

  ) {
    // Initialize the form with the model
    this.myForm = new RoleForm(this.fb, this.model);
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


  onSubmit(): void {
    if (this.myForm.valid) {
      console.log('Form Submitted!', this.myForm.value);
      const formDataModel = this.myForm.value as CraeteLookupRequest;

      if (this.roleIdInput) {
        this.editRole(formDataModel);
      } else {
        this.addNew(formDataModel);
      }
    } else {
      console.log('Form is invalid. Please correct the errors.');
    }
  }


  addNew(formModel: any): void {

    this.isLoading = true;
    this.apiService.addRole(formModel)
      .pipe(takeUntil(this.ngUnsubscribe)) // Unsubscribe automatically on component destroy
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {

            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Success,
              this.translationService.instant('role.added'),
              this.translationService.instant('role.added'));

            this.cancel(response?.statusCode)
          }
          else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('role.add'),
              this.translationService.instant('role.add-erorr'));
            this.log.error('add new .');
          }
        },
        error: (err) => this.log.error('Error add role:', err),
        complete: () => (this.isLoading = false)
      });
  }


  public cancel(statusCode: number) {
    // this.ref.close(statusCode);
    this.windowRef.close(statusCode);
  }


  private fetchRole(): void {
    this.isLoading = true;
    this.apiService.getRole(this.roleIdInput)
      .pipe(takeUntil(this.ngUnsubscribe)) // Unsubscribe automatically on component destroy
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            const roleEntity = response.data;
            this.model.id = roleEntity.id
            this.model.nameAr = roleEntity.nameAr
            this.model.nameEn = roleEntity.nameEn
            this.model.description = roleEntity.description

            this.myForm = new RoleForm(this.fb, this.model);

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


  private editRole(formModel: any): void {
    this.isLoading = true;
    this.apiService.updateRole(formModel)
      .pipe(takeUntil(this.ngUnsubscribe)) // Unsubscribe automatically on component destroy
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Success,
              this.translationService.instant('role.updated'),
              this.translationService.instant('role.updated'));

              this.cancel(response?.statusCode)
          }
          else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('role.updated'),
              this.translationService.instant('role.updated-erorr'));
            this.log.error('No roles data updated.');
          }
        },
        error: (err) => this.log.error('Error update role:', err),
        complete: () => (this.isLoading = false)
      });
  }


}