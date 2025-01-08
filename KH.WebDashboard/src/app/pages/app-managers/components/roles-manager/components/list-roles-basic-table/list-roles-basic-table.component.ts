import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '@app/@auth/services/custom-auth-service';
import { RoleApiService } from '@app/@core/api-services/role-api.service';
import { AppDefaultValues } from '@app/@core/const/default-values';
import { NotitficationsDefaultValues } from '@app/@core/const/notitfications-default-values';
import { PopUpWindowTypes } from '@app/@core/const/popup-window-types';
import { CommonLookupFiltersRequest } from '@app/@core/models/base/request/common-lookup-filters-request';
import { ApiResponse } from '@app/@core/models/base/response/custom-api-response';
import { PagedResponse } from '@app/@core/models/base/response/custom-paged-list-response.model';
import { LookupResponse } from '@app/@core/models/base/response/lookup.model';
import { EventObject } from '@app/@core/models/client/event-object.interface';
import { RoleFilterRequest } from '@app/@core/models/requests/role-filter-request';
import { RolesResponse } from '@app/@core/models/responses/roles-response';
import { capitalizeFirstLetter } from '@app/@core/ngx-formly/validations/time-validators';
import { CommonFilterComp } from '@app/@core/utils.ts/components-extentions/common.component';
import { convertToFormData } from '@app/@core/utils.ts/form/form-data-utils';
import { Logger } from '@app/@core/utils.ts/logger.service';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { LanguageEnum } from '@app/@i18n/models/language.enum';
import { NbDialogService, NbPopoverDirective, NbPosition, NbTrigger, NbWindowControlButtonsConfig, NbWindowService } from '@nebular/theme';
import { TranslateService } from '@ngx-translate/core';
import { APIDefinition, Columns, Config, DefaultConfig, STYLE } from 'ngx-easy-table';
import { Subscription, Subject, takeUntil } from 'rxjs';
import { AddRoleComponent } from '../add-role/add-role.component';
import { DetailsRoleComponent } from '../details-role/details-role.component';
import { RolesSignalRService } from '@app/@core/signalR/roles-signalr-services';

@Component({
  selector: 'app-list-roles-basic-table',
  templateUrl: './list-roles-basic-table.component.html',
  styleUrl: './list-roles-basic-table.component.scss'
})
export class ListRolesBasicTableComponent implements OnInit, AfterViewInit, OnDestroy {

  private log = new Logger(ListRolesBasicTableComponent.name);
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

  data: LookupResponse[] = [];
  holdData: LookupResponse[] = [];

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
  // lookupParamsDto: CommonLookupFiltersRequest = {};
  roleFilterRequest: RoleFilterRequest = {
    isDeleted: false,
    pageIndex: 1,
    pageSize: this.pagination.limit,
    search: "",
  };

  constructor(private toastNotificationService: ToastNotificationService,
    private apiService: RoleApiService,
    private readonly cdr: ChangeDetectorRef,
    private windowService: NbWindowService,
    private rolesSignalRService: RolesSignalRService,
    private authService: AuthService,
    public translationService: TranslateService) {
    this.getTableHeaderName()
  }

  ngOnInit() {
    const token = this.authService.getAccessToken(); // Get the JWT token
    this.rolesSignalRService.startConnection(token);
    this.rolesSignalRService.addRoleListener((role) => {
      this.fetchRoles();
    });

    this.initializeTableConfig();
    this.fetchRoles();
    // Subscribe to language changes
    // Subscribe to language changes
    this.translationService.onLangChange
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(() => {
        this.getTableHeaderName();
      });
  }

  ngAfterViewInit(): void {
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
      this.roleFilterRequest.pageSize = obj.value.limit ? obj.value.limit : this.pagination.limit;

      this.pagination.offset = obj.value.page ? obj.value.page : this.pagination.offset;
      this.roleFilterRequest.pageIndex = obj.value.page ? obj.value.page : this.roleFilterRequest.pageIndex;
    }

