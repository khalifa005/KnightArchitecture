import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { RoleApiService } from '@app/@core/api-services/role-api.service';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { CraeteLookupRequest } from '@app/@core/models/base/request/create-lookup.request';
import { Logger } from '@app/@core/utils.ts/logger.service';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { NbWindowRef } from '@nebular/theme';
import { TranslateService } from '@ngx-translate/core';
import { Subject, takeUntil } from 'rxjs';
import { AddRoleComponent } from '../../../roles-manager/components/add-role/add-role.component';
import { RoleForm } from '../../../roles-manager/components/add-role/role.form';
import { DepartmentForm } from './department.form';
import { CreateDepartmentRequest, DepartmentsService } from 'src/open-api';

@Component({
  selector: 'app-add-department',
  templateUrl: './add-department.component.html',
  styleUrl: './add-department.component.scss'
})
export class AddDepartmentComponent implements OnInit, OnDestroy {
  private log = new Logger(AddDepartmentComponent.name);

  @Input() idInput: number;
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  myForm: DepartmentForm;
  model: CraeteLookupRequest = new CraeteLookupRequest();
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    public translationService: TranslateService,
    private apiService: DepartmentsService,
    private toastNotificationService: ToastNotificationService,
    public windowRef: NbWindowRef<AddRoleComponent>,
  ) {
    // Initialize the form with the model
    this.myForm = new RoleForm(this.fb, this.model);
  }

  ngOnInit(): void {
    if (this.idInput) {
      this.fetchDepartment()
    }
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  onSubmit(): void {
    if (this.myForm.valid) {
      console.log('Form Submitted!', this.myForm.value);
      const formDataModel = this.myForm.value as CreateDepartmentRequest;

      if (this.idInput) {
        this.editDepartment(formDataModel);
      } else {
        this.addNew(formDataModel);
      }
    } else {
      console.log('Form is invalid. Please correct the errors.');
    }
  }

  addNew(formModel: CreateDepartmentRequest): void {
    this.isLoading = true;
    this.apiService.apiV1DepartmentsPost(formModel)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Success,
              this.translationService.instant('department.added'),
              this.translationService.instant('department.added-success')
            );
            this.cancel(response?.statusCode);
          } else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('department.add'),
              this.translationService.instant('department.add-error')
            );
            this.log.error('Add new department failed.');
          }
        },
        error: (err) => this.log.error('Error adding department:', err),
        complete: () => (this.isLoading = false)
      });
  }

  fetchDepartment(): void {
    this.isLoading = true;
    this.apiService.apiV1DepartmentsIdGet(this.idInput)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            const departmentEntity = response.data;
            this.model.id = departmentEntity.id;
            this.model.nameAr = departmentEntity.nameAr;
            this.model.nameEn = departmentEntity.nameEn;
            this.model.description = departmentEntity.description;

            this.myForm = new DepartmentForm(this.fb, this.model);
          } else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('department.get'),
              this.translationService.instant('department.get-error')
            );
            this.log.error('No department data received.');
          }
        },
        error: (err) => this.log.error('Error fetching department:', err),
        complete: () => (this.isLoading = false)
      });
  }

  editDepartment(formModel: CreateDepartmentRequest): void {
    this.isLoading = true;
    this.apiService.apiV1DepartmentsPut(formModel)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Success,
              this.translationService.instant('department.updated'),
              this.translationService.instant('department.updated-success')
            );
            this.cancel(response?.statusCode);
          } else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('department.update'),
              this.translationService.instant('department.update-error')
            );
            this.log.error('No department data updated.');
          }
        },
        error: (err) => this.log.error('Error updating department:', err),
        complete: () => (this.isLoading = false)
      });
  }

  public cancel(statusCode: number) {
    // this.ref.close(statusCode);
    this.windowRef.close(statusCode);
  }

}