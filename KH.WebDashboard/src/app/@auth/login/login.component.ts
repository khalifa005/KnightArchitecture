import { Router } from '@angular/router';
import { Observable, Subject, takeUntil } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { Component, ChangeDetectorRef, Inject, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../services/custom-auth-service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { LanguageEnum } from '../../@i18n/models/language.enum';
import { NbLayoutDirection, NbLayoutDirectionService } from '@nebular/theme';
import { I18nService } from '../../@i18n/services/i18n.service';
import { environment } from '../../../environments/environment';
import { LoginForm } from './login-form';
import { UserApiService } from '@app/@core/api-services/user-api.service';
import { ApiResponse } from '@app/@core/models/base/response/custom-api-response';
import { PermissionResponse } from '@app/@core/models/responses/permission-response';
import { loadPermissions } from '../services/permission-management.service';
import { NgxPermissionsService } from 'ngx-permissions';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';

@Component({
  selector: 'ngx-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class NgxLoginComponent implements OnInit,OnDestroy {

  directions = NbLayoutDirection;
  selectedLanguage = environment.defaultLanguage; // Default language
  showPassword: boolean = false; // Property to track password visibility
  submitted: boolean = false; // Property to track password visibility

  loginForm: LoginForm;
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  constructor(
    private authService: AuthService,
    private translate: TranslateService,
    private router: Router,
    private permissionsService: NgxPermissionsService,
    private toastNotificationService:ToastNotificationService,
    private userApiService: UserApiService,
    private directionService: NbLayoutDirectionService,
    private i18nService: I18nService) {

    this.loginForm = new LoginForm(this.translate);
    this.onLanguageChange(this.i18nService.language);
  }

  ngOnInit(): void {
    // this.tokendata();
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  login() {

    if (this.loginForm.invalid) {
      return;
    }

    // Process login
    this.submitted = true;
    console.log("User is logged in");
    const formData = this.loginForm.value;
    console.log('Form Data:', formData);

    // Output: { username: 'enteredUsername', password: 'enteredPassword' }
    if (this.loginForm.valid) {

      this.authService.login(formData)
      .pipe(takeUntil(this.ngUnsubscribe)) // Unsubscribe automatically on component destroy
       .subscribe({
        next: (response) => {
          console.log('Login Successful:', response);
          // Handle successful login
          this.fetchUserPermissions()
          // this.tokendata();

        },
        error: (error) => {
          console.error('Login Failed:', error);
          // Handle login error
          this.submitted = false;
        },
      });

    }

  }

  fetchUserPermissions() {
    this.userApiService.getUserPermissions().subscribe({
      next: (response: ApiResponse<PermissionResponse[]>) => {
        if (response.data) {
          const allUserPermissions = response.data.map((permission) => permission.key);

          if (allUserPermissions.length > 0) {
            loadPermissions(this.permissionsService, allUserPermissions ?? []);

            this.authService.setUserPermissions(allUserPermissions);
            console.log('user permissions :', allUserPermissions);
            this.router.navigateByUrl('/');
          }
          else {
            console.info('user doesnt have permissions:');
            this.toastNotificationService.showToast(NotitficationsDefaultValues.Warning, 'Warning', "User permissions not found");

            this.submitted = false;
          }
        }
        else {
          console.warn('Error fetching permissions:');

        }
      },
      error: (error) => {
        console.error('Error fetching permissions:', error);
        this.submitted = false;

        // this.loading = false;
      },
    });
  }

  tokendata() {
    var toekn = this.authService.decodeToken();
    console.info('JWT decoded token:', toekn);

    this.authService.getUserObservable().subscribe((user) => {
      const userinfo = user;
      console.log('User data from Observable :', userinfo);
    });

    const hasAccess = this.authService.doesUserHasRquiredRoles(['2', '5']); // Required roles
    console.log('does User Has the Rquired Roles :', hasAccess);

    const userdata = this.authService.getUser();
    console.log('direct userinfo data :', userdata);
  }

  // Method to toggle password visibility
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  getInputType() {
    if (this.showPassword) {
      return 'text';
    }
    return 'password';
  }

  toggleShowPassword() {
    this.showPassword = !this.showPassword;
  }

  onLanguageChange(language: string): void {
    if (language === 'ar-SA') {
      this.selectedLanguage = 'ar-SA';
      this.i18nService.language = LanguageEnum.Ar;
      this.directionService.setDirection(this.directions.RTL);
    } else if (language === 'en-US') {
      this.selectedLanguage = 'en-US';
      this.i18nService.language = LanguageEnum.En;
      this.directionService.setDirection(this.directions.LTR);
    }

    // Optionally persist the selected language in localStorage for consistency
    // localStorage.setItem('selectedLanguage', this.selectedLanguage);
  }

}

