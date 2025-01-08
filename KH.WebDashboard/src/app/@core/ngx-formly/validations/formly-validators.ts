import { AbstractControl, ValidationErrors } from '@angular/forms';
import { FormlyFieldConfig } from '@ngx-formly/core';
import { isNumber } from '../../utils.ts/data-types.utilts/number-utils';
import { getValidationMessage } from './formly-validation-messages';
import { LanguageEnum } from '../../../@i18n/models/language.enum';

export function fieldMatchValidator(control: AbstractControl) {
  const { password, passwordConfirm } = control.value;
  if (!password || !passwordConfirm) return null;
  return password === passwordConfirm ? null : { fieldMatch: { message: 'Password Not Matching' } };
}

export function IpValidator(control: AbstractControl): ValidationErrors | null {
  const regex = /(\d{1,3}\.){3}\d{1,3}/;
  return regex.test(control.value) ? null : { ip: true };
}

export function dateFutureValidator(control: AbstractControl, field: FormlyFieldConfig, options: any): ValidationErrors | null {
  if (!control.value) return null;
  const minDate = new Date();
  minDate.setDate(minDate.getDate() - (options.minFromDayFromToday || 0));
  const maxDate = new Date();
  maxDate.setDate(maxDate.getDate() + (options.maxToDayFromToday || 0));
  const inputDate = new Date(control.value);
  return inputDate >= minDate && inputDate <= maxDate ? null : { dateFuture: true };
}

export function timeFutureValidator(control: AbstractControl): ValidationErrors | null {
  const isValid = /^([01]\d|2[0-3]):([0-5]\d):([0-5]\d)$/.test(control.value);
  return isValid ? null : { timeFuture: { message: 'Please select a valid time' } };
}

export function autocompleteValidator(
    control: AbstractControl,
    field: FormlyFieldConfig,
    options :any,
  ): ValidationErrors | null {

  if (!control.value || control.value === undefined) {
    return null;
  }
    const isNumberResult = isNumber(control.value)

    if(isNumberResult){
     return null;
    }

    const langKey = localStorage.getItem('language') == LanguageEnum.Ar ? 'ar' : 'en';
    return getValidationMessage('autocomplete-validation', langKey);
    return { 'autocomplete-validation': { message: `please select a value` } };
  }