    if (obj.event === "onOrder") {
      this.pagination.sort = !!obj.value.key ? obj.value.key : this.pagination.sort;
      this.pagination.order = !!obj.value.order ? obj.value.order : this.pagination.order;

      if (this.pagination.order) {
        this.roleFilterRequest.sort = capitalizeFirstLetter(this.pagination.sort) + capitalizeFirstLetter(this.pagination.order);
        // this.roleFilterRequest.sortKey = this.pagination.sort;
        // this.roleFilterRequest.sortOrder = this.pagination.order;
      }
    }



    this.pagination = { ...this.pagination };
    this.fetchRoles();
  }

  onNameEnFilterChnaged(valueId: any) {
    this.roleFilterRequest.pageIndex = 1;
    this.fetchRoles();
  }

  onNameArFilterChnaged(valueId: any) {
    this.roleFilterRequest.pageIndex = 1;

    this.fetchRoles();
  }

  onDescriptionFilterChnaged(valueId: any) {
    this.roleFilterRequest.pageIndex = 1;

    this.fetchRoles();
  }

  onIdFilterChnaged(valueId: any) {
    this.roleFilterRequest.pageIndex = 1;

    this.fetchRoles();
  }

  onDeletedFilterChnaged(valueId: any) {
    this.roleFilterRequest.pageIndex = 1;

    this.roleFilterRequest.isDeleted = valueId;
    this.fetchRoles();
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

  onReactivateClicked(valueId: any): void {

    this.isLoading = true;
    this.apiService.reActivateRole(valueId)
      .pipe(takeUntil(this.ngUnsubscribe)) // Unsubscribe automatically on component destroy
      .subscribe({
        next: (response) => {
          if (response?.statusCode === 200 && response.data) {

            this.toastNotificationService.showToast(
              NotitficationsDefaultValues.Success,
              this.translationService.instant('role.reActivate'),
              this.translationService.instant('role.reActivate') + ":" + valueId);

            this.fetchRoles();
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

  onDetailsClicked(roleId: number) {


    const buttonsConfig: NbWindowControlButtonsConfig = {
      minimize: this.minimize,
      maximize: this.maximize,
      fullScreen: this.fullScreen,
      close: this.close,
    };

    let test = this.windowService.open(
      DetailsRoleComponent,
      {
        title: ' Role ' + '#' + roleId,
        // hasBackdrop: true,
        // closeOnEsc:true,
        buttons: buttonsConfig,
        windowClass: PopUpWindowTypes.FullScreen,
        context:
        {
          roleIdInput: roleId,
        }
      }
    )
      .onClose.subscribe(response => {
        if (response === 200) {
          // this.fetchRoles();
        }
      });

  }

  onAddClicked() {
    this.openRoleFormWindow();
  }

  onEditClicked(valueId: number) {
    this.openRoleFormWindow(valueId);
  }


  private fetchRoles(): void {
    this.isLoading = true;
    this.configuration.isLoading = true;
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
        complete: () => (this.configuration.isLoading = false)
      });
  }

  minimize = true;
  maximize = false;
  fullScreen = true;
  close = true;

  openRoleFormWindow(roleId?: number) {

    const buttonsConfig: NbWindowControlButtonsConfig = {
      minimize: this.minimize,
      maximize: this.maximize,
      fullScreen: this.fullScreen,
      close: this.close,
    };

    let test = this.windowService.open(
      AddRoleComponent,
      {
        title: roleId ? 'Edit Role ' + '#' + roleId : 'Add Role',
        // hasBackdrop: true,
        // closeOnEsc:true,
        buttons: buttonsConfig,
        windowClass: PopUpWindowTypes.Large,
        context:
        {
          roleIdInput: roleId,
        }
      }
    )
      .onClose.subscribe(response => {
        if (response === 200) {
          this.fetchRoles();
        }
      });

  }

}
