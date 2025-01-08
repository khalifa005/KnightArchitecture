import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ToastNotificationService } from '../../../../@core/utils.ts/toast-notification.service';
import { Logger } from '../../../../@core/utils.ts/logger.service';

@Component({
  selector: 'app-custom-textarea',
  templateUrl: './custom-textarea.component.html',
  styleUrls: ['./custom-textarea.component.css']
})
export class CustomTextareaComponent implements OnInit  , OnDestroy{
  private log = new Logger(CustomTextareaComponent.name);

  @Output() selectedItemChange = new EventEmitter<any>();
  @Input() selectedItem: any;

  @Input() readonly = false;
  @Input() isRequired = false;
  @Input() disabled = false;
  @Input() label: string = "";
  @Input() placeHolder: string = "";

  @Input() formcontrol: FormControl;
  private subs: Subscription[] = [];


  constructor(
    private toastNotificationService:ToastNotificationService) {

  }

  ngOnInit() {
  }



  onItemChanged() {
    this.selectedItemChange.emit(this.selectedItem);
  }

  ngOnDestroy() {
    this.subs.forEach((s) => s.unsubscribe());
  }


}
