import { Injector, LOCALE_ID, NgModule, inject, isDevMode } from '@angular/core';
import { BrowserModule, DomSanitizer } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ThemeModule } from './@theme/theme.module';
import { NbChatModule, NbDatepickerModule, NbDialogModule, NbIconLibraries, NbMenuModule, NbSidebarModule, NbToastrModule, NbWindowModule } from '@nebular/theme';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule, HttpRequest, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateLoader, TranslateModule, TranslateService } from '@ngx-translate/core';
import { StoreModule } from '@ngrx/store';
import { ROOT_REDUCERS, metaReducers } from './@reducers';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { StoreRouterConnectingModule } from '@ngrx/router-store';
import { APP_BASE_HREF, HashLocationStrategy, LocationStrategy } from '@angular/common';
import { NB_AUTH_TOKEN_INTERCEPTOR_FILTER, NbTokenStorage } from '@nebular/auth';
import { ResponseInterceptor } from './@interceptors/response.interceptor';
// import { NbCustomTokenStorage } from './@auth/token/nb/customtokenstorage';
import { CoreModule } from './@core/core.module';
import { I18nModule } from './@i18n/i18n.module';
import { CUSTOM_DATE_FORMAT, CUSTOM_DATE_FORMATS } from './@core/const/custom-mat-date-format';
import { FORMLY_CONFIG, FormlyModule } from '@ngx-formly/core';
import { ReactiveFormsModule } from '@angular/forms';
import { FormlyMaterialModule } from '@ngx-formly/material';
import { FormlyMatDatepickerModule } from '@ngx-formly/material/datepicker';
import { AutoCompleteExtension, registerTranslateExtension } from './@core/ngx-formly/formly-translate.extension';
import { maxFiles, minFiles, allowedFileExtensions, maxFilenameLength, totalFileSize, filenameForbiddenCharacters } from './@core/ngx-formly/validations/file-validators';
import { typeValidationMessage, minLengthValidationMessage, maxLengthValidationMessage, minValidationMessage, maxValidationMessage, multipleOfValidationMessage, minItemsValidationMessage, maxItemsValidationMessage, constValidationMessage, IpValidatorMessage, exclusiveMinimumValidationMessage, exclusiveMaximumValidationMessage } from './@core/ngx-formly/validations/formly-validation-messages';
import { fieldMatchValidator, IpValidator, dateFutureValidator, timeFutureValidator, autocompleteValidator } from './@core/ngx-formly/validations/formly-validators';
import { FileTypeValidationMessages } from './@core/ngx-formly/validations/file-type-validation-messages';
import { environment } from '../environments/environment';
import { ArrayTypeComponent } from './@core/ngx-formly/components/array.component';
import { AutocompleteTypeComponent } from './@core/ngx-formly/components/autocomplete-type.component';
import { AutocompleteemTypeComponent } from './@core/ngx-formly/components/autocompleteem-type.component';
import { FileTypeComponent } from './@core/ngx-formly/components/file/file-type/file-type.component';
import { MultiSchemaTypeComponent } from './@core/ngx-formly/components/multischema.component';
import { NbInputFormlyComponent } from './@core/ngx-formly/components/nb-input-formly';
import { NullTypeComponent } from './@core/ngx-formly/components/null.component';
import { ObjectTypeComponent } from './@core/ngx-formly/components/object.component';
import { MatIconRegistry } from '@angular/material/icon';
import { TimepickerTypeComponent } from './@core/ngx-formly/components/timepicker-type.component';
import { FormlyDateComponent } from './@core/ngx-formly/components/formly-date/formly-date.component';
import { NbDateFnsDateModule } from '@nebular/date-fns';
import { NgxEchartsModule } from 'ngx-echarts';
import { JwtHelperService, JwtModule } from '@auth0/angular-jwt';
import { StorageService } from './@core/utils.ts';
import { CustomJwtInterceptor } from './@interceptors/jwt.interceptor';
import { NgxPermissionsModule } from 'ngx-permissions';
import { TreeviewModule } from './@external-lib-override/treeview/treeview.module';
import { Configuration, ConfigurationParameters, ApiModule } from 'src/open-api';
// import { TreeviewModule } from '@samotics/ngx-treeview';

export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

function filterInterceptorRequest(req: HttpRequest<any>): boolean {
  return ['/api/v1/auth/login', '/api/v1/auth/refresh-token']
    .some(url => req.url.includes(url));
}

