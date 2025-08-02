import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { _HttpClient, ALAIN_I18N_TOKEN, SettingsService } from '@delon/theme';
import { ACLService } from '@delon/acl';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalService } from 'ng-zorro-antd/modal';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { Subject, takeUntil } from 'rxjs';
import { UserFilterRequest } from 'src/app/core/models/extentions/user-filter-request';
import { UserService, User, UserResponse } from '../user.service';
import { SHARED_IMPORTS } from '@shared';
import { AddUserProfileComponent } from '../add-user-profile/add-user-profile.component';
import { UserDetailsComponent } from '../user-details/user-details.component';
// import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'app-list-user-profiles',
  imports: [SHARED_IMPORTS],
  templateUrl: './list-user-profiles.component.html',
  styleUrl: './list-user-profiles.component.less'
})
export class ListUserProfilesComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  private readonly http = inject(_HttpClient);
  private readonly msg = inject(NzMessageService);
  private readonly modalSrv = inject(NzModalService);
  private readonly i18nSrv = inject(ALAIN_I18N_TOKEN);
  private readonly userService = inject(UserService);
  private readonly aclService = inject(ACLService);
  private readonly settingsService = inject(SettingsService);

  userFilterRequest: UserFilterRequest = {
    pageIndex: 1,
    pageSize: 10,
    sortOrder: 'IdDesc',
    sortKey: 'Id',
    search: '',
    ignoreCache: false
  };

  index1 = 0;
  index2 = 0;
  data: User[] = [];
  loading = false;
  total = 0;
  pageIndex = 1;
  pageSize = 10;
  searchValue = '';

  // Role filters for dropdown
  roleFilters = [
    { text: 'Doctor', value: 'Doctor' },
    { text: 'Nurse', value: 'Nurse' },
    { text: 'Receptionist', value: 'Receptionist' },
    { text: 'Pharmacist', value: 'Pharmacist' },
    { text: 'Lab Technician', value: 'Lab Technician' },
    { text: 'Radiologist', value: 'Radiologist' },
    { text: 'Administrator', value: 'Administrator' },
    { text: 'Accountant', value: 'Accountant' },
    { text: 'Security Guard', value: 'Security Guard' },
    { text: 'Cleaner', value: 'Cleaner' }
  ];

  // Status filters for dropdown
  statusFilters = [
    { text: 'Active', value: true },
    { text: 'Inactive', value: false }
  ];

  // Table configuration
  tableConfig = {
    pagination: true,
    pageSizeChanger: true,
    title: true,
    columnHeader: true,
    footer: true,
    expandable: true,
    checkbox: false,
    tableScroll: { x: '1200px', y: '400px' },
    tableLayout: 'fixed' as const,
    paginationPosition: 'bottom' as const,
    paginationType: 'default' as const,
    size: 'small' as const,
    fixedHeader: true
  };

  // Expandable configuration
  expandConfig = {
    expandRowByClick: true
  };

  ngOnInit(): void {
    this.loadUsers();
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  loadUsers(): void {
    this.loading = true;
    this.userService.getUsers(this.userFilterRequest)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response: UserResponse) => {
          this.data = response.data.map(user => ({
            ...user,
            expand: false
          }));
          this.total = response.total;
          this.pageIndex = response.pageIndex;
          this.pageSize = response.pageSize;
          this.loading = false;
        },
        error: (error) => {
          console.error('Error loading users:', error);
          this.loading = false;
          this.msg.error('Error loading users');
        }
      });
  }

  onQueryParamsChange(params: NzTableQueryParams): void {
    const { pageIndex, pageSize, sort, filter } = params;

    this.userFilterRequest.pageIndex = pageIndex;
    this.userFilterRequest.pageSize = pageSize;

    if (sort && sort.length > 0) {
      const sortItem = sort[0];
      this.userFilterRequest.sortKey = sortItem.key;
      this.userFilterRequest.sortOrder = sortItem.value === 'ascend' ? 'Asc' : 'Desc';
    }

    // let paramss = new HttpParams()
    //   .append('page', `${pageIndex}`)
    //   .append('results', `${pageSize}`)
    //   .append('sortField', `${this.userFilterRequest.sortKey}`)
    //   .append('sortOrder', `${this.userFilterRequest.sortOrder}`);
      
    //   filter.forEach((filter:any) => {
    //   filter.value.forEach((value:any) => {
    //     paramss = paramss.append(filter.key, value);
    //   });});

    this.loadUsers();
  }

  onSearch(): void {
    this.userFilterRequest.search = this.searchValue;
    this.userFilterRequest.pageIndex = 1;
    this.loadUsers();
  }

  onRoleFilterChange(selectedRoles: string[]): void {
    this.userFilterRequest.role = selectedRoles.length > 0 ? selectedRoles.join(',') : '';
    this.userFilterRequest.pageIndex = 1;
    this.loadUsers();
  }

  onStatusFilterChange(selectedStatuses: boolean[]): void {
    this.userFilterRequest.is_active = selectedStatuses.length > 0 ? selectedStatuses[0] : undefined;
    this.userFilterRequest.pageIndex = 1;
    this.loadUsers();
  }



  editUser(user: User): void {
    const modalRef = this.modalSrv.create({
      nzTitle: this.i18nSrv.fanyi('user.form.title.edit'),
      nzContent: AddUserProfileComponent,
      nzWidth: '80%',
      nzClosable: true,
      nzMaskClosable: false,
      nzDraggable: true,
      nzFooter: null,
      nzData: {
        userId: user.id,
        mode: 'edit'
      }
    });

    modalRef.afterClose
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((result) => {
        if (result) {
          this.loadUsers();
        }
      });
  }

  viewUserDetails(user: User): void {
    const modalRef = this.modalSrv.create({
      nzTitle: `User Details - ${user.name_en}`,
      nzContent: UserDetailsComponent,
      nzWidth: '90%',
      nzClosable: true,
      nzMaskClosable: false,
      nzDraggable: true,
      nzFooter: null,
      nzData: {
        userId: user.id
      }
    });

    modalRef.afterClose
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((result) => {
        if (result && result.action === 'edit') {
          // If user wants to edit from details modal, open edit modal
          this.editUser({ ...user, id: result.userId });
        } else if (result) {
          // Refresh the list if any changes were made
          this.loadUsers();
        }
      });
  }

  addUser(): void {
    const modalRef = this.modalSrv.create({
      nzTitle: this.i18nSrv.fanyi('user.form.title.add'),
      nzContent: AddUserProfileComponent,
      nzWidth: '80%',
      nzClosable: true,
      nzMaskClosable: false,
      nzDraggable: true,
      nzFooter: null,
      nzData: {
        mode: 'add'
      }
    });

    modalRef.afterClose
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe((result) => {
        if (result) {
          this.loadUsers();
        }
      });
  }

  activateUser(userId: number): void {
    this.userService.updateUserStatus(userId, true)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (success) => {
          if (success) {
            this.msg.success(this.i18nSrv.fanyi('user.status.success.activated'));
            this.loadUsers();
          } else {
            this.msg.error(this.i18nSrv.fanyi('user.status.error.updating'));
          }
        },
        error: (error) => {
          console.error('Error activating user:', error);
          this.msg.error(this.i18nSrv.fanyi('user.status.error.updating'));
        }
      });
  }

  deactivateUser(userId: number): void {
    this.userService.updateUserStatus(userId, false)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (success) => {
          if (success) {
            this.msg.success(this.i18nSrv.fanyi('user.status.success.deactivated'));
            this.loadUsers();
          } else {
            this.msg.error(this.i18nSrv.fanyi('user.status.error.updating'));
          }
        },
        error: (error) => {
          console.error('Error deactivating user:', error);
          this.msg.error(this.i18nSrv.fanyi('user.status.error.updating'));
        }
      });
  }

  currentPageDataChange(data: readonly User[]): void {
    console.log('currentPageDataChange: ', data);
  }

  getRoleColor(role: string): string {
    const roleColors: { [key: string]: string } = {
      'Doctor': 'blue',
      'Nurse': 'green',
      'Receptionist': 'orange',
      'Pharmacist': 'purple',
      'Lab Technician': 'cyan',
      'Radiologist': 'magenta',
      'Administrator': 'red',
      'Accountant': 'gold',
      'Security Guard': 'lime',
      'Cleaner': 'default'
    };
    return roleColors[role] || 'default';
  }

  get activeUsersCount(): number {
    return this.data.filter(user => user.is_active).length;
  }

  get inactiveUsersCount(): number {
    return this.data.filter(user => !user.is_active).length;
  }

  get lastUpdatedDate(): string {
    return this.data.length > 0 ? this.data[0].created_at : '';
  }

  onExpandChange(user: User, expanded: boolean): void {
    console.log('Expand change:', user.id, expanded);
    user.expand = expanded;
  }


}

