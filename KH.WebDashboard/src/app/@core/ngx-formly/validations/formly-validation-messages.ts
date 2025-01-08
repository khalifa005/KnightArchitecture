import { FormlyFieldConfig } from '@ngx-formly/core';

export function minItemsValidationMessage(error: any, field: any) {
  return `should NOT have fewer than ${field?.props?.minItems} items`;
}

export function maxItemsValidationMessage(error: any, field: any) {
  return `should NOT have more than ${field.props.maxItems} items`;
}

export function minLengthValidationMessage(error: any, field: any) {
  return `should NOT be shorter than ${field.props.minLength} characters`;
}

export function maxLengthValidationMessage(error: any, field: any) {
  return `should NOT be longer than ${field.props.maxLength} characters`;
}

export function minValidationMessage(error: any, field: any) {
  return `should be >= ${field.props.min}`;
}

export function maxValidationMessage(error: any, field: any) {
  return `should be <= ${field.props.max}`;
}

export function multipleOfValidationMessage(error: any, field: any) {
  return `should be multiple of ${field.props.step}`;
}

export function constValidationMessage(error: any, field: any) {
  return `should be equal to constant "${field.props.const}"`;
}

export function typeValidationMessage({ schemaType }: any) {
  return `should be "${schemaType[0]}".`;
}

export function IpValidatorMessage(error: any, field: FormlyFieldConfig) {
    //take anotehr params like message 
    if (!field || !field.formControl) {
      return "";
    }
    return `"${field.formControl.value}" message - is not a valid IP Address`;
  }

  export function exclusiveMinimumValidationMessage(error: any, field: any) {
    return `should be > ${field.props.step}`;
  }
  
  export function exclusiveMaximumValidationMessage(error: any, field: any) {
    return `should be < ${field.props.step}`;
  }

  type ErrorType = 'autocomplete-validation' | 'required' | 'minlength' | 'maxlength' | 'pattern';

  export function getValidationMessage(errorType: ErrorType, lang: 'en' | 'ar') {


    const messages: Record<ErrorType, { en: string; ar: string }> = {
      'autocomplete-validation': {
        en: 'Please select a value',
        ar: 'يرجى اختيار قيمة'
      },
      'required': {
        en: 'This field is required',
        ar: 'هذا الحقل مطلوب'
      },
      'minlength': {
        en: 'Value is too short',
        ar: 'القيمة قصيرة جدًا'
      },
      'maxlength': {
        en: 'Value is too long',
        ar: 'القيمة طويلة جدًا'
      },
      'pattern': {
        en: 'Invalid format',
        ar: 'تنسيق غير صالح'
      }
    };
  
    const message = messages[errorType][lang];
    return { [errorType]: { message } };
  }