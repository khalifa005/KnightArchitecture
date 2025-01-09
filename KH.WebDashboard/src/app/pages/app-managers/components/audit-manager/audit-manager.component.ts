import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { UsersService, AuditsService, UserFilterRequest, AuditResponseListApiResponse, StringApiResponse } from 'src/open-api';

@Component({
  selector: 'app-audit-manager',
  templateUrl: './audit-manager.component.html',
  styleUrl: './audit-manager.component.scss'
})
export class AuditManagerComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();

  userAudits: AuditResponseListApiResponse | null = null;
  exportStatus: StringApiResponse | null = null;

  constructor(private auditsService: AuditsService) {}

  ngOnInit(): void {
    // Example initial calls
    this.getUserAudits('1'); // Replace '1' with a valid userId
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  /**
   * Fetch audits for a specific user
   * @param userId - User ID
   */
  getUserAudits(userId: string): void {
    this.auditsService
      .apiV1AuditsGetUserAuditsUserIdGet(userId)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          this.userAudits = response;
          console.log('User Audits:', response);
        },
        error: (error) => {
          console.error('Error fetching user audits:', error);
        }
      });
  }

  /**
   * Export audits for a specific user
   * @param userId - User ID
   * @param searchString - Search filter
   * @param searchInOldValues - Search in old values
   * @param searchInNewValues - Search in new values
   */
  exportUserAudits(
    userId: string,
    searchString?: string,
    searchInOldValues?: boolean,
    searchInNewValues?: boolean
  ): void {
    this.auditsService
      .apiV1AuditsExportUserAuditsUserIdGet(
        userId,
        searchString,
        searchInOldValues,
        searchInNewValues
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          this.exportStatus = response;
          console.log('Export Status:', response);
        },
        error: (error) => {
          console.error('Error exporting user audits:', error);
        }
      });
  }
}