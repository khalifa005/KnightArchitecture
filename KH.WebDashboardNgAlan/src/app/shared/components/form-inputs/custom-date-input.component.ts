import { Component, Input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { FieldInvalidPipe, FieldErrorPipe } from '../../pipes';
import { I18nPipe } from '@delon/theme';

@Component({
  selector: 'app-custom-date-input',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzFormModule, NzDatePickerModule, FieldInvalidPipe, FieldErrorPipe, I18nPipe],
  template: `
    <div class="form-field">
      <label class="form-label" [class.required]="required">
        {{ label | i18n }}
      </label>
      <nz-form-control 
        [nzValidateStatus]="(control | fieldInvalid) ? 'error' : ''"
        [nzErrorTip]="control | fieldError:fieldName">
        <nz-date-picker 
          [formControl]="control"
          class="rounded-date-picker"
          [nzPlaceHolder]="(placeholder || '') | i18n"
          [nzFormat]="dateFormat"
          [nzShowTime]="showTime">
        </nz-date-picker>
      </nz-form-control>
    </div>
  `,
  styles: []
})
export class CustomDateInputComponent {
  @Input() control!: FormControl;
  @Input() label!: string;
  @Input() placeholder?: string;
  @Input() fieldName!: string;
  @Input() required = false;
  @Input() dateFormat = 'yyyy-MM-dd';
  @Input() showTime = false;
} 