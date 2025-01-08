import { Component, OnInit } from '@angular/core';
import { FieldType } from '@ngx-formly/material';
import { FieldTypeConfig } from '@ngx-formly/core';
import { Observable } from 'rxjs';
import { map, startWith, switchMap } from 'rxjs/operators';

interface State {
    id: number;
    name: string;
  }

@Component({
  selector: 'formly-autocomplete-type',
  template: `

 <!-- <input nbInput placeholder="Pick Date" [nbDatepicker]="formcontrol" [formControl]="formControl">
        <nb-datepicker #formcontrol></nb-datepicker>
         -->
      <!-- <input
   type="text"
   nbInput 
   fullWidth
   id="TypeId"
   placeholder="{{ props.placeholder | translate }}"
   [status]="(formControl.invalid && (formControl.dirty || formControl.touched)) ? 'danger' : null"
   [formControl]="formControl"
   > -->
 
`,
})

export class NbInputFormlyComponent extends FieldType<FieldTypeConfig> implements OnInit {



  ngOnInit() {

  }
}
