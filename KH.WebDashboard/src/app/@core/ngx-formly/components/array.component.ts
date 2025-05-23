import { Component } from '@angular/core';
import { FieldArrayType } from '@ngx-formly/core';

@Component({
  selector: 'formly-array-type',
  template: `
    <div class="mb-3">
      <legend *ngIf="props.label">{{ props.label }}</legend>
      <p *ngIf="props.description">{{ props.description }}</p>
      <div class="d-flex flex-row-reverse">
        <button class="btn btn-primary" type="button" (click)="add()">+</button>
      </div>

      <div class="alert alert-danger" role="alert" *ngIf="showError && formControl.errors">
        <formly-validation-message [field]="field"></formly-validation-message>
      </div>

      <div *ngFor="let field of field.fieldGroup; let i = index" class="row align-items-start">
        <formly-field class="col" [field]="field"></formly-field>
        <div *ngIf="field.props.removable !== false" class="col-2 text-right">
          <button class="btn btn-danger" type="button" (click)="remove(i)">-</button>
        </div>
      </div>
    </div>
  `,
})
export class ArrayTypeComponent extends FieldArrayType {}


/**  Copyright 2021 Formly. All Rights Reserved.
    Use of this source code is governed by an MIT-style license that
    can be found in the LICENSE file at https://github.com/ngx-formly/ngx-formly/blob/main/LICENSE */