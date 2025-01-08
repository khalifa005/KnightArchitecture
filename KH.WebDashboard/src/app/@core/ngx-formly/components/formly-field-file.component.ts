import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FieldType } from '@ngx-formly/material';
import { FieldTypeConfig } from '@ngx-formly/core';
import { Subject } from 'rxjs';

@Component({
  selector: 'formly-field-file',
  styleUrls: ['./formly-field-file.component.scss'],
  template: `

<!-- <mat-form-field class="form-field" appearance="fill">
      <mat-label>
        {{ props.placeholder }}
        <span *ngIf="props.required" class="required-asterisk">*</span>
      </mat-label>
      <input 
        matInput
        type="file" 
        [formControl]="formControl" 
        [formlyAttributes]="field" 
        (change)="onFileChange($event)"
        (focus)="onFocus()"
        (click)="onClick()" 
      />
      <mat-error *ngIf="showError">{{ errorMessage }}</mat-error>
    </mat-form-field> -->

<div class="form-group col-md-6">
<label [ngClass]="{'text-danger': isFocus && (formControl.invalid) || (formControl.dirty)}">
        {{ props.placeholder }}
        <span *ngIf="props.required" class="required-asterisk">*</span>
      </label>

    <input 
    type="file" 
    [formControl]="formControl" 
    [formlyAttributes]="field" 
    (change)="onFileChange($event)"
    (focus)="onFocus()"
    (click)="onClick()" 
     />
  
     <!-- <div *ngIf="showError">
        <small class="text-danger">{{ errorMessage }}</small>
      </div>
      
      <div *ngIf="(formControl.invalid && isFocus) || (formControl.dirty && isFocus)">
        <p class="text-danger">Please select a file.</p>
      </div> -->
  </div>

  `,
})
export class FormlyFieldFileComponent extends FieldType<FieldTypeConfig> implements OnInit, OnDestroy {
  
  private destroy$: Subject<void> = new Subject<void>();
 isFocus: boolean = false;
 errorMessage: string = 'please select a value';
 keyName: string = '';

  ngOnInit() {
    this.keyName = this.field.key as string;

  }
  
  onFileChange(event: any) {

    let sdsd = this.field;
    let sdsdasa = this.props;
    let tets = "";
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => {
        const base64String = reader.result as string;
        tets = base64String;
        
        // this.formControl.setValue(base64String);
        // this.formControl.patchValue(base64String);

        if(this.model){
          // this.field.formControl.patchValue(base64String);

          this.model[this.keyName] = base64String;
        }
        
      };
      reader.onerror = (error) => {
        console.error('Error reading file: ', error);
        this.formControl.setErrors({ fileReadError: true });
      this.model[this.keyName] = null;

      };
    } else {
      this.formControl.setValue(null);
      this.model[this.keyName] = null;
    }

    let test  = tets;
    let sdhsd = this.formControl.value;
  }

  
  onFocus() {
    this.isFocus = true;
   }
   
   onClick() {
    this.isFocus = true;
   }

  override ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
    super.ngOnDestroy();
  }
}
