import { first } from 'rxjs/operators';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl, ValidationErrors } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ToastNotificationService } from '../../../../@core/utils.ts/toast-notification.service';
import { Logger } from '../../../../@core/utils.ts/logger.service';

@Component({
  selector: 'ngx-custom-input',
  templateUrl: './custom-input.component.html',
  styleUrls: ['./custom-input.component.scss']
})
export class CustomInputComponent implements OnInit  , OnDestroy{
  private log = new Logger(CustomInputComponent.name);

  @Output() selectedItemChange = new EventEmitter<any>();
  @Input() selectedItem: any;
  @Input() readonly = false;
  @Input() isRequired = false;
  @Input() disabled = false;
  @Input() icon: string = "";
  @Input() label: string = "";
  @Input() placeHolder: string = "";
  // @Input() regexPattern: RegExp;
  @Input() regexPattern: RegExp | string = ""; // Default: Non-numeric values only


  @Input() formcontrol: FormControl;
  private subs: Subscription[] = [];


  constructor(
    private toastNotificationService:ToastNotificationService) {

  }

  ngOnInit() {

      // Ensure regexPattern is a RegExp object
      if (typeof this.regexPattern === 'string') {
        this.regexPattern = new RegExp(this.regexPattern);
      }


if(this.selectedItem){
  this.log.info(this.selectedItem);
}

  }

  onItemChanged(value:any) {
    this.selectedItemChange.emit(this.selectedItem);
  }

  validateInput(value: string) {
    if (this.regexPattern instanceof RegExp && !this.regexPattern.test(value)) {
      this.selectedItem = value.replace(new RegExp(this.regexPattern, 'g'), ''); // Remove invalid characters
    } else {
      this.selectedItem = value;
    }
    this.selectedItemChange.emit(this.selectedItem);
  }

  preventInvalidInput(event: KeyboardEvent) {
    // Allow only characters that match the regex pattern
    if (this.regexPattern instanceof RegExp && !this.regexPattern.test(event.key)) {
      event.preventDefault();
    }
  }

  ngOnDestroy() {
    this.subs.forEach((s) => s.unsubscribe());
  }


}
