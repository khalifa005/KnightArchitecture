import { Component, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { FieldType } from '@ngx-formly/material';
import { FieldTypeConfig } from '@ngx-formly/core';
import { Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, startWith, switchMap, takeUntil } from 'rxjs/operators';
import { getItemsByArrayNameKeyword } from '../../utils.ts/data-types.utilts/array-utils';
import { isNumber } from '../../utils.ts/data-types.utilts/number-utils';

@Component({
  selector: 'formly-autocomplete-type',
  styleUrls: ['./autocomplete-type.component.scss'],
  template: `

  <input
    matInput
    [matAutocomplete]="auto"
    [formControl]="formControl"
    [formlyAttributes]="field"
    [placeholder]="props.placeholder"
    [errorStateMatcher]="errorStateMatcher">
  

  <mat-autocomplete #auto="matAutocomplete" [displayWith]="displayFn">
    <mat-option *ngFor="let value of filter | async" [value]="value.id">
      {{ value.name }}
    </mat-option>
  </mat-autocomplete>
  <mat-icon matSuffix>arrow_drop_down</mat-icon>

<!-- </mat-form-field> -->
`,
})

export class AutocompleteTypeComponent extends FieldType<FieldTypeConfig> implements OnInit, OnDestroy  {
  filter: Observable<any>;
  private destroy$: Subject<void> = new Subject<void>();

  listOfOptionsTest:any[];
  selectedItem : string = "testtt";
  private dynamicSearch(text: string, list :any[]): any[] {
    
    // console.log(this.props.label + "  this.props.autocomplete");
    // console.log(this.props.options);

    if(isNumber(text)){
    return list;    
    }

    return text ? list.filter(item => item.name.toLowerCase().includes(text.toLowerCase())) : list;
  }

  private setupFilter() {
    this.filter = this.formControl.valueChanges.pipe(
      startWith(''),
      map(text => this.dynamicSearch(text, this.props.options as any[])),
      takeUntil(this.destroy$)
    );
  }

  private setupParentControlListener() {
    const parentId = this.props['parentId'];
    if (parentId) {
      const parentControl = this.form.get(parentId);
      if (parentControl) {
        if (parentControl.value) {
          this.updateOptionsBasedOnParent(parentControl.value);
        }

        parentControl.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(parentNewValue => {
          this.updateOptionsBasedOnParent(parentNewValue);
        });
      }
    }else{
      // this.updateOptionsBasedForSelected();
    }
  }

  private updateOptionsBasedForSelected() {
    const optionsKey = this.props['optionsKey'];
    if (optionsKey) {
      const listOfOptions = getItemsByArrayNameKeyword(this.options.formState.selectOptionsData, optionsKey) as any[];
      this.listOfOptionsTest = listOfOptions;
    }
  }

  private updateOptionsBasedOnParent(parentNewValue: any) {
    const optionsKey = this.props['optionsKey'];
    if (optionsKey) {
      const listOfOptions = getItemsByArrayNameKeyword(this.options.formState.selectOptionsData, optionsKey) as any[];
      this.listOfOptionsTest = listOfOptions;
      if (listOfOptions) {
        this.props.options = listOfOptions.filter(option => option.parentId == parentNewValue);
        this.formControl.patchValue(null);
      }
    }
  }

  displayFn(item: any): string {

    if(item && this.listOfOptionsTest ){
      const options = this.props.options as any[];
      const selectedOption = options.find(x=> x.id == item);
      
      return selectedOption ? selectedOption.name : 'undefined';
    }
    
    return item;
  }

  ngOnInit() {

    if (this.formControl.value) {
      this.formControl.patchValue(this.formControl.value);
    }
    this.setupFilter();
    this.setupParentControlListener();
  }

  // ngOnInit() {
  //   //get form controle based on parentId - then listen on chnages and updated the options
  //   console.log(this.props['parentId']);
  //   console.log(this.props['optionsKey']);

  //   if(this.props['parentId']){
 
  //   let parentControl = this.form.get(this.props['parentId']);
  //   if(parentControl){

  //     parentControl.valueChanges
  //     .pipe(
  //       takeUntil(this.destroy$) // This will handle unsubscribing when destroy$ emits
  //     ).subscribe(parentNewValue => {
  //       console.log('Value changed:', parentNewValue);

  //       const optionsKey = this.props['optionsKey'];
        
  //       if(optionsKey){

  //         let listOfOptions = getItemsByArrayNameKeyword(this.options.formState.selectOptionsData, optionsKey) as any[];
  //         if(listOfOptions){
  //           this.props.options = listOfOptions.filter(option => option.parentId == parentNewValue) as any[];
  //           console.log("this.props['listOfOptions.filter']");
  //           console.log(this.props.options);
  //           this.formControl.patchValue(null);

  //         }
  //       }
        
  //     // this.filter = this.formControl.valueChanges.pipe(
  //     //   startWith(''),
  //     //   map(text => this.dynamicSearch(text, this.props.options as any[])),
  //     // );
  //       // You can add your logic here that needs to run when value changes
  //     });

  //   }
  //   }

  //   // const test = this.props.options;
  //   // this.filter = this.formControl.valueChanges.pipe(
  //   //   startWith(''),
  //   //   map(text => this.search(text)),
  //   // );
    
  //   this.filter = this.formControl.valueChanges.pipe(
  //     startWith(''),
  //     map(text => this.dynamicSearch(text, this.props.options as any[])),
  //   );

  //   // console.log("autocomplete.options"); // This will log the FormlyFormOptions associated with the form.
  //   // console.log(this.options.formState.selectOptionsData);
    
  //   // console.log("autocomplete.options.Data"); // This will log the FormlyFormOptions associated with the form.
  //   // const teams = getItemsByArrayNameKeyword(this.options.formState.selectOptionsData, 'team');

  //   // console.log(teams);

    
  // }
  
  override ngOnDestroy() {
    // Custom cleanup logic for this child component
    this.destroy$.next();
    this.destroy$.complete();

    // Call the parent class's ngOnDestroy method
    super.ngOnDestroy();
  }

}
