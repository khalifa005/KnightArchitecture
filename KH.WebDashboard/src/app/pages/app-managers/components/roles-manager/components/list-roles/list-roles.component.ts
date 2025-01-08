import { ChangeDetectorRef, Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { RoleApiService } from '@app/@core/api-services/role-api.service';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { LookupResponse } from '@app/@core/models/base/response/lookup.model';
import { RoleFilterRequest } from '@app/@core/models/requests/role-filter-request';
import { capitalizeFirstLetter } from '@app/@core/ngx-formly/validations/time-validators';
import { Logger } from '@app/@core/utils.ts/logger.service';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { NbWindowService } from '@nebular/theme';
import { TranslateService } from '@ngx-translate/core';
import { Config, DefaultConfig, STYLE } from 'ngx-easy-table';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-list-roles',
  templateUrl: './list-roles.component.html',
  styleUrl: './list-roles.component.scss'
})
export class ListRolesComponent implements OnInit {

  // Define table columns
  tableColumns = [
    {
      key: 'id',
      title: 'ID',
      filterType: 'input', // Dynamic filter type
      placeholder: 'Search by ID',
      cellTemplateKey: 'textCellTemplate'
    },
    {
      key: 'nameEn',
      title: 'Name (English)',
      filterType: 'input', // Dynamic filter type
      placeholder: 'Search by English Name',
      cellTemplateKey: 'textCellTemplate'
    },
    {
      key: 'nameAr',
      title: 'Name (Arabic)',
      filterType: 'input', // Dynamic filter type
      placeholder: 'Search by Arabic Name',
      cellTemplateKey: 'textCellTemplate'
    },
    {
      key: 'customaction',
      title: 'customAction',
      cellTemplateKey: 'customactionTemplate' // Use custom template
    },
  ];

  public pagination = {
    limit: 10,//page-size
    offset: 0,//page index
    count: -1,
    sort: '',
    order: '',
  };


  constructor(private toastNotificationService: ToastNotificationService,
    private apiService: RoleApiService,
    private readonly cdr: ChangeDetectorRef,
    private windowService: NbWindowService,
    public translationService: TranslateService) {
  }

  ngOnInit() {
    this.initializeTableConfig();
    this.fetchRoles();
  }

  // Handle filter changes
  onFilterChangedxx(filters: any) {
    console.log('Filters applied:', filters);

    Object.entries(filters).forEach(([key, value]) => {
      console.log(`Filter Key: ${key}, Filter Value: ${value}`);
    });


    this.fetchRoles();
  }


  onFilterChanged(filters: any) {
    console.log('Filters applied:', filters);
  
    // Convert filters to JSON object
    const filterObject: any = {};
    Object.entries(filters).forEach(([key, value]) => {
      console.log(`Filter Key: ${key}, Filter Value: ${value}`);
      // Add only non-empty values to the filter object
      if (value !== null && value !== undefined ) {
        filterObject[key] = value;
      }
    });
  
    // Merge filterObject with roleFilterRequest properties that have values
    const mergedFilters = {
      ...this.roleFilterRequest,
      ...filterObject,
    };
  
    // Log the final merged object
    console.log('Merged Filters:', mergedFilters);
  
    // Update roleFilterRequest with the merged filters
    this.roleFilterRequest = mergedFilters;
  
    // Fetch roles with the updated filters
    this.fetchRoles();
  }
  

  // Handle pagination changes
  onPaginationChanged(obj: any) {
    console.log('Pagination updated:', obj);
    // // Add logic to handle pagination, e.g., fetch the next set of data
    // this.pagination.limit = obj.limit ? obj.limit : this.pagination.limit;
    // this.roleFilterRequest.pageSize = obj.limit ? obj.limit : this.pagination.limit;

    // this.pagination.offset = obj.value.page ? obj.value.page : this.pagination.offset;
    this.roleFilterRequest.pageIndex = obj.offset ? obj.offset : this.roleFilterRequest.pageIndex;
    this.roleFilterRequest.pageSize = obj.limit ? obj.limit : this.roleFilterRequest.pageSize;
    this.fetchRoles();

  }

  onSortingChanged(obj: any) {

    this.roleFilterRequest.sort = capitalizeFirstLetter(obj.sortColumnKey) + capitalizeFirstLetter(obj.sortOrder);

    this.fetchRoles();
  }

  editUser(userId: number) {
    // Placeholder for editing logic. Replace with your implementation.
    console.log(`Edit user with ID ${userId}`);
  }

  editUserHandler(userId: number) {
    console.log(`Edit requested for user with ID: ${userId}`);
    // Implement your edit logic here
  }

  deleteUserHandler(userId: number) {
    // this.rolesData = this.rolesData.filter(user => user.id !== userId);
    console.log(`User with ID ${userId} deleted.`);
  }

  // if need to send custom html template
  @ViewChild('customactionTemplate') customactionTemplate: TemplateRef<any>;


  roleFilterRequest: RoleFilterRequest = {
    isDeleted: false,
    pageIndex: 1,
    pageSize: this.pagination.limit,
    search: "",
  };

  private log = new Logger(ListRolesComponent.name);
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  isLoading: boolean = true;
  data: LookupResponse[] = [];

  private fetchRoles(): void {
    this.isLoading = true;
    this.apiService.getPagedRoles(this.roleFilterRequest)
      .pipe(takeUntil(this.ngUnsubscribe)) // Unsubscribe automatically on component destroy
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {
            this.data = response.data.items ?? [];
            this.cdr.markForCheck();
            // ensure this.pagination.count is set only once and contains count of the whole array, not just paginated one
            this.pagination.count = response.data.totalCount;
            this.pagination.limit = response.data.pageSize;
            this.pagination.offset = response.data.currentPage;
            this.pagination = { ...this.pagination };
          }
          else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('role.list'),
              this.translationService.instant('role.lis-erorr'));
            this.log.error('No roles data received.');
          }
        },
        error: (err) => this.log.error('Error fetching roles:', err),
        complete: () => (this.isLoading = false)
      });
  }

  onDeletedClicked(valueId: any): void {

    this.isLoading = true;
    this.apiService.deleteRole(valueId)
      .pipe(takeUntil(this.ngUnsubscribe)) // Unsubscribe automatically on component destroy
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {

            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Success,
              this.translationService.instant('role.delete'),
              this.translationService.instant('role.delete') + ":" + valueId);

            this.fetchRoles();
          }
          else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('role.delete'),
              this.translationService.instant('role.delete-erorr'));
            this.log.error('No roles data deleted.');
          }
        },
        error: (err) => this.log.error('Error dekleting roles:', err),
        complete: () => (this.isLoading = false)
      });
  }

  public configuration: Config;

  initializeTableConfig(): void {
    this.configuration = { ...DefaultConfig };
    // this.configuration.isLoading = true;
    this.configuration.showContextMenu = false;
    this.configuration.resizeColumn = false;
    this.configuration.columnReorder = true;
    this.configuration.fixedColumnWidth = false;
    this.configuration.tableLayout.hover = true;
    this.configuration.tableLayout.striped = true;
    this.configuration.tableLayout.style = STYLE.TINY;
    this.configuration.tableLayout.borderless = false;
    // this.configuration.checkboxes = true;
    this.configuration.serverPagination = true;
    this.configuration.horizontalScroll = false;
  }
}