import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AuthService } from '@app/@auth/services/custom-auth-service';
import { RoleApiService } from '@app/@core/api-services/role-api.service';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { PopUpWindowTypes } from '@app/@core/const/popup-window-types';
import { LookupResponse } from '@app/@core/models/base/response/lookup.model';
import { EventObject } from '@app/@core/models/client/event-object.interface';
import { capitalizeFirstLetter } from '@app/@core/ngx-formly/validations/time-validators';
import { RolesSignalRService } from '@app/@core/signalR/roles-signalr-services';
import { Logger } from '@app/@core/utils.ts/logger.service';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { NbWindowService, NbWindowControlButtonsConfig } from '@nebular/theme';
import { TranslateService } from '@ngx-translate/core';
import { APIDefinition, Columns, Config, DefaultConfig, STYLE } from 'ngx-easy-table';
import { Subscription, Subject, takeUntil } from 'rxjs';
import { DepartmentFilterRequest, DepartmentListResponse, DepartmentListResponsePagedListApiResponse, DepartmentsService, RoleFilterRequest } from 'src/open-api';
import { AddRoleComponent } from '../../../roles-manager/components/add-role/add-role.component';
import { DetailsRoleComponent } from '../../../roles-manager/components/details-role/details-role.component';
import { AddDepartmentComponent } from '../add-department/add-department.component';
import { DetailsDepartmentComponent } from '../details-department/details-department.component';

@Component({
  selector: 'app-list-department',
  templateUrl: './list-department.component.html',
  styleUrl: './list-department.component.scss'
})
export class ListDepartmentComponent implements OnInit, OnDestroy {

  private log = new Logger(ListDepartmentComponent.name);
  private subs: Subscription[] = [];
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  @ViewChild('table') table: APIDefinition;

  public columns: Columns[] = [
    { key: 'Id', title: 'Id' },
    { key: 'NameEn', title: 'NameEn' },
    { key: 'NameAr', title: 'NameAr' },
    { key: 'Description', title: 'Description' },
    { key: 'IsDeleted', title: 'IsDeleted' },
  ];

  data: DepartmentListResponse[] = [];
  holdData: DepartmentListResponse[] = [];

  isLoading: boolean = true;
  compasearchByReeasonIdny: string = '';
  searchByReeasonNameEnVal: string = '';
  searchByReeasonNameArVal: string = '';
  searchByReeasonDescriptionVal: string = '';


  standAloneInputRegExp: RegExp = /^[a-zA-Z\s]*$/;
  inputControlRegExp: RegExp = /^[a-zA-Z0-9]*$/; // Allows only alphanumeric characters


  public configuration: Config;

  public pagination = {
    limit: 10,//page-size
    offset: 0,//page index
    count: -1,
    sort: '',
    order: '',
  };

  constructor(private toastNotificationService: ToastNotificationService,
    private readonly cdr: ChangeDetectorRef,
    private windowService: NbWindowService,
    private apiService: DepartmentsService,
    private authService: AuthService,
    public translationService: TranslateService) {
    this.getTableHeaderName()
  }

