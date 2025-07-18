import { HttpContext, HttpStatusCode } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { StartupService } from '@core';
import { ReuseTabService } from '@delon/abc/reuse-tab';
import { ACLService } from '@delon/acl';
import { ALLOW_ANONYMOUS, DA_SERVICE_TOKEN, ITokenModel, SocialOpenType, SocialService } from '@delon/auth';
import { I18nPipe, SettingsService, _HttpClient } from '@delon/theme';
import { environment } from '@env/environment';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzTabChangeEvent, NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { finalize, Subject, takeUntil } from 'rxjs';
import { AuthenticationService, ApiV1AuthenticationLoginPostRequestParams, LoginRequest } from 'src/app/shared/open-api';

@Component({
  selector: 'passport-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.less'],
  providers: [SocialService],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    RouterLink,
    ReactiveFormsModule,
    I18nPipe,
    NzCheckboxModule,
    NzTabsModule,
    NzAlertModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzToolTipModule,
    NzIconModule
  ]
})
export class UserLoginComponent implements OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  private aclService = inject(ACLService);

  private readonly authenticationService = inject(AuthenticationService);
  private readonly router = inject(Router);
  private readonly settingsService = inject(SettingsService);
  private readonly socialService = inject(SocialService);
  private readonly reuseTabService = inject(ReuseTabService, { optional: true });
  private readonly tokenService = inject(DA_SERVICE_TOKEN);
  private readonly startupSrv = inject(StartupService);
  private readonly http = inject(_HttpClient);
  private readonly cdr = inject(ChangeDetectorRef);

  form = inject(FormBuilder).nonNullable.group({
    userName: ['khalifa_CEO', [Validators.required]],
    password: ['KhalifaPassword', [Validators.required]],
    // userName: ['', [Validators.required, Validators.pattern(/^(admin|user)$/)]],
    // password: ['', [Validators.required, Validators.pattern(/^(ng-alain\.com)$/)]],
    mobile: ['', [Validators.required, Validators.pattern(/^1\d{10}$/)]],
    captcha: ['', [Validators.required]],
    remember: [true]
  });
  error = '';
  type = 0;
  loading = false;

  count = 0;
  interval$: any;

  switch({ index }: NzTabChangeEvent): void {
    this.type = index!;
  }

  getCaptcha(): void {
    const mobile = this.form.controls.mobile;
    if (mobile.invalid) {
      mobile.markAsDirty({ onlySelf: true });
      mobile.updateValueAndValidity({ onlySelf: true });
      return;
    }
    this.count = 59;
    this.interval$ = setInterval(() => {
      this.count -= 1;
      if (this.count <= 0) {
        clearInterval(this.interval$);
      }
    }, 1000);
  }

  submit(): void {
    this.error = '';
    if (this.type === 0) {
      const { userName, password } = this.form.controls;
      userName.markAsDirty();
      userName.updateValueAndValidity();
      password.markAsDirty();
      password.updateValueAndValidity();
      if (userName.invalid || password.invalid) {
        return;
      }
    } else {
      const { mobile, captcha } = this.form.controls;
      mobile.markAsDirty();
      mobile.updateValueAndValidity();
      captcha.markAsDirty();
      captcha.updateValueAndValidity();
      if (mobile.invalid || captcha.invalid) {
        return;
      }
    }

    this.loading = true;
    this.cdr.detectChanges();

    const fullApiUrl = `${environment.api.serverUrl}/api/v1/Authentication/Login`;

    // Prepare payload
    const loginRequest: LoginRequest = {
      username: this.form.value.userName,
      password: this.form.value.password
    };

    this.authenticationService
      .apiV1AuthenticationLoginPost(
        { loginRequest },
        'body',
        false,
        {
          context: new HttpContext().set(ALLOW_ANONYMOUS, true),
          httpHeaderAccept: 'application/json'
        }
      )
      .pipe(
        takeUntil(this.ngUnsubscribe),
        finalize(() => {
          this.loading = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe({
        next: res => {
          if (res?.statusCode !== HttpStatusCode.Ok) {
            this.error = res?.errorMessage ?? 'Login failed';
            this.cdr.detectChanges();
            return;
          }
          this.reuseTabService?.clear();

          const response = res.data;
          const tokenModel: ITokenModel = {
            token: response?.accessToken,
            expired: +new Date() + 1000 * 60 * 5
          }

          this.tokenService.set(tokenModel);

          // ACLSet admin permissions
          // this.aclService.setFull(true);
          // Set actual permissions
          // this.aclService.setAbility(realUser.permissions);

          //  <div *aclIf="'SNAPSHOTS_MANAGE'">
          //   <button (click)="editSnapshot()">Edit</button>
          // </div>

          // Store ALL user properties from API response
          this.settingsService.setUser({
            // ...res.data.user, // Include all properties from API
            name: "khalifa", // Map as needed
            avatar: "./assets/tmp/img/avatar.jpg",
            email: "khalifa@gmail.com",
            // Add any custom properties
            department: "res.data.user.department",
            role: "super-admin",
            permissions: ["SNAPSHOTS_VIEW", "USER_MANAGEMENT",10, 'USER-EDIT']
          });

          // Now you get type hints when using
          const userDepartment = this.settingsService.user.department;

          // Set ACL permissions
          // this.aclService.setAbility(res.data.user.aclPermissions);
          // this.aclService.setAbility(["SNAPSHOTS_VIEW", "USER_MANAGEMENT"]);
          this.aclService.setAbility(this.settingsService.user.permissions);
          console.log('User permissions:', this.settingsService.user.permissions);
          console.log('ACL abilities:', this.aclService['abilities']);

          // Filter menu based on permissions
          // inject(StartupService).filterMenuByPermissions();


          this.startupSrv.load().subscribe(() => {
            let url = this.tokenService.referrer!.url || '/';
            if (url.includes('/passport')) {
              url = '/';
            }
            this.router.navigateByUrl(url);
          });

          // this.staticLogin();

        },
        error: () => {
          this.error = 'Login failed due to server error';
          this.cdr.detectChanges();
        }
      });


  }

  staticLogin() {

    this.http
      .post(
        '/login/account',
        {
          type: this.type,
          userName: this.form.value.userName,
          password: this.form.value.password
        },
        null,
        {
          context: new HttpContext().set(ALLOW_ANONYMOUS, true)
        }
      )
      .pipe(
        finalize(() => {
          this.loading = false;
          this.cdr.detectChanges();
        })
      )
      .subscribe(res => {
        if (res.msg !== 'ok') {
          this.error = res.msg;
          this.cdr.detectChanges();
          return;
        }
        // 清空路由复用信息
        this.reuseTabService?.clear();
        // 设置用户Token信息
        // TODO: Mock expired value
        res.user.expired = +new Date() + 1000 * 60 * 5;
        this.tokenService.set(res.user);
        // 重新获取 StartupService 内容，我们始终认为应用信息一般都会受当前用户授权范围而影响
        this.startupSrv.load().subscribe(() => {
          let url = this.tokenService.referrer!.url || '/';
          if (url.includes('/passport')) {
            url = '/';
          }
          this.router.navigateByUrl(url);
        });
      });
  }
  open(type: string, openType: SocialOpenType = 'href'): void {
    let url = ``;
    let callback = ``;
    if (environment.production) {
      callback = `https://ng-alain.github.io/ng-alain/#/passport/callback/${type}`;
    } else {
      callback = `http://localhost:4200/#/passport/callback/${type}`;
    }
    switch (type) {
      case 'auth0':
        url = `//cipchk.auth0.com/login?client=8gcNydIDzGBYxzqV0Vm1CX_RXH-wsWo5&redirect_uri=${decodeURIComponent(callback)}`;
        break;
      case 'github':
        url = `//github.com/login/oauth/authorize?client_id=9d6baae4b04a23fcafa2&response_type=code&redirect_uri=${decodeURIComponent(
          callback
        )}`;
        break;
      case 'weibo':
        url = `https://api.weibo.com/oauth2/authorize?client_id=1239507802&response_type=code&redirect_uri=${decodeURIComponent(callback)}`;
        break;
    }
    if (openType === 'window') {
      this.socialService
        .login(url, '/', {
          type: 'window'
        })
        .subscribe(res => {
          if (res) {
            this.settingsService.setUser(res);
            this.router.navigateByUrl('/');
          }
        });
    } else {
      this.socialService.login(url, '/', {
        type: 'href'
      });
    }
  }

  ngOnDestroy(): void {

    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();

    if (this.interval$) {
      clearInterval(this.interval$);
    }
  }
}