export function apiConfigFactory(): Configuration {
  const params: ConfigurationParameters = {
    basePath: environment.apiBaseUrlWithoutApiVersion,
  };
  return new Configuration(params);
}


// export function tokenGetter() {
//   return localStorage.getItem('access_token');
// }

// Factory function to get the token
// export function tokenGetter(storageService: StorageService): string | null {
//   return storageService.secureStorage.getItem('token'); // Adjust key as needed
// }

@NgModule({
  declarations: [
    AppComponent
  ],
  bootstrap: [AppComponent],
  imports: [I18nModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ApiModule.forRoot(apiConfigFactory),
    NgxPermissionsModule.forRoot(),
    CoreModule.forRoot(),
    NbChatModule.forRoot({
      messageGoogleMapKey: 'AIzaSyA_wNuCzia92MAmdLRzmqitRGvCF7wCZPY',
    }),
    ThemeModule.forRoot(),
    NbMenuModule.forRoot(),
    NbSidebarModule.forRoot(),
    NbDialogModule.forRoot(),
    NbWindowModule.forRoot(),
    NbToastrModule.forRoot(),
    NbDatepickerModule.forRoot(),
    NbDateFnsDateModule.forRoot({
      format: 'dd-MM-yyyy', // Set the global date format
      // parseOptions: { useAdditionalWeekYearTokens: true, useAdditionalDayOfYearTokens: true },
      // formatOptions: { useAdditionalWeekYearTokens: true, useAdditionalDayOfYearTokens: true },
      
      parseOptions: { useAdditionalWeekYearTokens: true, useAdditionalDayOfYearTokens: true },
      formatOptions: { useAdditionalWeekYearTokens: true, useAdditionalDayOfYearTokens: true, timeZone: 'UTC' },
      // parseOptions: { locale: arEG },
      // formatOptions: { locale: arEG },
    }),
    TranslateModule.forRoot({
      defaultLanguage: 'en-US',
      isolate: false,
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    }),
    
    TreeviewModule.forRoot(),

    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          const injector = inject(StorageService);
          return injector.secureStorage.getItem('access_token');
        },
        disallowedRoutes: [], // Optional: Specify any routes to exclude
      },
    }),

    StoreModule.forRoot(ROOT_REDUCERS, {
      metaReducers,
      runtimeChecks: {
        strictStateImmutability: true,
        strictActionImmutability: true,
        strictStateSerializability: true,
        strictActionSerializability: true
      }
    }),
    StoreDevtoolsModule.instrument({
      name: 'PNET CUSTOM'
    }),
    StoreRouterConnectingModule.forRoot(),
    // NgbModule,
    FormlyModule.forRoot({
      validators: [
        { name: 'fieldMatch', validation: fieldMatchValidator },
        { name: 'ip', validation: IpValidator },
        {
          name: 'date-future',
          validation: dateFutureValidator,
          options: { days: 2 },
        },
        {
          name: 'time-future',
          validation: timeFutureValidator,
          options: { days: 2 },
        },
        {
          name: 'max-files',
          validation: maxFiles,
          options: { maxFiles: 2 },
        },
        {
          name: 'min-files',
          validation: minFiles,
          options: { minFiles: 0 },
        },
        {
          name: 'total-file-size',
          validation: totalFileSize,
          options: { maxTotalFilesize: 2 },
        },
        {
          name: 'allowed-file-extensions',
          validation: allowedFileExtensions,
          options: { allowedFileExtensions: ['pdf', 'png', 'jpg'] },
        },
        {
          name: 'filename-forbidden-characters',
          validation: filenameForbiddenCharacters,
          options: { forbiddenCharacters: ['c', 'k'] },
        },
        {
          name: 'max-filename-length',
          validation: maxFilenameLength,
          options: { maxFilenameLength: 20 },
        },
        // {
        //   name: 'max-files',
        //   validation: FileListValidators.maxFiles(3),
        //   options: { days: 2 },
        // },
        {
          name: 'autocomplete-validation',
          validation: autocompleteValidator
        },
      ],
      validationMessages: [
        // new FileTypeValidationMessages(APP_LOCALE_ID).validationMessages,
        // { name: 'required', message: 'This field is required' },
        ...new FileTypeValidationMessages(environment.defaultLanguage).validationMessages,
        { name: 'type', message: typeValidationMessage },
        { name: 'ip', message: IpValidatorMessage },
        { name: 'minLength', message: minLengthValidationMessage },
        { name: 'maxLength', message: maxLengthValidationMessage },
        { name: 'min', message: minValidationMessage },
        { name: 'max', message: maxValidationMessage },
        { name: 'multipleOf', message: multipleOfValidationMessage },
        { name: 'exclusiveMinimum', message: exclusiveMinimumValidationMessage },
        { name: 'exclusiveMaximum', message: exclusiveMaximumValidationMessage },
        { name: 'minItems', message: minItemsValidationMessage },
        { name: 'maxItems', message: maxItemsValidationMessage },
        { name: 'uniqueItems', message: 'should NOT have duplicate items' },
        { name: 'const', message: constValidationMessage },
        { name: 'enum', message: `must be equal to one of the allowed values` },
      ],
      types: [
        { name: 'null', component: NullTypeComponent, wrappers: ['form-field'] },
        { name: 'array', component: ArrayTypeComponent },
        { name: 'object', component: ObjectTypeComponent },
        { name: 'multischema', component: MultiSchemaTypeComponent },
        {
          name: 'customdatepicker',
          component: FormlyDateComponent,
          wrappers: ['form-field'],
        },
        {
          name: 'autocomplete',
          component: AutocompleteTypeComponent,
          wrappers: ['form-field'],
        },
        {
          name: 'autocompleteem',
          component: AutocompleteemTypeComponent,
          wrappers: ['form-field'],
        },
        {
          name: 'timepicker',
          component: TimepickerTypeComponent,
          wrappers: ['form-field'],
        },
        {
          name: 'nbinput',
          component: NbInputFormlyComponent,
          wrappers: ['form-field'],
        },
        { name: 'file', component: FileTypeComponent },

      ],
      extensions: [
        {
          name: 'autoFilter',
          extension: new AutoCompleteExtension(),//this can do custom logic like caling api based on url
        },
        // {
        //   name: 'TranspileStrCodVimbo',
        //   extension: new TranspileStringToCode()
        // }
      ]
    }),

    NgxEchartsModule.forRoot({
      echarts: () => import('echarts')
    }),
    
    ReactiveFormsModule,
    FormlyMaterialModule,
    FormlyMatDatepickerModule,
    
  ],

  providers: [
    { provide: LocationStrategy, useClass: HashLocationStrategy },
    // { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    // { provide: HTTP_INTERCEPTORS, useClass: JWTInterceptor, multi: true },
    // { provide: NB_AUTH_TOKEN_INTERCEPTOR_FILTER, useValue: filterInterceptorRequest },

    { provide: HTTP_INTERCEPTORS, useClass: ResponseInterceptor, multi: true },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: CustomJwtInterceptor,
      multi: true,
    },
    
    { provide: APP_BASE_HREF, useValue: "/" },
    // { provide: NbTokenStorage, useClass: NbCustomTokenStorage },
   
    //         //below erlated to mat nad moment
    //         Date formats (dd/MM/yyyy for en-GB).
    // Calendar starting day (Monday for en-GB).

    // { provide: MAT_DATE_FORMATS, useValue: CUSTOM_DATE_FORMAT },
    // { provide: DateAdapter, useClass: MomentDateAdapter },

    // { provide: MAT_MOMENT_DATE_ADAPTER_OPTIONS, useValue: { useUtc: true } },
    { provide: FORMLY_CONFIG, multi: true, useFactory: registerTranslateExtension, deps: [TranslateService] },
    // { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    // { provide: DateAdapter, useClass: CustomDateAdapter },

    provideHttpClient(withInterceptorsFromDi()),
  ]
})
export class AppModule {
  constructor(
    matIconRegistry: MatIconRegistry,
    private iconLibraries: NbIconLibraries,
    sanitizer: DomSanitizer) {
    matIconRegistry.addSvgIconInNamespace('fileType', 'fileDrop', sanitizer.bypassSecurityTrustResourceUrl('assets/svgs/file_copy-24px.svg'));
    matIconRegistry.addSvgIconInNamespace('fileType', 'file', sanitizer.bypassSecurityTrustResourceUrl('assets/svgs/cloud_done-24px.svg'));
    matIconRegistry.addSvgIconInNamespace('fileType', 'fileUpload', sanitizer.bypassSecurityTrustResourceUrl('assets/svgs/cloud_upload-24px.svg'));
    matIconRegistry.addSvgIconInNamespace('fileType', 'fileRemove', sanitizer.bypassSecurityTrustResourceUrl('assets/svgs/clear-24px.svg'));
    // Register Font Awesome as a custom pack
    //    this.iconLibraries.registerFontPack('font-awesome', { packClass: 'fa', iconClassPrefix: 'fa' });
  }
}
