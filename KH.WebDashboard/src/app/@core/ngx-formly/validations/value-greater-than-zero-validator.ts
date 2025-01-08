import { AbstractControl, ValidationErrors } from "@angular/forms";

export function valueGreaterThanZeroValidator(
  control: AbstractControl
): ValidationErrors | null {
  if (control.value !== null && control.value <= 0) {
    return { valueGreaterThanZero: true }; // Validation error key
  }
  return null; // No error
}
