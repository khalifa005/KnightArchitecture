import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { DefaultConfig, STYLE } from 'ngx-easy-table';
import { Subject, takeUntil } from 'rxjs';
import { UserListResponsePagedListApiResponse, UsersService, UserFilterRequest } from 'src/open-api';

@Component({
  selector: 'app-user-manager',
  templateUrl: './user-manager.component.html',
  styleUrl: './user-manager.component.scss'
})
export class UserManagerComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();

  data: any[] = [];
  // columns = [
  //   { key: 'id', title: 'ID' },
  //   { key: 'username', title: 'Username' },
  //   { key: 'email', title: 'Email' },
  //   { key: 'role', title: 'Role' },
  //   { key: 'isDeleted', title: 'Status' },
  //   { key: 'option', title: 'Option' },

  // ];
  columns: { key: string; title: string }[] = [];

  configuration: any;
  pagination = { limit: 10, offset: 0, count: -1, sort: '', order: '' };
  userFilterRequest: UserFilterRequest = {
    pageIndex: 1,
    pageSize: this.pagination.limit,
    isDeleted: false,
  };

  constructor(private usersService: UsersService, 
    private translateService: TranslateService,
    private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadColumnTranslations();

    this.initializeTableConfig();
    this.fetchUsers();

      // Subscribe to language change for dynamic updates
      this.translateService.onLangChange.pipe(takeUntil(this.ngUnsubscribe)).subscribe(() => {
        this.loadColumnTranslations();
      });
  }

  loadColumnTranslations(): void {
    this.columns = [
      { key: 'id', title: this.translateService.instant('TABLE.ID') },
      { key: 'username', title: this.translateService.instant('TABLE.USERNAME') },
      { key: 'email', title: this.translateService.instant('TABLE.EMAIL') },
      { key: 'role', title: this.translateService.instant('TABLE.ROLE') },
      { key: 'isDeleted', title: this.translateService.instant('TABLE.IS_DELETED') },
      { key: '', title: this.translateService.instant('TABLE.OPTIONS') },
    ];
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  initializeTableConfig(): void {
     this.configuration = { ...DefaultConfig };
        // this.configuration.isLoading = true;
        this.configuration.showContextMenu = false;
        this.configuration.resizeColumn = false;
        this.configuration.columnReorder = true;
        this.configuration.fixedColumnWidth = false;
        this.configuration.tableLayout.hover = true;
        this.configuration.tableLayout.striped = true;
        this.configuration.tableLayout.style = STYLE.NORMAL;
        this.configuration.tableLayout.borderless = false;
        // this.configuration.checkboxes = true;
        this.configuration.serverPagination = true;
        this.configuration.horizontalScroll = false;
  }

  fetchUsers(): void {
    this.usersService.apiV1UsersListPost(this.userFilterRequest)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.data) {
            this.data = response.data.items || [];
            this.pagination.count = response.data.totalCount || 0;
            this.cdr.markForCheck();
          }
        },
        error: (err) => console.error('Error fetching users:', err),
      });
  }

  onFilterChanged(): void {
    this.userFilterRequest.pageIndex = 1;
    this.fetchUsers();
  }

  eventEmitted(event: { event: string; value: any }): void {
    if (event.event === 'onPagination') {
      this.pagination.limit = event.value.limit || this.pagination.limit;
      this.pagination.offset = event.value.page || this.pagination.offset;
      this.userFilterRequest.pageIndex = this.pagination.offset;
      this.fetchUsers();
    } else if (event.event === 'onOrder') {
      this.pagination.sort = event.value.key || this.pagination.sort;
      this.pagination.order = event.value.order || this.pagination.order;
      this.userFilterRequest.sort = `${this.pagination.sort}_${this.pagination.order}`;
      this.fetchUsers();
    }
  }

  onAddClicked(): void {
    // Logic to open Add User window
  }

  onEditClicked(userId: number): void {
    // Logic to open Edit User window
  }

  onDetailsClicked(userId: number): void {
    // Logic to view user details
  }

  onDeactivateClicked(userId: number): void {
    // Logic to deactivate user
  }

  onActivateClicked(userId: number): void {
    // Logic to reactivate user
  }
}