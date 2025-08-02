import { Component, Input, forwardRef } from '@angular/core';
import { FormControl, ControlValueAccessor, NG_VALUE_ACCESSOR, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { FieldInvalidPipe, FieldErrorPipe } from '../../pipes';
import { I18nPipe } from '@delon/theme';

@Component({
  selector: 'app-custom-text-input',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzFormModule, NzInputModule, FieldInvalidPipe, FieldErrorPipe, I18nPipe],
  template: `
    <div class="form-field">
      <label class="form-label" [class.required]="required">
        {{ label | i18n }}
      </label>
      <nz-form-control 
        [nzValidateStatus]="(control | fieldInvalid) ? 'error' : ''"
        [nzErrorTip]="control | fieldError:fieldName">
        <input 
          nz-input 
          [formControl]="control"
          [placeholder]="(placeholder || '') | i18n"
          class="rounded-input"
          [dir]="direction"
          [type]="inputType" />
      </nz-form-control>
    </div>
  `,
  styles: []
})
export class CustomTextInputComponent {
  @Input() control!: FormControl;
  @Input() label!: string;
  @Input() placeholder?: string;
  @Input() fieldName!: string;
  @Input() required = false;
  @Input() direction: 'ltr' | 'rtl' = 'ltr';
  @Input() inputType: 'text' | 'email' | 'password' | 'number' = 'text';
} 