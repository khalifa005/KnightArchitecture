import { Directive , TemplateRef, ViewChild} from "@angular/core";
import { APIDefinition, Columns, Config, DefaultConfig } from 'ngx-easy-table';
import { TranslateService } from "@ngx-translate/core";
import { NbWindowControlButtonsConfig } from "@nebular/theme";
import { LookupResponse } from "@app/@core/models/base/response/lookup.model";
import { LanguageEnum } from "@app/@i18n/models/language.enum";

@Directive()


export abstract class CommonFilterComp{
  @ViewChild('searchByReeasonId')
  searchByReeasonIdRef: TemplateRef<any>;
  @ViewChild('searchByReeasonNameEn')
  searchByReeasonNameEnRef: TemplateRef<any>;
  @ViewChild('searchByReeasonNameAr')
  searchByReeasonNameArRef: TemplateRef<any>;
  @ViewChild('searchByReeasonDescription')
  searchByReeasonDescriptionRef: TemplateRef<any>;
  @ViewChild('searchByReeasonIsDelete')
  searchByReeasonIsDeleteRef: TemplateRef<any>;
  @ViewChild('table') table: APIDefinition;

  @ViewChild('insuranceType')
  InsuranceTypeRef: TemplateRef<any>;
  @ViewChild('hasCancellationRequestDependency')
  hasCancellationRequestDependencyRef: TemplateRef<any>;
  @ViewChild('hasPolicyDataDependency')
  hasPolicyDataDependencyRef: TemplateRef<any>;
  @ViewChild('showForCustomerPortal')
  showForCustomerPortalRef: TemplateRef<any>;


  compasearchByReeasonIdny: string ='';
  searchByReeasonNameEnVal:string ='';
  searchByReeasonNameArVal:string ='';
  searchByReeasonDescriptionVal:string='';
  selectedIsDelete = new Set<string>(['true','false' ]);

  insuranceTypeVal:string='';
  hasCancellationRequestDependencyVal:string='';
  hasPolicyDataDependencyVal:string='';
  selectedIsShowPoratal = new Set<string>(['true','false' ]);

  public configuration: Config;
  
  public columns: Columns[] = [
    { key: 'Id', title: 'Id' },
    { key: 'NameEn', title: 'NameEn' },
    { key: 'NameAr', title: 'NameAr' },
    { key: 'Description', title: 'Description' },
    { key: 'IsDeleted', title: 'IsDeleted' },
    // { key: 'age', title: 'Age', width: '15%', orderEnabled: true, searchEnabled: false },
  ];

  data: LookupResponse[] =[] ;
  holdData: LookupResponse[] =[] ;

  constructor(public localizationService: TranslateService){}

 get configWindowDialog() : NbWindowControlButtonsConfig{
  return   {
    minimize: false,
    maximize:false,
    fullScreen: false,
    close: true
  };
 }

 get configWindowDrop() {
  return {
   hasBackdrop: true, closeOnEsc: true
  }
 }

  filter(field: string, event: Event | string): void {
    const value = typeof event === 'string' ? event : (event.target as HTMLInputElement).value.trim();
    const data = [...this.holdData]
    switch(field){
      case 'compasearchByReeasonIdny':
        this.compasearchByReeasonIdny = (value.trim());
        break;
      case 'searchByReeasonNameEn':
        this.searchByReeasonNameEnVal = (value.trim());
        break
      case "searchByReeasonNameAr":
        this.searchByReeasonNameArVal = (value.trim());
        break;
      case "searchByReeasonDescription":
        this.searchByReeasonDescriptionVal = (value.trim());
        break;
      case "IsDeleted":
        this.selectedIsDelete.has(value)
        ? this.selectedIsDelete.delete(value)
        : this.selectedIsDelete.add(value);
        break;

      case "isShowPoratal":
        this.selectedIsShowPoratal.has(value)
        ? this.selectedIsShowPoratal.delete(value)
        : this.selectedIsShowPoratal.add(value);
        break;
      default:
        return;
    }


    this.data = data.filter((el:any,index:number)=>{ // need the index here becuase the boolean

      return    value?
      this.selectedIsDelete.has(el.IsDeleted.toString())&&
      (this.compasearchByReeasonIdny.length >0 ? el.Id === parseInt(this.compasearchByReeasonIdny)
      :this.searchByReeasonNameEnVal.length >0 ? el.NameEn.includes(this.searchByReeasonNameEnVal)
      :this.searchByReeasonNameArVal.length >0 ? el.NameAr.includes(this.searchByReeasonNameArVal)
      :this.searchByReeasonDescriptionVal.length >0 ? el.Description.includes(this.searchByReeasonDescriptionVal)
      :true):true
     })
  }

  initalTable() {
    this.configuration = { ...DefaultConfig };
    this.configuration.tableLayout.hover = !this.configuration.tableLayout.hover;
    this.configuration.tableLayout.striped = !this.configuration.tableLayout.striped;
    this.configuration.tableLayout.borderless= true;
    this.configuration.tableLayout.style = 'big';
    this.configuration.isLoading = true;
    this.configuration.serverPagination = true;
    this.configuration.threeWaySort = true;
    // this.configuration.resizeColumn = true;
    this.configuration.rowReorder = true;
    this.configuration.columnReorder = true;
    this.configuration.fixedColumnWidth = false;
    this.configuration = { ...DefaultConfig };


  }

  getTableHeaderName() {
    let isEngLanguage = this.localizationService.currentLang === LanguageEnum.En;
    this.columns = [
    { key: 'Id', title: isEngLanguage ? 'Id' : 'الرقم التعريفي'  , headerActionTemplate: this.searchByReeasonIdRef},
    { key: 'NameEn', title: isEngLanguage ? 'Name english' : 'الاسم الانجليزي'   , headerActionTemplate: this.searchByReeasonNameEnRef},
    { key: 'NameAr', title: isEngLanguage ? 'Name arabic' : 'الاسم لعربي'  , headerActionTemplate: this.searchByReeasonNameArRef},
    { key: 'Description', title: isEngLanguage ? 'Description' : 'التفاصيل' , headerActionTemplate: this.searchByReeasonDescriptionRef},
    { key: 'IsDeleted', title: isEngLanguage ? 'Is deleted' : 'هل تم مسة'  , headerActionTemplate: this.searchByReeasonIsDeleteRef},
    {key:"Option",title:isEngLanguage?"Option":"خيارات"},
    ];
  }
}
