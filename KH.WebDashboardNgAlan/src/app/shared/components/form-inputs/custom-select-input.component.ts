import { Component, Input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { FieldInvalidPipe, FieldErrorPipe } from '../../pipes';
import { I18nPipe } from '@delon/theme';

export interface SelectOption {
  value: any;
  label: string;
}

@Component({
  selector: 'app-custom-select-input',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzFormModule, NzSelectModule, FieldInvalidPipe, FieldErrorPipe, I18nPipe],
  template: `
    <div class="form-field">
      <label class="form-label" [class.required]="required">
        {{ label | i18n }}
      </label>
      <nz-form-control 
        [nzValidateStatus]="(control | fieldInvalid) ? 'error' : ''"
        [nzErrorTip]="control | fieldError:fieldName">
        <nz-select 
          [formControl]="control"
          class="rounded-select"
          [nzPlaceHolder]="(placeholder || '') | i18n"
          [nzAllowClear]="allowClear"
          [nzDisabled]="disabled">
          @for (option of options; track option.value) {
            <nz-option 
              [nzValue]="option.value" 
              [nzLabel]="option.label">
            </nz-option>
          }
        </nz-select>
      </nz-form-control>
    </div>
  `,
  styles: []
})
export class CustomSelectInputComponent {
  @Input() control!: FormControl;
  @Input() label!: string;
  @Input() placeholder?: string;
  @Input() fieldName!: string;
  @Input() required = false;
  @Input() options: SelectOption[] = [];
  @Input() allowClear = true;
  @Input() disabled = false;
} 