import { ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '@app/@auth/services/custom-auth-service';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { Logger } from '@app/@core/utils.ts/logger.service';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { NbWindowService, NbWindowControlButtonsConfig } from '@nebular/theme';
import { TranslateService } from '@ngx-translate/core';
import { APIDefinition, Columns, Config, DefaultConfig, STYLE } from 'ngx-easy-table';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { UserListResponse, UserFilterRequest, UserListResponsePagedListApiResponse, UsersService, CreateUserRequest } from 'src/open-api';
import { AddUserComponent } from '../add-user/add-user.component';
import { DetailsUserComponent } from '../details-user/details-user.component';

@Component({
  selector: 'app-list-users',
  templateUrl: './list-users.component.html',
  styleUrl: './list-users.component.scss'
})
export class ListUsersComponent implements OnInit, OnDestroy {
  private log = new Logger(ListUsersComponent.name);
  private subs: Subscription[] = [];
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  @ViewChild('table') table: APIDefinition;

  public columns: Columns[] = [
    { key: 'id', title: 'ID' },
    { key: 'username', title: 'Username' },
    { key: 'name', title: 'Name' },
    { key: 'email', title: 'Email' },
    { key: 'createdDate', title: 'Created Date' },
    { key: 'isDeleted', title: 'Is Deleted' },
    { key: 'option', title: 'Options' },
  ];

  data: UserListResponse[] = [];
  isLoading: boolean = true;

  public configuration: Config;

  public pagination = {
    limit: 10, // page size
    offset: 0, // page index
    count: -1, // total count
    sort: '',
    order: '',
  };

  filterRequest: UserFilterRequest = {
    isDeleted: false,
    pageIndex: 1,
    pageSize: this.pagination.limit,
    search: '',
  };

  constructor(
    private toastNotificationService: ToastNotificationService,
    private readonly cdr: ChangeDetectorRef,
    private windowService: NbWindowService,
    private apiService: UsersService,
    private authService: AuthService,
    public translationService: TranslateService
  ) {
    this.initializeTableConfig();
  }

  ngOnInit(): void {
    this.fetchData();
    this.getTableHeaderName();
    // Subscribe to language changes
    this.translationService.onLangChange.pipe(takeUntil(this.ngUnsubscribe)).subscribe(() => {
      this.getTableHeaderName();
    });
  }

  ngOnDestroy(): void {
    this.subs.forEach((s) => s.unsubscribe());
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  getTableHeaderName(): void {
    this.columns = [
      { key: 'id', title: this.translationService.instant('TABLE.ID') },
      { key: 'username', title: this.translationService.instant('TABLE.USERNAME') },
      { key: 'name', title: this.translationService.instant('TABLE.NAME') },
      { key: 'email', title: this.translationService.instant('TABLE.EMAIL') },
      { key: 'createdDate', title: this.translationService.instant('TABLE.CREATED_DATE') },
      { key: 'isDeleted', title: this.translationService.instant('TABLE.IS_DELETED') },
      { key: 'option', title: this.translationService.instant('TABLE.OPTIONS') },
    ];
  }

  initializeTableConfig(): void {
    this.configuration = { ...DefaultConfig };
    this.configuration.tableLayout.striped = true;
    this.configuration.tableLayout.hover = true;
    this.configuration.showContextMenu = false;
    this.configuration.fixedColumnWidth = false;
    this.configuration.serverPagination = true;
    this.configuration.horizontalScroll = false;
  }

  eventEmitted(event: { event: string; value: any }): void {
    if (event.event !== 'onClick') {
      this.processGridEvent(event);
    }
  }

  private processGridEvent(obj: any): void {
    if (obj.event === 'onPagination') {
      this.pagination.limit = obj.value.limit ?? this.pagination.limit;
      this.filterRequest.pageSize = obj.value.limit ?? this.pagination.limit;
      this.pagination.offset = obj.value.page ?? this.pagination.offset;
      this.filterRequest.pageIndex = obj.value.page ?? this.filterRequest.pageIndex;
    }

    if (obj.event === 'onOrder') {
      this.pagination.sort = obj.value.key ?? this.pagination.sort;
      this.pagination.order = obj.value.order ?? this.pagination.order;
      this.filterRequest.sort = `${this.pagination.sort}${this.pagination.order}`;
    }

    this.pagination = { ...this.pagination };
    this.fetchData();
  }

  fetchData(): void {
    this.isLoading = true;
    this.apiService
      .apiV1UsersListPost(this.filterRequest)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            this.data = response.data.items ?? [];
            this.pagination.count = response.data.totalCount;
            this.pagination.limit = response.data.pageSize;
            this.pagination.offset = response.data.currentPage;
            this.cdr.markForCheck();
          } else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('user.list'),
              this.translationService.instant('user.list-error')
            );
            this.log.error('No user data received.');
          }
        },
        error: (err) => this.log.error('Error fetching user data:', err),
        complete: () => (this.isLoading = false),
      });
  }

  onAddClicked(): void {
    this.openAddEditWindow();
  }

  onEditClicked(id: number): void {
    this.openAddEditWindow(id);
  }

  onDetailsClicked(id: number): void {
    const buttonsConfig: NbWindowControlButtonsConfig = {
      minimize: true,
      maximize: false,
      fullScreen: true,
      close: true,
    };

    this.windowService.open(null, {
      title: `User Details - #${id}`,
      buttons: buttonsConfig,
      context: { id },
    });
  }

  onDeletedClicked(id: number): void {
    this.apiService.apiV1UsersIdDelete(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          // this.toastrService.success('User deactivated successfully.', 'Success');
          this.toastNotificationService.showToast(
            NotitficationsDefaultValues.Success,
            this.translationService.instant('user.delete'),
            this.translationService.instant('user.delete'));

          this.fetchData();
        },
        error: () => {
          this.toastNotificationService.showToast(
            NotitficationsDefaultValues.Danger,
            this.translationService.instant(''),
            this.translationService.instant(''));
        },
      });
  }

  onReactivateClicked(id: number): void {
    // Assuming there's a separate endpoint to activate a user
    this.apiService.apiV1UsersReActivateIdPut(id)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          // this.toastrService.success('User activated successfully.', 'Success');
          this.toastNotificationService.showToast(
            NotitficationsDefaultValues.Success,
            this.translationService.instant('user.delete'),
            this.translationService.instant('user.delete'));

          this.fetchData();
        },
        error: () => {
          // this.toastrService.danger('Failed to activate user.', 'Error');
          this.toastNotificationService.showToast(
            NotitficationsDefaultValues.Danger,
            this.translationService.instant(''),
            this.translationService.instant(''));
        },
      });
  }

  private openAddEditWindow(id?: number): void {
    const buttonsConfig: NbWindowControlButtonsConfig = {
      minimize: true,
      maximize: false,
      fullScreen: true,
      close: true,
    };

    this.windowService.open(AddUserComponent, {
      title: id ? `Edit User - #${id}` : 'Add New User',
      buttons: buttonsConfig,
      context: { id },
    }).onClose.subscribe(response => {
      if (response === 200) {
        this.fetchData();
      }
    });
  }


  onIdFilterChnaged(valueId: any) {
    this.filterRequest.pageIndex = 1;

    this.fetchData();
  }

  onDeletedFilterChnaged(valueId: any) {
    this.filterRequest.pageIndex = 1;

    this.filterRequest.isDeleted = valueId;
    this.fetchData();
  }


  /**
* Add a new user.
*/
  addUser(): void {
    const newUser: CreateUserRequest = {
      firstName: "Mahmoud",
      middleName: "Mohamed",
      lastName: "Khalifa",
      password: "KhalifaPassword",
      email: "khalifa_CEO1@example.com",
      mobileNumber: "05100000010",
      username: "khalifa_CEO1",
      sensitiveData: "AccountNumberExample",
      birthDate: "1995-07-24",
      groupId: 2,
      departmentId: 5,
      roleIds: [3],
    };

    this.apiService.apiV1UsersPost(newUser)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          // this.toastrService.success('User added successfully.', 'Success');
          this.toastNotificationService.showToast(
            NotitficationsDefaultValues.Success,
            this.translationService.instant('user.delete'),
            this.translationService.instant('user.delete'));

          this.fetchData();
        },
        error: () => {
          // this.toastrService.danger('Failed to add user.', 'Error');

          this.toastNotificationService.showToast(
            NotitficationsDefaultValues.Danger,
            this.translationService.instant('Failed to add user'),
            this.translationService.instant('Error'));
        },
      });
  }

  /**
   * Edit an existing user.
   */
  editUser(userId: number): void {
    const updatedUser: CreateUserRequest = {
      id: userId,
      middleName: "Mohamed 2",
      lastName: "Khalifa 2",
      email: "khalifa_CEO1@example.com",
      mobileNumber: "05100000010",
      username: "khalifa_CEO1",
      sensitiveData: "AccountNumberExample",
      birthDate: "1995-07-24",
      groupId: 2,
      departmentId: 5,
      roleIds: [3],
    };

    this.apiService.apiV1UsersPut(updatedUser)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          // this.toastrService.success('User updated successfully.', 'Success');
          this.toastNotificationService.showToast(
            NotitficationsDefaultValues.Success,
            this.translationService.instant('user.delete'),
            this.translationService.instant('user.delete'));
          this.fetchData();
        },
        error: () => {
          this.toastNotificationService.showToast(
            NotitficationsDefaultValues.Danger,
            this.translationService.instant(''),
            this.translationService.instant(''));
        },
      });
  }

}
