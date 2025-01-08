import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FieldType } from '@ngx-formly/material';
import { FieldTypeConfig } from '@ngx-formly/core';
import { Subject } from 'rxjs';
import { TimepickerComponent } from 'ngx-bootstrap/timepicker';
import { validateTimeString } from '../validations/time-validators';

@Component({
  selector: 'formly-test-type',
  styleUrls: ['./autocomplete-type.component.scss'],
  template: `
 <div class="form-group col-md-6">
     <label [ngClass]="{'text-danger': isFocus && (formControl.invalid) || (formControl.dirty ) }">
    {{ props.placeholder }}
    <span *ngIf="props.required" class="required-asterisk">*</span>
  </label>

    <timepicker class="formly-test" 
  
    #timepicker 
    [ngModel]="formControl.value" 
    (focus)="onFocus()"
    (click)="onClick()" 
    (ngModelChange)="onTimeChange($event)"></timepicker>
    
    <div *ngIf="showError">
      <small class="text-danger">{{ errorMessage }}</small>
    </div>
    
    <div  *ngIf="( (formControl.invalid && isFocus ) || (formControl.dirty && isFocus ))">
      <p class="text-danger">please select a value </p>
    </div>
  </div>
  `,
})
export class TimepickerTypeComponent extends FieldType<FieldTypeConfig> implements OnInit, OnDestroy {
  @ViewChild('timepicker', { static: true }) timepicker: TimepickerComponent;
  private destroy$: Subject<void> = new Subject<void>();
  timePickerValue: Date;
  errorMessage: string = 'please select a value';
  isFocus: boolean = false;

  ngOnInit() {
    // this.timepicker.onTouched().
    if (this.formControl.value) {
      this.timePickerValue = this.parseTime(this.formControl.value);
      this.formControl.setValue(this.timePickerValue);
    }
  }
  
  onFocus() {
    // let kd = this.field
   this.isFocus = true;
  }
  
  onClick() {
   this.isFocus = true;
  }

  parseTime(timeString: string): Date {
    const [hours, minutes, seconds] = timeString.split(':').map(Number);
    const date = new Date();
    date.setHours(hours, minutes, seconds, 0);
    return date;
  }

  onTimeChange(value: any) {
    this.isFocus = true;
    if (value) {
      const timeString = this.formatTime(value);
      if (validateTimeString(timeString)) {
        this.formControl.setValue(timeString);
        this.errorMessage = '';
      } else {
        this.formControl.setValue(null);

        this.errorMessage = 'Invalid time format';
      }
    } else {
      this.formControl.setValue(null);

      this.errorMessage = 'Invalid time selected';
    }

  }
  formatTime(date: Date): string {
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    const seconds = date.getSeconds().toString().padStart(2, '0');
    return `${hours}:${minutes}:${seconds}`;
  }


  // validateTime(timeString: string): boolean {
  //   const regex = /^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$/;
  //   return regex.test(timeString);
  // }

  override ngOnDestroy() {
    // Custom cleanup logic for this child component
    this.destroy$.next();
    this.destroy$.complete();

    // Call the parent class's ngOnDestroy method
    super.ngOnDestroy();
  }
}
