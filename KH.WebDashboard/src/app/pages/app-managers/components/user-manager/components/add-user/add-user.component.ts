import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { Logger } from '@app/@core/utils.ts/logger.service';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { NbWindowRef } from '@nebular/theme';
import { TranslateService } from '@ngx-translate/core';
import { Subject, takeUntil } from 'rxjs';
import { CreateUserRequest, UsersService } from 'src/open-api';
import { UserForm } from './user.form';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrl: './add-user.component.scss'
})
export class AddUserComponent implements OnInit, OnDestroy {
  private log = new Logger(AddUserComponent.name);

  @Input() userIdInput: number;
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  myForm: UserForm;
  model: CreateUserRequest = {};
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private usersService: UsersService,
    public translationService: TranslateService,
    private toastNotificationService: ToastNotificationService,
    public windowRef: NbWindowRef<AddUserComponent>
  ) {
    // Initialize the form with the model
    this.myForm = new UserForm(this.fb, this.model);
  }

  ngOnInit(): void {
    if (this.userIdInput) {
      this.fetchUser();
    }
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  onSubmit(): void {
    if (this.myForm.valid) {
      const formDataModel = this.myForm.value as CreateUserRequest;

      if (this.userIdInput) {
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
      .apiV1UsersIdGet(this.userIdInput)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            const userEntity = response.data;
            this.model = { ...userEntity };
            this.myForm = new UserForm(this.fb, this.model);
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
}