import {
  AfterViewInit,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subscription, Subject } from 'rxjs';
import { AppDefaultValues } from '../../../../@core/const/default-values';
import { LookupResponse } from '../../../../@core/models/base/response/lookup.model';
import { isNumber } from '../../../../@core/utils.ts/data-types.utilts/number-utils';
import { Logger } from '../../../../@core/utils.ts/logger.service';
import { ToastNotificationService } from '../../../../@core/utils.ts/toast-notification.service';
import { I18nService } from '../../../../@i18n/services/i18n.service';
import { LanguageEnum } from '../../../../@i18n/models/language.enum';

@Component({
  selector: 'app-nb-auto-complete',
  templateUrl: './nb-auto-complete.component.html',
  styleUrls: ['./nb-auto-complete.component.scss'],
})
export class NbAutoCompleteComponent
  implements OnInit, AfterViewInit, OnChanges, OnDestroy
{
  private log = new Logger(NbAutoCompleteComponent.name);
  @ViewChild('inputRef') inputElementRef: ElementRef;

  @Output() selectedItemChanged: EventEmitter<number> = new EventEmitter();
  @Input() selectedItem: number;
  @Input() label: string = '';
  @Input() readonly = false;
  @Input() selectOptions: LookupResponse[] = [];
  @Input() placeHolder: string = 'enter-value';
  @Input() isRequired = false;
  @Input() disabled = false;
  @Input() formcontrol: FormControl = new FormControl();

  private subs: Subscription[] = [];
  private destroy$: Subject<void> = new Subject<void>();

  filteredSelectOptions: LookupResponse[] = [];
  firstRender = true;

  constructor(private toastNotificationService: ToastNotificationService,private i18nService: I18nService) {
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.filteredSelectOptions = this.selectOptions;
    this.filter(this.formcontrol.value);
  }

  ngOnInit() {
    this.callWhenOnInit();
  }

  ngOnDestroy() {
    this.subs.forEach((s) => s.unsubscribe());
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngAfterViewInit(): void {
    if (this.firstRender && this.selectOptions) {
      if (this.formcontrol.value && this.formcontrol.value > AppDefaultValues.DropDownAllOption) {
        const initialValue = this.selectOptions.find(
          (x) => x.id == this.formcontrol.value
        );
        this.inputElementRef.nativeElement.value =
        this.i18nService.language == LanguageEnum.Ar
            ? initialValue?.nameAr
            : initialValue?.nameEn;
      } else {
        this.inputElementRef.nativeElement.value = '';
      }
      this.firstRender = false;
    }
  }

  callWhenOnInit() {
    this.formcontrol.valueChanges.subscribe((text) => {
      this.filter(text);
    });
  }

  /**
   * Filters options based on the current input value and active language.
   * @param value The input value to filter against.
   * @returns Filtered options.
   */
  private filter(value: any): LookupResponse[] {
    this.formcontrol.setErrors(null);

    if (this.selectOptions) {
      
      if (isNumber(value) && value > 0 && typeof value != 'string') {
      // const test = typeof value === 'string';

        this.filteredSelectOptions = this.selectOptions.filter(
          (item) => item.id ===  Number(value)
        );

        if (!this.firstRender && this.inputElementRef?.nativeElement) {
          const initialValue = this.selectOptions.find(
            (x) => x.id === Number(this.formcontrol.value)
          );

          if(initialValue === undefined || initialValue === null ){
            if (!this.firstRender && this.inputElementRef?.nativeElement) {
              this.inputElementRef.nativeElement.value = '';
              this.formcontrol.setErrors({ customError: true });
            }
            return this.filteredSelectOptions;
          }

          this.inputElementRef.nativeElement.value =
          this.i18nService.language == LanguageEnum.Ar
              ? initialValue?.nameAr
              : initialValue?.nameEn;
        }
        return this.filteredSelectOptions;
      }

      if (value === '' || value === AppDefaultValues.DropDownAllOption) {
        this.filteredSelectOptions = this.selectOptions;

        if (!this.firstRender && this.inputElementRef?.nativeElement) {
          this.inputElementRef.nativeElement.value = '';
          this.formcontrol.setErrors({ customError: true });
        }
        return this.filteredSelectOptions;
      }

      const filterKey = this.i18nService.language == LanguageEnum.Ar ? 'nameAr' : 'nameEn';
      this.filteredSelectOptions = value
        ? this.selectOptions.filter((option) =>
            option[filterKey].toLowerCase().includes(value.toLowerCase())
          )
        : this.selectOptions;

      this.formcontrol.setErrors({ customError: true });
      return this.filteredSelectOptions;
    }
    return [];
  }

  private filterx(value: any): LookupResponse[] {
    this.formcontrol.setErrors(null);
  
    if (!this.selectOptions) {
      return [];
    }
  
    const filterKey =
      this.i18nService.language == LanguageEnum.Ar ? 'nameAr' : 'nameEn';
  
    // Handle number input
    if (isNumber(value)) {
      const numberValue = Number(value);
  
      // Match both id and text containing the number
      this.filteredSelectOptions = this.selectOptions.filter(
        (item) =>
          item.id === numberValue || item[filterKey].includes(numberValue.toString())
      );
  
      if (!this.firstRender && this.inputElementRef?.nativeElement) {
        const initialValue = this.selectOptions.find(
          (x) => x.id === this.formcontrol.value
        );
        this.inputElementRef.nativeElement.value =
          this.i18nService.language == LanguageEnum.Ar
            ? initialValue?.nameAr
            : initialValue?.nameEn;
      }
  
      return this.filteredSelectOptions;
    }
  
    // Handle string input
    if (value === '' || value === AppDefaultValues.DropDownAllOption) {
      this.filteredSelectOptions = this.selectOptions;
  
      if (!this.firstRender && this.inputElementRef?.nativeElement) {
        this.inputElementRef.nativeElement.value = '';
        this.formcontrol.setErrors({ customError: true });
      }
  
      return this.filteredSelectOptions;
    }
  
    this.filteredSelectOptions = this.selectOptions.filter((option) =>
      option[filterKey].toLowerCase().includes(value.toLowerCase())
    );
  
    if (!this.filteredSelectOptions.length) {
      this.formcontrol.setErrors({ customError: true });
    }
  
    return this.filteredSelectOptions;
  }
  /**
   * Display function for the autocomplete input.
   * Dynamically selects the correct language for display.
   * @param option The option to display.
   * @returns The display value.
   */
  displayFn = (option: any): string => {
    if (
      option &&
      this.filteredSelectOptions &&
      isNumber(option) &&
      option > AppDefaultValues.DropDownAllOption
    ) {
      const selectedOption = this.filteredSelectOptions.find(
        (x) => x.id === option
      );
      return this.i18nService.language == LanguageEnum.Ar
        ? selectedOption?.nameAr || option.toString()
        : selectedOption?.nameEn || option.toString();
    }
    return option ? option.toString() : '';
  };

  onItemChanged() {
    this.selectedItemChanged.emit(this.selectedItem);
  }

}
