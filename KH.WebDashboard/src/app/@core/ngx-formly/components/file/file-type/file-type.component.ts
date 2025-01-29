import { Component } from '@angular/core';
import { FieldArrayType, FieldArrayTypeConfig, FormlyFieldConfig, FormlyFieldProps } from '@ngx-formly/core';
import { SelectedFile } from '../../../formly-file-extentions/selected-file';

@Component({
  selector: 'ngz-formly-material-file-type',
  templateUrl: './file-type.component.html',
  styleUrls: ['./file-type.component.scss']
})
export class FileTypeComponent extends FieldArrayType {

  requiredFileType: string = '';
  maxFiles: string = '';
  maxFileSizeInMB: string = '';

  ngOnInit() {
    if (this.field.fieldArray) {
      this.getAllowedFileExtensions();
      this.getMaxFileSize();
      this.getMaxUploadFiles();

    }

    if (this.field.validators) {
      // console.log("Field Validators:", this.field.validators);
    }
  }


  onSelectFiles(files: SelectedFile[]) {
    // this.getAllowedFileExtensions();
    // this.getMaxFileSize();
    // this.getMaxUploadFiles();

    // console.log("FieldArray:", this.field.fieldArray);
    // // console.log("FieldArray:", this.field.fieldArray.);
    // console.log("Validators:", this.field.validators);

    // console.log("formControl");
    // console.log(this.formControl);
    // console.log("field.formControl");
    // console.log(this.field.formControl);
    // console.log("file props");
    // console.log(this.props);
    // const test1 = this.form;
    // const test2 = this.model;
    // const test2dss = this.field.fieldGroup;
    // const test2ds = this.field;
    // const a = this.form;
    // const aa = this.model;
    // const aas = this.field;

    this.field.formControl.markAsTouched();
    files.forEach(file => {
      this.add(this.formControl.length, file);
    });

  }

  onDeleteFile(index: number) {

    console.log("field.fieldGroup");
    console.log(this.field.fieldGroup);
    console.log(this.field);
    let filesValue = this.formControl.value;
    let targetFileToDelete = filesValue[index];
    //targetFileToDelete.filePath send it to specific api to delete
    this.remove(index);
  }


  getAllowedFileExtensions(): string[] {
    let fieldArrayConfig: FormlyFieldConfig | null = null;

    if (this.field.fieldArray) {
      // If fieldArray is a function, invoke it with `this.field` to get the actual config
      fieldArrayConfig =
        typeof this.field.fieldArray === 'function'
          ? this.field.fieldArray(this.field)
          : this.field.fieldArray;
    }

    // Ensure fieldArrayConfig exists and has validators
    if (fieldArrayConfig?.validators?.validation) {
      const allowedExtensionsValidator = fieldArrayConfig.validators.validation.find(
        (v) => v.name === 'allowed-file-extensions'
      );
      if (allowedExtensionsValidator) {
        this.requiredFileType = allowedExtensionsValidator.options?.allowedFileExtensions.join(',');
        return allowedExtensionsValidator.options?.allowedFileExtensions || [];
      }
    }

    return [];
  }


  getMaxUploadFiles(): number {
    let fieldArrayConfig: FormlyFieldConfig | null = null;

    if (this.field.fieldArray) {
      fieldArrayConfig =
        typeof this.field.fieldArray === 'function'
          ? this.field.fieldArray(this.field)
          : this.field.fieldArray;
    }

    if (fieldArrayConfig?.validators?.validation) {
      const maxFilesValidator = fieldArrayConfig.validators.validation.find(
        (v) => v.name === 'max-files'
      );
      if (maxFilesValidator) {
        this.maxFiles = maxFilesValidator.options?.maxFiles;
        return maxFilesValidator.options?.maxFiles || Number.MAX_SAFE_INTEGER; // Default to no limit if undefined
      }
    }

    return Number.MAX_SAFE_INTEGER; // No limit if validator is missing
  }

  getMaxFileSize(): number {
    let fieldArrayConfig: FormlyFieldConfig | null = null;

    if (this.field.fieldArray) {
      fieldArrayConfig =
        typeof this.field.fieldArray === 'function'
          ? this.field.fieldArray(this.field)
          : this.field.fieldArray;
    }

    if (fieldArrayConfig?.validators?.validation) {
      const maxFileSizeValidator = fieldArrayConfig.validators.validation.find(
        (v) => v.name === 'total-file-size'
      );
      if (maxFileSizeValidator) {
        this.maxFileSizeInMB = maxFileSizeValidator.options?.totalFileSize;
        return maxFileSizeValidator.options?.totalFileSize || Number.MAX_SAFE_INTEGER; // Default to no limit if undefined
      }
    }

    return Number.MAX_SAFE_INTEGER; // No limit if validator is missing
  }




}
