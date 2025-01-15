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

  @Input() userId: number;
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  userDetails: UserDetailsResponse | null = null;
  isLoading: boolean = false;

  constructor(
    private usersService: UsersService,
    private toastNotificationService: ToastNotificationService,
    private translateService: TranslateService
  ) {}

  ngOnInit(): void {
    if (this.userId) {
      this.fetchUserDetails();
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
      .apiV1UsersIdGet(this.userId)
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
}
