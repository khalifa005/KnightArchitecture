import { Component, EventEmitter, Input, OnDestroy, OnInit, Optional, Output } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { NbWindowService, NbWindowRef } from '@nebular/theme';
import { Subscription, Subject, takeUntil } from 'rxjs';
import { NavItem } from '../../../../@core/interfaces/nav-item';
import { Logger } from '../../../../@core/utils.ts/logger.service';
import { AddNoteForm } from './add-note.form';
import { ToastNotificationService } from '../../../../@core/utils.ts/toast-notification.service';

@Component({
  selector: 'app-add-note-form',
  templateUrl: './add-note-form.component.html',
  styleUrls: ['./add-note-form.component.scss']
})
export class AddNoteFormComponent implements OnInit, OnDestroy {

  private log = new Logger(AddNoteFormComponent.name);
  
  @Output() valueChange = new EventEmitter<any>();
  @Input() passedRowData : any;
  @Input() title : string;
  @Input() menuItem : NavItem;
  private subs: Subscription[] = [];
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  form: AddNoteForm;
  isUpdateMode: boolean = false;

  formRandomNumber: number;
  taskNoteRequestParameter: any = {};

  constructor(
    private router:Router,
    @Optional() private windowService: NbWindowService,
    @Optional() public windowRef: NbWindowRef<AddNoteFormComponent>,
    private toastNotificationService:ToastNotificationService,
    public fb: FormBuilder)
    {

    }

  // modelDto: AddNotePayloadDto = new AddNotePayloadDto();

public cancel(statusCode:number) {
  this.windowRef.close(statusCode);
}

ngOnInit() {
  // const userInfo = this.authApiService.getUserInfoFromSys();

  }

  
save(){
  if (this.form.valid) {

  }
}



private mapFormToDto(form: AddNoteForm): any {
  return {
    ...form.value,
  } as any;
}

reset() {
  this.form.reset();
}


toggleAllControls(enable: boolean) {
  Object.keys(this.form.controls).forEach(controlName => {
    const control = this.form.get(controlName);
    if (enable) {
      control?.enable();
    } else {
      control?.disable();
    }
  });
}

ngOnDestroy() {
  this.subs.forEach((s) => s.unsubscribe());
  this.ngUnsubscribe.next();
  this.ngUnsubscribe.complete();
}


}
