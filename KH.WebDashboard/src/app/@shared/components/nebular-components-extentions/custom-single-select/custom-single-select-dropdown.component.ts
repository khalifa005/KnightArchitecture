import { ChangeDetectorRef, Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { AppDefaultValues } from '../../../../@core/const/default-values';
import { LookupResponse } from '../../../../@core/models/base/response/lookup.model';
import { Logger } from '../../../../@core/utils.ts/logger.service';
import { ToastNotificationService } from '../../../../@core/utils.ts/toast-notification.service';

@Component({
  selector: 'app-custom-single-select-dropdown',
  templateUrl: './custom-single-select-dropdown.component.html',
  styleUrls: ['./custom-single-select-dropdown.component.scss']
})
export class CustomSingleSelectDropdownComponent implements OnInit, OnChanges, OnDestroy {
  private log = new Logger(CustomSingleSelectDropdownComponent.name);

  @Output() selectedItemChanged: EventEmitter<number> = new EventEmitter();
  @Input() selectedItem: number = -1;
  @Input() label: string = "";
  @Input() readonly = false;
  @Input() selectOptions: LookupResponse[];
  @Input() placeHolder: string = "";
  @Input() isRequired = false;
  @Input() disabled = false;
  @Input() formcontrol: FormControl;
  private subs: Subscription[] = [];


  dropDownAllOption = new LookupResponse(AppDefaultValues.DropDownAllOption,
    AppDefaultValues.DropDownAllOptionAr,
    AppDefaultValues.DropDownAllOptionEn);

  constructor(
    private cdr: ChangeDetectorRef,
    private toastNotificationService: ToastNotificationService) {
  }
  ngOnChanges(changes: SimpleChanges): void {

  }

  trackById(index: number, item: any): any {
    return item.id;
  }

  ngOnInit() {
    this.getLookups();
  }

  getLookups() {
    //if we would like to call api
  }

  onItemChanged() {
    this.selectedItemChanged.emit(this.selectedItem);
  }

  ngOnDestroy() {
    this.subs.forEach((s) => s.unsubscribe());
  }

}
