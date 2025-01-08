import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';

export class WorkingDayForm extends FormGroup {
  constructor(fb: FormBuilder, model: any = {}) {
    super(
      fb.group({
        workingDate: [model.workingDate || null, [
          Validators.required,
          WorkingDayForm.dateRangeValidator(new Date('2023-01-01'), new Date('2024-12-31')),
        ]],
        fromWorkingHour: [model.fromWorkingHour || null, [
          Validators.required,
          Validators.min(0),
          Validators.max(23),
        ]],
        toWorkingHour: [model.toWorkingHour || null, [
          Validators.required,
          Validators.min(0),
          Validators.max(23),
        ]],
      }).controls
    );
  }

  static dateRangeValidator(minDate: Date, maxDate: Date) {
    return (control: AbstractControl) => {
      const date = new Date(control.value);
      if (date < minDate) return { tooEarly: true };
      if (date > maxDate) return { tooLate: true };
      return null;
    };
  }
}
