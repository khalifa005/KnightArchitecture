import { Component, Input } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { I18nPipe } from '@delon/theme';

@Component({
  selector: 'app-custom-switch-input',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NzFormModule, NzSwitchModule, I18nPipe],
  template: `
    <div class="form-field">
      <label class="form-label">
        {{ label | i18n }}
      </label>
      <nz-form-control>
        <nz-switch 
          [formControl]="control"
          class="status-switch">
        </nz-switch>
      </nz-form-control>
    </div>
  `,
  styles: []
})
export class CustomSwitchInputComponent {
  @Input() control!: FormControl;
  @Input() label!: string;
} 