import { FormGroup, FormControl, FormBuilder, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';

export class LoginForm extends FormGroup {
  readonly usernameControl = this.get('username') as FormControl;
  readonly passwordControl = this.get('password') as FormControl;

  constructor(
    private translate: TranslateService,
    fb: FormBuilder = new FormBuilder()
  ) {
    super(
      fb.group({
        username: [
          null,
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(20),
            LoginForm.regexValidator(/^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]*$/, 'VALIDATION.USERNAME.INVALID'),
          ],
        ],
        password: [
          null,
          [
            Validators.required,
            Validators.minLength(8),
            Validators.maxLength(20),
            LoginForm.regexValidator(/^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]*$/, 'VALIDATION.PASSWORD.INVALID'),
          ],
        ],
      }).controls
    );
  }

  /**
   * Static regex validator with a translation key.
   */
  static regexValidator(pattern: RegExp, translationKey: string) {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control.value) return null;
      return pattern.test(control.value) ? null : { regex: { key: translationKey } };
    };
  }

  /**
   * Get error messages dynamically using ngx-translate.
   */
  getErrorMessage(controlName: string): string | null {
    const control = this.get(controlName);
    if (!control || !control.errors) return null;

    if (control.hasError('required')) {
      return this.translate.instant('VALIDATION.REQUIRED');
    }
    if (control.hasError('minlength')) {
      const requiredLength = control.errors['minlength'].requiredLength;
      return this.translate.instant('VALIDATION.MIN_LENGTH', { length: requiredLength });
    }
    if (control.hasError('maxlength')) {
      const requiredLength = control.errors['maxlength'].requiredLength;
      return this.translate.instant('VALIDATION.MAX_LENGTH', { length: requiredLength });
    }
    if (control.hasError('regex')) {
      const translationKey = control.errors['regex'].key;
      return this.translate.instant(translationKey);
    }

    return null;
  }
}
