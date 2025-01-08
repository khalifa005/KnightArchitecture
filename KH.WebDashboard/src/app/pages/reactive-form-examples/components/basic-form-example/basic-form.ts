import { FormGroup, FormControl, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { fileExtensionsValidator, maxFilesValidator, requiredFileUploadValidationSizeInMB } from '../../../../@core/utils.ts/form/validations/form.validation-helpers';

export class BasicExampleForm extends FormGroup {

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

  constructor(fb: FormBuilder = new FormBuilder(), model: any = {}) {
    super(
      fb.group({
        inputControl: [model.inputControl || '', [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(20),
          BasicExampleForm.regexValidator(/^[a-zA-Z0-9]*$/)
        ]],
        textareaControl: [model.textareaControl || '', [
          Validators.required,
          Validators.minLength(10),
          Validators.maxLength(200),
        ]],
        dateControl: [model.dateControl || '', [
          Validators.required,
          BasicExampleForm.dateRangeValidator(new Date('2023-01-01'), new Date('2024-12-31')),
        ]],
        productCategoryControl: [model.productCategoryControl || null, Validators.required],
        subCategoryControl: [model.subCategoryControl || null, Validators.required],
        dropdownControl: [model.dropdownControl || null, [Validators.required, BasicExampleForm.valueGreaterThanZeroValidator]],
        multiSelectControl: [model.multiSelectControl || [], Validators.required],
        hoursControl: [model.hoursControl || null, [Validators.required, BasicExampleForm.valueGreaterThanZeroValidator]],
        minutesControl: [model.minutesControl || null, Validators.required],
        fileControl: [model.fileControl || [], [
          Validators.required,
          fileExtensionsValidator(['csv', 'xlsx', 'png', 'jpg', 'pdf', 'mp4']),
          maxFilesValidator(2),
          requiredFileUploadValidationSizeInMB(20)
        ]],
      }).controls
    );
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
