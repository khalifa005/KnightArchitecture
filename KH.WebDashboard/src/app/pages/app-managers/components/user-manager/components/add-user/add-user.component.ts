import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { Logger } from '@app/@core/utils.ts/logger.service';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { NbWindowRef } from '@nebular/theme';
import { TranslateService } from '@ngx-translate/core';
import { Subject, takeUntil } from 'rxjs';
import { CreateUserRequest, DepartmentFilterRequest, DepartmentListResponse, DepartmentsService, RoleFilterRequest, RoleListResponse, RolesService, UserDetailsResponse, UsersService } from 'src/open-api';
import { UserForm } from './user.form';
import { LookupResponse } from '@app/@core/models/base/response/lookup.model';
import { transformToLookup } from '@app/@core/utils.ts/lookup-mapper';
import { date } from '@app/@core/utils.ts/form/validations/form.validation-helpers';
import { formatDateFns } from '@app/@core/utils.ts/date/date-utils';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrl: './add-user.component.scss'
})
export class AddUserComponent implements OnInit, OnDestroy {
  private log = new Logger(AddUserComponent.name);

  @Input() idInput: number;
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  myForm: UserForm;
  modelx: CreateUserRequest = {
    firstName: "",
    middleName: "",
    lastName: "",
    password: "",
    email: "",
    mobileNumber: "",
    username: "",
    sensitiveData: "",
    birthDate: "",
    // groupId: ,
    // departmentId: 5,
    roleIds: [],
  };
  model: CreateUserRequest = {
    firstName: "Mahmoud",
    middleName: "Mohamed",
    lastName: "Khalifa",
    password: "KhalifaPassword",
    email: "khalifa_CEO1@example.com",
    mobileNumber: "05100000010",
    username: "khalifa_CEO1",
    sensitiveData: "AccountNumberExample",
    // birthDate: formatDateFns('2023-01-20T10:30:00.000Z', 'dd-MM-yyyy'),
    birthDate: new Date('2024-12-31').toDateString(),
    groupId: 2,
    departmentId: 5,
    roleIds: [3],
  };

  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private usersService: UsersService,
    public translationService: TranslateService,
    private toastNotificationService: ToastNotificationService,
    private departmentApiService: DepartmentsService,
    private roleApiService: RolesService,

    public windowRef: NbWindowRef<AddUserComponent>
  ) {

  }

  ngOnInit(): void {

    this.fetchDepartmentData();
    this.fetchRolesData();
    if (this.idInput) {
      this.fetchUser();
    } else {
      // Initialize the form with the model
      this.myForm = new UserForm(this.fb, this.model);
    }
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  onSubmit(): void {
    if (this.myForm.valid) {
      const formDataModel = this.myForm.value as CreateUserRequest;

      if (this.idInput) {
        this.editUser(formDataModel);
      } else {
        this.addNewUser(formDataModel);
      }
    } else {
      console.log('Form is invalid. Please correct the errors.');
    }
  }

  addNewUser(formModel: CreateUserRequest): void {
    this.isLoading = true;
    this.usersService
      .apiV1UsersPost(formModel)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Success,
              this.translationService.instant('user.added'),
              this.translationService.instant('user.added')
            );
            this.cancel(response?.statusCode);
          } else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('user.add-error'),
              this.translationService.instant('user.add-error')
            );
            this.log.error('Error adding user.');
          }
        },
        error: (err) => this.log.error('Error adding user:', err),
        complete: () => (this.isLoading = false),
      });
  }

  private fetchUser(): void {
    this.isLoading = true;
    this.usersService
      .apiV1UsersIdGet(this.idInput)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            const userEntity = response.data;
            const departmentsIds = userEntity.userDepartments.map(x => x.departmentId);
            this.model = { ...userEntity };
            this.model.departmentId = departmentsIds.length > 0 ? departmentsIds[0] : null;
            this.model.birthDate = new Date(userEntity.birthDate).toDateString();
            this.myForm = new UserForm(this.fb, this.model, true);
            // this.myForm = new UserForm(new FormBuilder(), this.model, true);

          } else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('user.fetch-error'),
              this.translationService.instant('user.fetch-error')
            );
            this.log.error('No user data received.');
          }
        },
        error: (err) => this.log.error('Error fetching user:', err),
        complete: () => (this.isLoading = false),
      });
  }

  private editUser(formModel: CreateUserRequest): void {
    this.isLoading = true;
    this.usersService
      .apiV1UsersPut(formModel)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Success,
              this.translationService.instant('user.updated'),
              this.translationService.instant('user.updated')
            );
            this.cancel(response?.statusCode);
          } else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('user.update-error'),
              this.translationService.instant('user.update-error')
            );
            this.log.error('Error updating user.');
          }
        },
        error: (err) => this.log.error('Error updating user:', err),
        complete: () => (this.isLoading = false),
      });
  }

  public cancel(statusCode: number) {
    this.windowRef.close(statusCode);
  }

  departmentsResponse: DepartmentListResponse[] = [];
  departments: LookupResponse[] = [];

  fetchDepartmentData(): void {
    const filterRequest: DepartmentFilterRequest = {
      isDeleted: false,
      pageIndex: 1,
      pageSize: 500,
      search: "",
    };
    this.departmentApiService
      .apiV1DepartmentsPagedListPost(filterRequest) // Pass the filter object
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            this.departmentsResponse = (response.data.items ?? []);
            this.departments = transformToLookup(this.departmentsResponse);
          }
          else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('role.list'),
              this.translationService.instant('role.lis-erorr'));
            this.log.error('No roles data received.');
          }
        },
        error: (error) => {
          console.error('Error fetching paged department list:', error);
        }
      });
  }

  rolesResponse: RoleListResponse[] = [];
  roles: LookupResponse[] = [];

  fetchRolesData(): void {
    const filterRequest: RoleFilterRequest = {
      isDeleted: false,
      pageIndex: 1,
      pageSize: 500,
      search: "",
    };
    this.roleApiService
      .apiV1RolesPagedListPost(filterRequest) // Pass the filter object
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            this.rolesResponse = (response.data.items ?? []);
            this.roles = transformToLookup(this.rolesResponse);
          }
          else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('role.list'),
              this.translationService.instant('role.lis-erorr'));
            this.log.error('No roles data received.');
          }
        },
        error: (error) => {
          console.error('Error fetching paged department list:', error);
        }
      });
  }

}