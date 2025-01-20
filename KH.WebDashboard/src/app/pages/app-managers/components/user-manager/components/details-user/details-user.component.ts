import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { Logger } from '@app/@core/utils.ts/logger.service';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { TranslateService } from '@ngx-translate/core';
import { Subject, takeUntil } from 'rxjs';
import { UserDetailsResponse, UserDetailsResponseApiResponse, UsersService } from 'src/open-api';

@Component({
  selector: 'app-details-user',
  templateUrl: './details-user.component.html',
  styleUrl: './details-user.component.scss'
})
export class DetailsUserComponent implements OnInit, OnDestroy {
  private log = new Logger(DetailsUserComponent.name);

  @Input() idInput: number;
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  userDetails: UserDetailsResponse | null = null;

  isLoading: boolean = false;

  constructor(
    private usersService: UsersService,
    private toastNotificationService: ToastNotificationService,
    private translateService: TranslateService
  ) {}

  ngOnInit(): void {
    if (this.idInput) {
      this.fetchUserDetails();

      // this.fetchEmails();
      // this.fetchSMS();
      // this.fetchFiles();
      // this.fetchLogs();

    } else {
      this.log.error('No user ID provided.');
    }
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  private fetchUserDetails(): void {
    this.isLoading = true;
    this.usersService
      .apiV1UsersIdGet(this.idInput)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            this.userDetails = response.data ;
          } else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translateService.instant('user.details-fetch-error'),
              this.translateService.instant('user.details-fetch-error')
            );
            this.log.error('Error fetching user details');
          }
        },
        error: (err) => {
          this.log.error('Error fetching user details:', err);
          this.toastNotificationService.showToast(
            NotitficationsDefaultValues.Danger,
            this.translateService.instant('user.details-fetch-error'),
            this.translateService.instant('user.details-fetch-error')
          );
        },
        complete: () => (this.isLoading = false),
      });
  }



  // Static Data for Emails, SMS, Files, and Logs
  emails = [
    {
      subject: 'Welcome to the platform',
      sentDate: '2025-01-10',
      recipient: 'user@example.com',
      content: 'Thank you for joining our platform. We are excited to have you.',
    },
    {
      subject: 'Password Reset',
      sentDate: '2025-01-15',
      recipient: 'user@example.com',
      content: 'Your password reset request has been processed successfully.',
    },
  ];

  smsMessages = [
    {
      recipient: '+966123456789',
      sentDate: '2025-01-05',
      content: 'Your verification code is 123456.',
    },
    {
      recipient: '+966987654321',
      sentDate: '2025-01-12',
      content: 'Your subscription has been renewed successfully.',
    },
  ];

  files = [
    {
      name: 'ProfilePicture.jpg',
      downloadUrl: '/assets/mock/ProfilePicture.jpg',
    },
    {
      name: 'TermsAndConditions.pdf',
      downloadUrl: '/assets/mock/TermsAndConditions.pdf',
    },
  ];

  logs = [
    { timestamp: '2025-01-01 10:00:00', message: 'User logged in.' },
    { timestamp: '2025-01-05 14:20:00', message: 'User updated their profile.' },
    { timestamp: '2025-01-15 16:30:00', message: 'User requested password reset.' },
  ];

  sendEmail(): void {
    // Logic for sending email (currently just a console log)
    console.log('Send Email button clicked.');
  }

  sendSMS(): void {
    // Logic for sending SMS (currently just a console log)
    console.log('Send SMS button clicked.');
  }
  // emails: Array<{ subject: string; sentDate: string; recipient: string; content: string }> = [];
  // smsMessages: Array<{ recipient: string; sentDate: string; content: string }> = [];
  // files: Array<{ name: string; downloadUrl: string }> = [];
  // logs: Array<{ timestamp: string; message: string }> = [];

  // private fetchEmails(): void {
  //   this.isLoading = true;
  //   this.usersService
  //     .apiV1UsersEmailsGet(this.idInput)
  //     .pipe(takeUntil(this.ngUnsubscribe))
  //     .subscribe({
  //       next: (response) => {
  //         this.emails = response.data || [];
  //       },
  //       error: (err) => {
  //         this.log.error('Error fetching emails:', err);
  //       },
  //       complete: () => (this.isLoading = false),
  //     });
  // }

  // private fetchSMS(): void {
  //   this.isLoading = true;
  //   this.usersService
  //     .apiV1UsersSmsGet(this.idInput)
  //     .pipe(takeUntil(this.ngUnsubscribe))
  //     .subscribe({
  //       next: (response) => {
  //         this.smsMessages = response.data || [];
  //       },
  //       error: (err) => {
  //         this.log.error('Error fetching SMS messages:', err);
  //       },
  //       complete: () => (this.isLoading = false),
  //     });
  // }

  // private fetchFiles(): void {
  //   this.isLoading = true;
  //   this.usersService
  //     .apiV1UsersFilesGet(this.idInput)
  //     .pipe(takeUntil(this.ngUnsubscribe))
  //     .subscribe({
  //       next: (response) => {
  //         this.files = response.data || [];
  //       },
  //       error: (err) => {
  //         this.log.error('Error fetching files:', err);
  //       },
  //       complete: () => (this.isLoading = false),
  //     });
  // }

  // private fetchLogs(): void {
  //   this.isLoading = true;
  //   this.usersService
  //     .apiV1UsersLogsGet(this.idInput)
  //     .pipe(takeUntil(this.ngUnsubscribe))
  //     .subscribe({
  //       next: (response) => {
  //         this.logs = response.data || [];
  //       },
  //       error: (err) => {
  //         this.log.error('Error fetching logs:', err);
  //       },
  //       complete: () => (this.isLoading = false),
  //     });
  // }
}
