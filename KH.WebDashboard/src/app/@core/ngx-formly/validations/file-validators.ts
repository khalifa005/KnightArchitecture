import { AbstractControl, ValidationErrors } from '@angular/forms';
import { FormlyFieldConfig } from '@ngx-formly/core';
import { SelectedFile } from '../formly-file-extentions/selected-file';
import { TotalFilesizeError } from '../formly-file-extentions/total-filesize-error';
import { FilenameForbiddenCharactersError } from '../formly-file-extentions/filename-forbidden-characters-error';
import { FileExtensionError } from '../formly-file-extentions/file-extension-error';
import { calculatedFileSizeInKB } from '@app/@core/utils.ts/file/file-utils';

export function maxFiles( control: AbstractControl,
    field: FormlyFieldConfig,
    options :any): ValidationErrors | null {
    if (!control.value ) {
        return null; // No files uploaded yet, so no validation error
    }

    const files = control.value;
    const maxFilesAllowed = options?.maxFiles ?? Number.MAX_SAFE_INTEGER;

    return files.length <= maxFilesAllowed ? null : { maxFiles: { maxAllowed: maxFilesAllowed, actual: files.length } };
}

export function minFiles(control: AbstractControl, options: any): ValidationErrors | null {
    const files = control.value || [];
    return files.length >= options.minFiles ? null : { minFiles: true };
}


export function allowedFileExtensions(
    control: AbstractControl,
    field: FormlyFieldConfig,
    options :any,
  ): ValidationErrors | null 
  {
    // const selectedFiles: SelectedFile[] = control.value;

    if (!control.value) {
      return null;
    }

    const uppercasedAllowedFileExtensions = options.allowedFileExtensions
    .map((extension:String) => extension.toUpperCase());

    if(!uppercasedAllowedFileExtensions || uppercasedAllowedFileExtensions.length == 0 ){
      return null;
    }
    const selectedFile: SelectedFile = control.value;
    const file: File = selectedFile.file;

    const index = file.name.lastIndexOf('.');

    if (index === -1) {
      const error: FileExtensionError = {
        allowedFileExtensions: options.allowedFileExtensions,
        actualFileExtension: "undefined"
      };
      return { fileExtensionError: error };
    }

    const fileExtension = file.name.substring(index + 1);

    if (!uppercasedAllowedFileExtensions.includes(fileExtension.toUpperCase())) {
      const error: FileExtensionError = {
        allowedFileExtensions: options.allowedFileExtensions,
        actualFileExtension: fileExtension
      };
      return { fileExtensionError: error };
    }
    return null;
  }
  

export function maxFilenameLength(control: AbstractControl, options: any): ValidationErrors | null {
    if (!control.value) return null;
    const file = control.value.file;
    const filename = file.name.split('.').slice(0, -1).join('.');
    return filename.length <= options.maxFilenameLength ? null : { maxFilenameLength: true };
}


export function totalFileSize(
    control: AbstractControl,
    field: FormlyFieldConfig,
    options: any,
): ValidationErrors | null {
    if (!control.value ) {
        return null; // No files uploaded yet, so no validation error
    }

    // Convert maxTotalFilesize from MB to bytes
    const maxTotalFilesizeInBytes = (options?.maxTotalFilesize ?? Number.MAX_SAFE_INTEGER) * 1024 * 1024;

     const file = control.value;
        if ( file ) {
          // const extension = file.name.split('.')[1].toLowerCase();
    
          let fileInKB = calculatedFileSizeInKB(options.totalFileSize)
          if (file.size > fileInKB ) {
            return { 
                totalFilesize: { 
                    maxTotalFilesize: fileInKB, 
                    actualTotalFilesize:file.size 
                } 
            };
          }
    
          return null;
        }
    
        return null;
}



export function filenameForbiddenCharacters(
    control: AbstractControl,
    field: FormlyFieldConfig,
    options: any,
): ValidationErrors | null {
    if (!control.value) {
        return null;
    }

    const selectedFile: SelectedFile = control.value;
    const file: File = selectedFile.file;

    const index = file.name.lastIndexOf('.');

    if (index === -1) {
        const error: FilenameForbiddenCharactersError = {
            forbiddenCharacters: options.forbiddenCharacters,
            actualForbiddenCharacters: ["undefined"]
        };
        return { filenameForbiddenCharacters: error };
    }

    const filename = file.name.substring(0, index);
    const actualForbiddenCharacters = new Array<string>();

    options.forbiddenCharacters.forEach((forbiddenCharacter: any) => {
        if (filename.includes(forbiddenCharacter)) {
            actualForbiddenCharacters.push(forbiddenCharacter);
        }
    });

    if (actualForbiddenCharacters.length !== 0) {
        const error: FilenameForbiddenCharactersError = {
            forbiddenCharacters: options.forbiddenCharacters,
            actualForbiddenCharacters
        };
        return { filenameForbiddenCharacters: error };
    }

    return null;
}