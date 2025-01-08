import { CdkContextMenuTrigger, CdkMenu, CdkMenuItem, CdkMenuTrigger } from '@angular/cdk/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTreeModule } from '@angular/material/tree';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatListModule } from '@angular/material/list';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
// import { FormlyModule } from '@ngx-formly/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTabsModule } from '@angular/material/tabs';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatNativeDateModule } from '@angular/material/core';
// import { FormlyMatDatepickerModule } from '@ngx-formly/material/datepicker';
import { PortalModule } from '@angular/cdk/portal';
import { MatCardModule } from '@angular/material/card';

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableModule } from 'ngx-easy-table';
import { ThemeModule } from '../@theme/theme.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { DyTableComponent } from './components/dy-table/dy-table.component';
import { NbAccordionModule, NbButtonModule, NbInputModule, NbSelectModule, NbIconModule, NbCheckboxModule, NbAutocompleteModule, NbCardModule, NbListModule, NbPopoverModule, NbChatModule } from '@nebular/theme';
import { AddNoteFormComponent } from './components/forms/add-note-form/add-note-form.component';
import { DyMatMenuComponent } from './components/dy-mat-menu/dy-mat-menu.component';
import { IdCheckerPipe } from './pipes/id-checker.pipe';
import { NestedFieldsPipe } from './pipes/nested-table-fields.pipe';
import { NumberCounterComponent } from './components/display/number-counter/number-counter.component';
import { DynamicSvgComponent } from './components/display/dynamic-svg/dynamic-svg.component';
import { NewsPostPlaceholderComponent } from './components/display/news-post-placeholder/news-post-placeholder.component';
import { CustomCardComponent } from './components/display/custom-card/custom-card.component';
import { NgxEchartsModule } from 'ngx-echarts';
import { EchartsPieComponent } from './components/display/echarts/echarts-pie.component';
import { LoadingSpinnerComponent } from './components/display/loading-spinner/loading-spinner.component';
import { CustomInputComponent } from './components/nebular-components-extentions/custom-input/custom-input.component';
import { CustomTextareaComponent } from './components/nebular-components-extentions/custom-textarea/custom-textarea.component';
import { DatePickerComponent } from './components/nebular-components-extentions/date-picker/date-picker.component';
import { NbAutoCompleteComponent } from './components/nebular-components-extentions/nb-auto-complete/nb-auto-complete.component';
import { TranslatorPipe } from './pipes/translator.pipe';
import { CustomSingleSelectDropdownComponent } from './components/nebular-components-extentions/custom-single-select/custom-single-select-dropdown.component';
import { CustomMultiSelectDropdownComponent } from './components/nebular-components-extentions/custom-multi-select/custom-multi-select-dropdown.component';
import { TimeDropdownComponent } from './components/nebular-components-extentions/time-dropdown/time-dropdown.component';
import { FileUploaderComponent } from './components/files/file-uploader/file-uploader.component';
import { FilePreviewComponent } from './components/files/file-preview/file-preview.component';
import { DownloaderComponent } from './components/files/downloader/downloader.component';
import { CustomYesNoSelectComponent } from './components/nebular-components-extentions/custom-yes-no-select/custom-yes-no-select.component';

const NB_MODULES : any= [
  ReactiveFormsModule,
  FormsModule,
  ThemeModule,
  TableModule,
  NbAutocompleteModule,
  NbAccordionModule,
  NbChatModule,
    NbButtonModule,
    FormsModule,
    ReactiveFormsModule,
    NbListModule,
    NbInputModule,
    NbSelectModule,
    NbCardModule,
    NbIconModule,
    NbPopoverModule,
    NbCheckboxModule,
    NgxEchartsModule,
];

const Mat_MODULES = [
  CdkContextMenuTrigger,
  CdkMenu,
  CdkMenuItem,
  // NgxMaterialTimepickerModule,
  MatProgressSpinnerModule,
  // DragDropModule,

  // ScrollingModule,

  MatTreeModule,
  MatGridListModule,
  MatListModule,
  MatProgressBarModule,
  MatTooltipModule,
  MatFormFieldModule,
  MatTabsModule,
  MatPaginatorModule,
  MatButtonModule,
  MatMenuModule,
  MatIconModule,
  MatInputModule,
  MatAutocompleteModule,
  MatNativeDateModule,
  PortalModule,
  MatCardModule,
  CdkMenuItem,
  CdkMenuTrigger,
  CdkMenu,
];


const COMPONENTS : any = [
  DyTableComponent,
  AddNoteFormComponent,
  DyMatMenuComponent,
  CustomYesNoSelectComponent,
  CustomInputComponent,
  CustomTextareaComponent,
  DatePickerComponent,
  NbAutoCompleteComponent,
  
  NumberCounterComponent,
  DynamicSvgComponent,
  NewsPostPlaceholderComponent,
  CustomCardComponent,
  EchartsPieComponent,
  LoadingSpinnerComponent,
  CustomSingleSelectDropdownComponent,
  CustomMultiSelectDropdownComponent,
  TimeDropdownComponent,
  FilePreviewComponent,
  FileUploaderComponent,
  DownloaderComponent,
];

const PIPES = [
  IdCheckerPipe,
  NestedFieldsPipe,
  TranslatorPipe,
];


@NgModule({
  exports: [CommonModule, ...NB_MODULES, ...PIPES, ...COMPONENTS, ...Mat_MODULES],
  declarations: [...COMPONENTS, ...PIPES],
  imports: [
    CommonModule,
    
   TranslateModule,
    ...NB_MODULES,
    ...Mat_MODULES,
    

  ]
})
export class SharedModule { }
