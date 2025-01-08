import { HttpEventType } from '@angular/common/http';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { NbDateService, NbWindowRef } from '@nebular/theme';
import { TranslateService } from '@ngx-translate/core';
import { Subscription, Subject, takeUntil } from 'rxjs';
import { FileApiService } from '../../../../@core/api-services/fileApiService';
import { NotitficationsDefaultValues } from '../../../../@core/const/notitfications-default-values';
import { StatusCode } from '../../../../@core/enums/status-code.enum';
import { LookupResponse } from '../../../../@core/models/base/response/lookup.model';
import { Logger } from '../../../../@core/utils.ts/logger.service';
import { ToastNotificationService } from '../../../../@core/utils.ts/toast-notification.service';
import { I18nService } from '../../../../@i18n/services/i18n.service';
import { fixDatesInModel } from '../../../../@core/utils.ts/date/date-helper-functions';
import { ExportParameter } from '../../../../@core/models/export-parameter';
import { ReportParameters } from '../../../../@core/models/reports-parameters';

@Component({
  selector: 'app-export-data',
  templateUrl: './export-data.component.html',
  styleUrls: ['./export-data.component.scss']
})
export class ExportDataComponent implements OnInit , OnDestroy {

  private log = new Logger(ExportDataComponent.name);
  private subs: Subscription[] = [];
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  @Input() selectOptions: LookupResponse[];
  @Input() apiUrl: string;
  //later try to make a generic dto for exporting data - use url and dto to export any data
  @Input() exportParameter: ExportParameter; 

  progress: number;
  paramsDto = new ReportParameters();
  selectedColumns : LookupResponse[];

  exportLoadingSpinner : boolean = false;
  searchLoadingSpinner : boolean = false;

  min: Date;
  max: Date;
  fromDate = new FormControl(new Date());
  toDate = new FormControl(new Date());

  constructor(private toastNotificationService:ToastNotificationService,
    protected dateService: NbDateService<Date>,
     private i18nService: I18nService,
     private fileApiService: FileApiService,
     public windowRef: NbWindowRef<ExportDataComponent>,
    public localizationService: TranslateService) {
    //consider this api
    this.paramsDto.IsDeleted = false;
    this.paramsDto.PageIndex = 1;
    this.paramsDto.Search = "";

    // this.paramsDto.ExportInArbic = i18nService.language == LanguageEnum.Ar;

    this.fromDate.patchValue(this.dateService.addDay(this.monthStart, 0));
    this.toDate.patchValue(this.dateService.addDay(this.monthEnd, 0));

    this.min = this.dateService.addDay(this.dateService.today(), -45);
    this.max = this.dateService.addDay(this.dateService.today(), 45);
  }

  get monthStart(): Date {
    return this.dateService.getMonthStart(new Date());
  }

  get monthEnd(): Date {
    return this.dateService.getMonthEnd(new Date());
  }

  ngOnInit() {
  }

  whenOptionsChnaged(items:any)
  {
    this.selectedColumns = items;

    this.exportParameter.selectedColumnsToExportObj = this.selectedColumns;
  }

  exportData() {

    this.exportLoadingSpinner = true;

    this.exportParameter.fromDate = this.fromDate.value;
    this.exportParameter.toDate = this.toDate.value;
    this.exportParameter.selectedColumnsToExport = this.exportParameter.selectedColumnsToExportObj?.map(x=> x.columnDbName);

    this.exportParameter = fixDatesInModel(this.exportParameter);
    
  //   this.fileApiService
  //   .exportGeneralResult(this.exportParameter)
  //   .pipe(takeUntil(this.ngUnsubscribe))
  //   .subscribe((response : any) => {

  //     if (response.type === HttpEventType.UploadProgress)
  //               this.progress = Math.round((100 * response.loaded) / response.total);
  //     else if (response.type === HttpEventType.Response) {
  //               let downloadLink = document.createElement('a');
  //               downloadLink.href = window.URL.createObjectURL(response.body);
  //               let date: Date = new Date();

  //               // Get year, month, and day part from the date
  //               var year = date.toLocaleString("default", { year: "numeric" });
  //               var month = date.toLocaleString("default", { month: "2-digit" });
  //               var day = date.toLocaleString("default", { day: "2-digit" });

  //               // Generate yyyy-mm-dd date string
  //               var formattedDate = year + "-" + month + "-" + day;
  //               // this.log.info(formattedDate);

  //               downloadLink.setAttribute('download', 'snapshot data-' + formattedDate);
  //               document.body.appendChild(downloadLink);
  //               // window.open(downloadLink.href, "_blank"); //popups
  //               downloadLink.click();
  //               downloadLink.remove();

  //               this.exportLoadingSpinner = false;
  //               this.cancel(StatusCode.Success);
  //           }
  //  },
  //  (erorr:any) => {
  //   this.exportLoadingSpinner = false;
  //   this.cancel(StatusCode.Bad);

  //  });
  
   }

   public cancel(statusCode:number) {
    this.windowRef.close(statusCode);
  }

  
  isValidFilter()
  {

    if(!this.fromDate || !this.toDate ){
      this.log.info(this.fromDate.value);
      this.log.info(this.toDate.value);

      this.toastNotificationService.showToast(NotitficationsDefaultValues.Danger, 'select from - to filter', "erorr");
      return false;
    }
    this.log.info(this.fromDate.value);
    this.log.info(this.toDate.value);

    this.paramsDto.FromDate = this.fromDate.value;
    this.paramsDto.ToDate = this.toDate.value;
    return true;
  }


  ngOnDestroy() {
    this.subs.forEach((s) => s.unsubscribe());

    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

}
