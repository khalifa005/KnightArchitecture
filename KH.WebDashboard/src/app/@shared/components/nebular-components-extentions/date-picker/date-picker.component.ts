import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { Logger } from '../../../../@core/utils.ts/logger.service';
import { ToastNotificationService } from '../../../../@core/utils.ts/toast-notification.service';

@Component({
  selector: 'ngx-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.scss']
})
export class DatePickerComponent implements OnInit , OnDestroy{
  private log = new Logger(DatePickerComponent.name);

  @Output() selectedItemChange = new EventEmitter<any>();
  @Input() selectedItem: any;
  @Input() readonly = false;
  @Input() isRequired = false;
  @Input() disabled = false;
  @Input() label: string = "";
  @Input() className: string = "";
  @Input() placeHolder: string = "";
  // @Input() messages = validationMessages;
  @Input() min: Date;
  @Input() max: Date;
  // errors: ValidationErrors

  @Input() formcontrol: FormControl;
  private subs: Subscription[] = [];


  constructor(
    private toastNotificationService:ToastNotificationService) {

  }

  ngOnInit() {


  }

  onItemChanged(value:any) {
  this.log.info(value);

    this.selectedItemChange.emit(this.selectedItem);
  }

  ngOnDestroy() {
    this.subs.forEach((s) => s.unsubscribe());
  }


}
