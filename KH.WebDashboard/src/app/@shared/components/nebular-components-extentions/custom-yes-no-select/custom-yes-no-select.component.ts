import { ChangeDetectorRef, Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl } from '@angular/forms';
import { AppDefaultValues } from '@app/@core/const/default-values';
import { LookupResponse } from '@app/@core/models/base/response/lookup.model';
import { Logger } from '@app/@core/utils.ts/logger.service';
import { ToastNotificationService } from '@app/@core/utils.ts/toast-notification.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-custom-yes-no-select',
  templateUrl: './custom-yes-no-select.component.html',
  styleUrl: './custom-yes-no-select.component.scss'
})
export class CustomYesNoSelectComponent implements OnInit, OnChanges, OnDestroy {
  private log = new Logger(CustomYesNoSelectComponent.name);

  @Output() selectedItemChanged: EventEmitter<number> = new EventEmitter();
  @Input() selectedItem: any = -1;
  @Input() label: string = "";
  @Input() readonly = false;
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
