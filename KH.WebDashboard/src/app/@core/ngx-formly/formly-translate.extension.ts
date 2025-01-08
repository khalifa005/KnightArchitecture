import { FormlyExtension, FormlyFieldConfig } from '@ngx-formly/core';
import { TranslateService } from '@ngx-translate/core';

export class FormlyTranslateExtension implements FormlyExtension {
  constructor(private translate: TranslateService) {}
  prePopulate(field: FormlyFieldConfig) {
    // const props = field.props || {};
    let props :any = field.props;
    if (!props || !props.translate || props._translated ) {
      return;
    }

    props._translated = true;
    field.expressions = {
      ...(field.expressions || {}),
      'props.label': this.translate.stream(props.label),
    };
  }
}

export function registerTranslateExtension(translate: TranslateService) {
  return {
    validationMessages: [
      {
        name: 'required',
        message() {
          return translate.stream('@form.validation.required');
        },
      },
    ],
    extensions: [
      {
        name: 'translate',
        extension: new FormlyTranslateExtension(translate),
      },
    ],
  };
}


///this is an extention - we can also add custom type -components using nebullar
export class AutoCompleteExtension implements FormlyExtension {
  constructor() {}
  prePopulate(field: FormlyFieldConfig) {

    // console.log("AutoCompleteExtension");
    let props :any = field.props;
    if (!props || !props.translate || props._translated ) {
      return;
    }

    props._translated = true;
    field.expressions = {
      ...(field.expressions || {}),
      // 'props.label': "test",
    };
  }
}