import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Subscription, range } from 'rxjs';
import { AppDefaultValues } from '../../../../@core/const/default-values';
import { LookupResponse } from '../../../../@core/models/base/response/lookup.model';
import { Logger } from '../../../../@core/utils.ts/logger.service';

@Component({
  selector: 'app-time-dropdown',
  templateUrl: './time-dropdown.component.html',
  styleUrls: ['./time-dropdown.component.css']
})
export class TimeDropdownComponent implements OnInit {
  private log = new Logger(TimeDropdownComponent.name);

  @Output() selectedItemChanged: EventEmitter<number> = new EventEmitter();
  @Input() fromRange: number;
  @Input() toRange: number;
  @Input() selectedHours: number;
  @Input() selectedMinutes: number;
  @Input() readonly = false;
  @Input() isRequired = false;
  @Input() disabled = false;
  @Input() formcontrol: FormControl;
  @Input() label: string = "ss";
  @Input() placeHolder: string = "ss";
  // @Input() formcontrolForMinutes: FormControl;
  private subs: Subscription[] = [];
  show = false;

  Hours:number[];

  dropDownAllOption = new LookupResponse(AppDefaultValues.DropDownAllOption,
    AppDefaultValues.DropDownAllOptionAr,
    "",
     AppDefaultValues.DropDownAllOptionEn);

  constructor() {
    // this.Hours = Array.from({ length: 24 }, (value, index) => index);
    // this.Hours = this.ranges(1, 24);

  }

  //  rangee(start, end, delta){
  //   return Array.from(
  //     {length: (end - start) / delta}, (v, k) => (k * delta) + start
  //   )
  // };

  ngOnInit() {
    this.Hours = this.ranges(this.fromRange, this.toRange);
    let sds = this.formcontrol;
    this.show = true;

  }

   ranges(start: number, end: number): number[] {
    start = Math.floor(start);
    end = Math.floor(end);

    const diff = end - start;
    if (diff === 0) {
        return [start];
    }

    const keys = Array(Math.abs(diff) + 1).keys();
    return Array.from(keys).map(x => {
        const increment = end > start ? x : -x;
        return start + increment;
    });
}
  onItemChanged() {
    this.selectedItemChanged.emit(this.selectedHours);
  }

}