  ngOnInit() {
    const token = this.authService.getAccessToken(); // Get the JWT token
    this.initializeTableConfig();
    this.fetchData();
    // Subscribe to language changes
    this.translationService.onLangChange
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.getTableHeaderName();
      });
  }

  ngOnDestroy() {
    this.subs.forEach((s) => s.unsubscribe());
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  getTableHeaderName(): void {
    this.columns = [
      { key: 'id', title: this.translationService.instant('TABLE.ID') },
      { key: 'nameEn', title: this.translationService.instant('TABLE.NAME_EN') },
      { key: 'nameAr', title: this.translationService.instant('TABLE.NAME_AR') },
      { key: 'description', title: this.translationService.instant('TABLE.DESCRIPTION') },
      { key: 'isDeleted', title: this.translationService.instant('TABLE.IS_DELETED') },
      { key: 'option', title: this.translationService.instant('TABLE.OPTIONS') },
    ];
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
    this.configuration.tableLayout.style = STYLE.TINY;
    this.configuration.tableLayout.borderless = false;
    // this.configuration.checkboxes = true;
    this.configuration.serverPagination = true;
    this.configuration.horizontalScroll = false;
  }

  //to get the sor filter event of the grid
  eventEmitted(event: { event: string; value: any }): void {
    if (event.event !== 'onClick') {
      this.processGridEvent(event);
    }
  }

  // Processes sorting and pagination events for the grid
  private processGridEvent(obj: EventObject): void {

    if (obj.event === "onPagination") {
      this.pagination.limit = obj.value.limit ? obj.value.limit : this.pagination.limit;
      this.filterRequest.pageSize = obj.value.limit ? obj.value.limit : this.pagination.limit;

      this.pagination.offset = obj.value.page ? obj.value.page : this.pagination.offset;
      this.filterRequest.pageIndex = obj.value.page ? obj.value.page : this.filterRequest.pageIndex;
    }

    if (obj.event === "onOrder") {
      this.pagination.sort = !!obj.value.key ? obj.value.key : this.pagination.sort;
      this.pagination.order = !!obj.value.order ? obj.value.order : this.pagination.order;

      if (this.pagination.order) {
        this.filterRequest.sort = capitalizeFirstLetter(this.pagination.sort) + capitalizeFirstLetter(this.pagination.order);
        // this.roleFilterRequest.sortKey = this.pagination.sort;
        // this.roleFilterRequest.sortOrder = this.pagination.order;
      }
    }



    this.pagination = { ...this.pagination };
    this.fetchData();
  }

  onNameEnFilterChnaged(valueId: any) {
    this.filterRequest.pageIndex = 1;
    this.fetchData();
  }

  onNameArFilterChnaged(valueId: any) {
    this.filterRequest.pageIndex = 1;

    this.fetchData();
  }

  onDescriptionFilterChnaged(valueId: any) {
    this.filterRequest.pageIndex = 1;

    this.fetchData();
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

  onDeletedClicked(valueId: any): void {

    this.isLoading = true;
    this.apiService.apiV1DepartmentsIdDelete(valueId)
      .pipe(takeUntil(this.ngUnsubscribe)) // Unsubscribe automatically on component destroy
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {

            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Success,
              this.translationService.instant('role.delete'),
              this.translationService.instant('role.delete') + ":" + valueId);

            this.fetchData();
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

  onReactivateClicked(valueId: any): void {

    this.isLoading = true;
    this.apiService.apiV1DepartmentsReActivateIdPut(valueId)
      .pipe(takeUntil(this.ngUnsubscribe)) // Unsubscribe automatically on component destroy
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {

            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Success,
              this.translationService.instant('role.reActivate'),
              this.translationService.instant('role.reActivate') + ":" + valueId);

            this.fetchData();
          }
          else {
            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Danger,
              this.translationService.instant('role.reActivate'),
              this.translationService.instant('role.reActivate-erorr'));
            this.log.error('No roles data deleted.');
          }
        },
        error: (err) => this.log.error('Error reActivate roles:', err),
        complete: () => (this.isLoading = false)
      });
  }

  onDetailsClicked(id: number) {


    const buttonsConfig: NbWindowControlButtonsConfig = {
      minimize: this.minimize,
      maximize: this.maximize,
      fullScreen: this.fullScreen,
      close: this.close,
    };

    let test = this.windowService.open(
      DetailsDepartmentComponent,
      {
        title: '  ' + '#' + id,
        // hasBackdrop: true,
        // closeOnEsc:true,
        buttons: buttonsConfig,
        windowClass: PopUpWindowTypes.FullScreen,
        context:
        {
          idInput: id,
        }
      }
    )
      .onClose.subscribe(response => {
        if (response === 200) {
          // this.fetchData();
        }
      });

  }

  onAddClicked() {
    this.openAddFormWindow();
  }

  onEditClicked(valueId: number) {
    this.openAddFormWindow(valueId);
  }

  filterRequest: DepartmentFilterRequest = {
    isDeleted: false,
    pageIndex: 1,
    pageSize: this.pagination.limit,
    search: "",
  };

  pagedDepartmentResponse: DepartmentListResponsePagedListApiResponse | null = null;

  // Fetch paginated departments
  fetchData(): void {

    this.apiService
      .apiV1DepartmentsPagedListPost(this.filterRequest) // Pass the filter object
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (response) => {
          this.pagedDepartmentResponse = response;
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
        error: (error) => {
          console.error('Error fetching paged department list:', error);
        }
      });
  }

  minimize = true;
  maximize = false;
  fullScreen = true;
  close = true;

  openAddFormWindow(id?: number) {

    const buttonsConfig: NbWindowControlButtonsConfig = {
      minimize: this.minimize,
      maximize: this.maximize,
      fullScreen: this.fullScreen,
      close: this.close,
    };

    let test = this.windowService.open(
      AddDepartmentComponent,
      {
        title: id ? 'Edit  ' + '#' + id : 'Add New',
        // hasBackdrop: true,
        // closeOnEsc:true,
        buttons: buttonsConfig,
        windowClass: PopUpWindowTypes.Large,
        context:
        {
          idInput: id,
        }
      }
    )
      .onClose.subscribe(response => {
        if (response === 200) {
          this.fetchData();
        }
      });

  }

}
