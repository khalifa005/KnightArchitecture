import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { Logger } from '@app/@core/utils.ts/logger.service';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { TranslateService } from '@ngx-translate/core';
import { Subject, takeUntil } from 'rxjs';
import { DepartmentResponse, DepartmentsService } from 'src/open-api';

@Component({
  selector: 'app-details-department',
  templateUrl: './details-department.component.html',
  styleUrl: './details-department.component.scss'
})
export class DetailsDepartmentComponent implements OnInit, OnDestroy {
  private log = new Logger(DetailsDepartmentComponent.name);

  @Input() idInput: number;
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  departmentDetails: DepartmentResponse = {} as DepartmentResponse;
  isLoading: boolean = false;

  constructor(
    private apiService: DepartmentsService,
    public translationService: TranslateService,
    private toastNotificationService: ToastNotificationService
  ) {}

  ngOnInit(): void {
    if (this.idInput) {
      this.fetchDepartment();
    }
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  private fetchDepartment(): void {
    this.isLoading = true;
    this.apiService.apiV1DepartmentsIdGet(this.idInput)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            this.departmentDetails = response.data;

            console.log('Department data received:', this.departmentDetails);
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
}