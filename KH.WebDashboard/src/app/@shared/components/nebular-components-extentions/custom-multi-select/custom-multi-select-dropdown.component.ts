import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AppDefaultValues } from '../../../../@core/const/default-values';
import { LookupResponse } from '../../../../@core/models/base/response/lookup.model';
import { Logger } from '../../../../@core/utils.ts/logger.service';
import { ToastNotificationService } from '../../../../@core/utils.ts/toast-notification.service';

@Component({
  selector: 'app-custom-multi-select-dropdown',
  templateUrl: './custom-multi-select-dropdown.component.html',
  styleUrls: ['./custom-multi-select-dropdown.component.scss']
})
export class CustomMultiSelectDropdownComponent implements OnInit , OnDestroy{
  private log = new Logger(CustomMultiSelectDropdownComponent.name);

  @Output() selectedItemChanged: EventEmitter<any[]> = new EventEmitter();
  @Input() selectedItems: any[] = [];
  @Input() label: string = "";
  @Input() placeholder: string = "";
  @Input() readonly = false;
  @Input() selectOptions: LookupResponse[];
  @Input() isRequired = false;
  @Input() isMultiple = true;
  @Input() disabled = false;
  @Input() formcontrol: FormControl;
  private subs: Subscription[] = [];


  dropDownAllOption = new LookupResponse(AppDefaultValues.DropDownAllOption,
    AppDefaultValues.DropDownAllOptionAr,
     AppDefaultValues.DropDownAllOptionEn);

  constructor(
    private toastNotificationService:ToastNotificationService) {

  }

  ngOnInit() {

    if(this.isMultiple){
      this.selectedItems = this.selectOptions.map(x=> x.id);
      this.selectedItemChanged.emit(this.selectOptions);
    }
    this.getLookups();
  }

  getLookups(){

  }

  onItemChanged() {

    const selectedItemsAsObjects = this.selectOptions.filter(x=> this.selectedItems.includes(x.id));
    this.selectedItemChanged.emit(selectedItemsAsObjects);
  }

  ngOnDestroy() {
    this.subs.forEach((s) => s.unsubscribe());
  }

}
