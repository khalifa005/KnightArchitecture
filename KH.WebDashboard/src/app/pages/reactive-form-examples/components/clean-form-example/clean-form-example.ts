import { FormGroup, FormControl, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { fileExtensionsValidator, maxFilesValidator, requiredFileUploadValidationSizeInMB } from '../../../../@core/utils.ts/form/validations/form.validation-helpers';
import { LookupResponse } from '../../../../@core/models/base/response/lookup.model';
import { BehaviorSubject } from 'rxjs';
import { AppDefaultValues } from '../../../../@core/const/default-values';

export class CleanExampleForm extends FormGroup {

  // Read-only getters for form controls
  readonly inputControl = this.get('inputControl') as FormControl;
  readonly textareaControl = this.get('textareaControl') as FormControl;
  readonly dateControl = this.get('dateControl') as FormControl;
  readonly productCategoryControl = this.get('productCategoryControl') as FormControl;
  readonly subCategoryControl = this.get('subCategoryControl') as FormControl;
  readonly dropdownControl = this.get('dropdownControl') as FormControl;
  readonly multiSelectControl = this.get('multiSelectControl') as FormControl;
  readonly hoursControl = this.get('hoursControl') as FormControl;
  readonly minutesControl = this.get('minutesControl') as FormControl;
  readonly fileControl = this.get('fileControl') as FormControl;

  // Observable for subcategories
  private subCategoriesSubject = new BehaviorSubject<LookupResponse[]>([]);
  subCategories$ = this.subCategoriesSubject.asObservable();

  constructor(fb: FormBuilder = new FormBuilder(),
    model: any = {},
    private allSubCategories: LookupResponse[] = []
  ) {
    super(
      fb.group({
        inputControl: [model.inputControl || '', [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(20),
          CleanExampleForm.regexValidator(/^[a-zA-Z0-9]*$/)
        ]],
        textareaControl: [model.textareaControl || '', [
          Validators.required,
          Validators.minLength(10),
          Validators.maxLength(200),
        ]],
        dateControl: [model.dateControl || '', [
          Validators.required,
          CleanExampleForm.dateRangeValidator(new Date('2023-01-01'), new Date('2024-12-31')),
        ]],
        productCategoryControl: [model.productCategoryControl || null, Validators.required],
        subCategoryControl: [model.subCategoryControl || null, Validators.required],
        dropdownControl: [model.dropdownControl || null, [Validators.required, CleanExampleForm.valueGreaterThanZeroValidator]],
        multiSelectControl: [model.multiSelectControl || [], Validators.required],
        hoursControl: [model.hoursControl || null, [Validators.required, CleanExampleForm.valueGreaterThanZeroValidator]],
        // minutesControl: [model.minutesControl || null, Validators.required],
        minutesControl: [model.minutesControl || null], // Initially optional

        fileControl: [model.fileControl || [], [
          Validators.required,
          fileExtensionsValidator(['csv', 'xlsx', 'png', 'jpg', 'pdf', 'mp4']),
          maxFilesValidator(2),
          requiredFileUploadValidationSizeInMB(20)
        ]],
      }).controls
    );


    // Initialize form with model
    if (model.subCategoryControl) {
      this.initializeFormWithChild(model.subCategoryControl);
    }

    // Listen for changes in the product category
    this.productCategoryControl.valueChanges.subscribe((parentId) => {
      this.updateSubCategories(parentId);
    });

    // Add conditional validation for minutesControl
    this.addConditionalMinutesValidation();

  }

  initializeFormWithChild(childId: number): void {
    const child = this.allSubCategories.find((sub) => sub.id === childId);
    if (!child) {
      console.error('Child ID not found:', childId);
      return;
    }

    const parentId = child.parentId;
    if (!parentId) {
      console.error('Parent ID not found for child:', childId);
      return;
    }

    this.productCategoryControl.setValue(parentId);
    this.updateSubCategories(parentId, childId);
  }

  updateSubCategories(parentId: number, selectedChildId?: number): void {

    const filteredSubCategories = this.allSubCategories.filter((sub) => sub.parentId === parentId);
    this.subCategoriesSubject.next(filteredSubCategories);

    if (selectedChildId) {
      this.subCategoryControl.setValue(selectedChildId);
    } else if (filteredSubCategories.length > 0) {
      // Set default value for subcategory if no specific child ID is provided
      // this.subCategoryControl.setValue(filteredSubCategories[0].id);
    } else {
      // Reset child control if no subcategories are available
      this.subCategoryControl.reset();
    }

    // this.subCategories = this.allSubCategories.filter((sub) => sub.parentId === parentId);

    // if (selectedChildId) {
    //   this.subCategoryControl.setValue(selectedChildId);
    // } else if (this.subCategories.length > 0) {
    //         // Set default value for subcategory if no specific child ID is provided

    //   // this.subCategoryControl.setValue(this.subCategories[0].id);
    // } else {
    //         // Reset child control if no subcategories are available
    //   this.subCategoryControl.reset();
    // }
  }

  private addConditionalMinutesValidation(): void {
    // Check initial value of hoursControl for default value
    this.updateMinutesValidation(this.hoursControl.value);

    // Listen for changes in hoursControl
    this.hoursControl.valueChanges.subscribe((hoursValue) => {
      this.updateMinutesValidation(hoursValue);
    });
  }

  private updateMinutesValidation(hoursValue: number): void {
    if (hoursValue === AppDefaultValues.DropDownAllOption || !hoursValue) {
      // Make minutesControl optional
      this.minutesControl.clearValidators();
      this.minutesControl.reset();
    } else {
      // Make minutesControl required
      this.minutesControl.setValidators([Validators.required, CleanExampleForm.valueGreaterThanZeroValidator]);
    }
    this.minutesControl.updateValueAndValidity();
  }

  // Static validators
  static regexValidator(pattern: RegExp) {
    return (control: AbstractControl) => {
      if (!control.value) return null;
      return pattern.test(control.value) ? null : { regex: { requiredPattern: pattern.toString() } };
    };
  }

  static dateRangeValidator(minDate: Date, maxDate: Date) {
    return (control: AbstractControl) => {
      const date = new Date(control.value);
      if (date < minDate) return { tooEarly: true };
      if (date > maxDate) return { tooLate: true };
      return null;
    };
  }

  static valueGreaterThanZeroValidator(control: AbstractControl) {
    return control.value > 0 ? null : { valueGreaterThanZero: true };
  }

}
