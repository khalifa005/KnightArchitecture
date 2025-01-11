import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { UsersService, AuditsService, UserFilterRequest, AuditResponseListApiResponse, StringApiResponse } from 'src/open-api';
import { saveAs } from 'file-saver'; // Install with `npm install file-saver`

@Component({
  selector: 'app-audit-manager',
  templateUrl: './audit-manager.component.html',
  styleUrl: './audit-manager.component.scss'
})
export class AuditManagerComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();

  userAudits: AuditResponseListApiResponse | null = null;
  exportStatusMessage: string | null = null;

  constructor(private auditsService: AuditsService) {}

  ngOnInit(): void {
    // Fetch audits for a specific user (example userId: '1')
    this.getUserAudits('1');
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
   * @param searchString - Search term
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
        searchInNewValues,
        'response' // To handle raw response
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          const blob = new Blob([response.body], { type: 'application/octet-stream' });
          saveAs(blob, `user-audits-${userId}.csv`); // Example file name
          this.exportStatusMessage = 'Export successful. File downloaded.';
        },
        error: (error) => {
          this.exportStatusMessage = 'Export failed. Please try again.';
          console.error('Error exporting user audits:', error);
        }
      });
  }

  exportUserAuditsx(
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
      searchInNewValues,
      'response' // To handle raw response
 // Specify the expected content type for files
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          const blob = new Blob([response.body], { type: 'application/octet-stream' });
          const url = window.URL.createObjectURL(blob);
  
          // Create a temporary anchor element to trigger the download
          const a = document.createElement('a');
          a.href = url;
          a.download = `user-audits-${userId}.csv`; // Example file name
          document.body.appendChild(a);
          a.click();
  
          // Clean up the DOM by removing the temporary anchor element
          document.body.removeChild(a);
          window.URL.revokeObjectURL(url);
  
          this.exportStatusMessage = 'Export successful. File downloaded.';
        },
        error: (error) => {
          this.exportStatusMessage = 'Export failed. Please try again.';
          console.error('Error exporting user audits:', error);
        }
      });
  }

  exportUserAudits(
    userId: string,
    searchString: string = '',
    searchInOldValues: boolean = false,
    searchInNewValues: boolean = false
  ): void {
    this.auditsService
      .apiV1AuditsExportUserAuditsUserIdGet(
        userId,
        searchString,
        searchInOldValues,
        searchInNewValues,
        'response', // Ensures full HTTP response is returned
        false,
        { httpHeaderAccept: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' }
      )
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          // Create a Blob from the response
          const blob = new Blob([response.body], {
            type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
          });
  
          // Create a download link and trigger download
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = 'AuditTrails.xlsx';
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
  
          window.URL.revokeObjectURL(url);
          this.exportStatusMessage = 'Export successful. File downloaded.';
        },
        error: (error) => {
          this.exportStatusMessage = 'Export failed. Please try again.';
          console.error('Error exporting user audits:', error);
        }
      });
  }
  
}