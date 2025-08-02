import { Pipe, PipeTransform, inject } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { ALAIN_I18N_TOKEN } from '@delon/theme';

@Pipe({
   name: 'fieldError',
   standalone: true
})
export class FieldErrorPipe implements PipeTransform {
   private readonly i18nSrv = inject(ALAIN_I18N_TOKEN);

   transform(control: AbstractControl | null, fieldName: string): string {
      if (!control || !control.errors) {
         return '';
      }

      const errors = control.errors;

      if (errors['required']) {
         return this.i18nSrv.fanyi('user.form.validation.required');
      }

      if (errors['email']) {
         return this.i18nSrv.fanyi('user.form.validation.email');
      }

      if (errors['pattern']) {
         if (fieldName === 'phone_number') {
            return this.i18nSrv.fanyi('user.form.validation.phone');
         }
         if (fieldName === 'national_id') {
            return this.i18nSrv.fanyi('user.form.validation.national_id');
         }
      }

      if (errors['minlength']) {
         return this.i18nSrv.fanyi('user.form.validation.min_length')
            .replace('{0}', errors['minlength'].requiredLength);
      }

      if (errors['maxlength']) {
         return this.i18nSrv.fanyi('user.form.validation.max_length')
            .replace('{0}', errors['maxlength'].requiredLength);
      }

      return '';
   }
} 