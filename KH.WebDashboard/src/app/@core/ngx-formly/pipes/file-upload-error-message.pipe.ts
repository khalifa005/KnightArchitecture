import { Pipe, PipeTransform } from '@angular/core';
import { LanguageEnum } from '@app/@i18n/models/language.enum';
import { I18nService } from '@app/@i18n/services/i18n.service';

@Pipe({
  name: 'fileUploadErrorMessage'
})
export class FileUploadErrorMessagePipe implements PipeTransform {
  
  constructor(private i18nService: I18nService) {}

  transform(errorKey: string, errorValue: any): string {
    let message: string;

    const language = this.i18nService.language;
    
    switch (errorKey) {
      case 'required':
        message = language === LanguageEnum.Ar ? 'هذه الخانة مطلوبة' : 'This field is required';
        break;
      case 'email':
        message = language === LanguageEnum.Ar ? 'البريد الإلكتروني غير صالح' : 'Invalid email address';
        break;
      case 'minlength':
        message = language === LanguageEnum.Ar ? `الحد الأدنى للطول هو ${errorValue?.requiredLength} أحرف` : `Minimum length is ${errorValue?.requiredLength} characters`;
        break;
      case 'maxlength':
        message = language === LanguageEnum.Ar ? `الحد الأقصى للطول هو ${errorValue?.requiredLength} أحرف` : `Maximum length is ${errorValue?.requiredLength} characters`;
        break;
      case 'pattern':
        message = language === LanguageEnum.Ar ? 'تنسيق غير صالح' : 'Invalid format';
        break;
      case 'customError':
        message = language === LanguageEnum.Ar ? 'يجب أن يكون الطول على الأقل 5 أحرف' : 'Value must be at least 5 characters long';
        break;
      case 'min':
        message = language === LanguageEnum.Ar ? `الحد الأدنى للقيمة هو ${errorValue?.min}` : `Minimum value is ${errorValue?.min}`;
        break;
      case 'max':
        message = language === LanguageEnum.Ar ? `الحد الأقصى للقيمة هو ${errorValue?.max}` : `Maximum value is ${errorValue?.max}`;
        break;
      case 'maxFiles':
        message = language === LanguageEnum.Ar ? `أقصى عدد للملفات المسموح به هو ${errorValue?.maxFiles} الملفات الحالية ${errorValue?.actualFiles}` : `Max files allowed is ${errorValue?.maxFiles} current files ${errorValue?.actualFiles}`;
        break;
      case 'minFiles':
        message = language === LanguageEnum.Ar ? `الحد الأدنى للملفات المطلوبة هو ${errorValue?.minFiles} الملفات الحالية ${errorValue?.actualFiles}` : `Min files required is ${errorValue?.minFiles} current files ${errorValue?.actualFiles}`;
        break;
      case 'totalFilesize':
        message = language === LanguageEnum.Ar ? `الحجم الكلي المسموح به للملفات هو ${Math.round(errorValue?.maxTotalFilesize / (1024 * 1024))} ميجابايت، حجم الملفات الحالي هو ${Math.round(errorValue?.actualTotalFilesize / (1024 * 1024))} ميجابايت` : `Total allowed file size is ${Math.round(errorValue?.maxTotalFilesize / (1024 * 1024))} MB, current file size is ${Math.round(errorValue?.actualTotalFilesize / (1024 * 1024))} MB`;
        break;
      case 'fileExtension':
        message = language === LanguageEnum.Ar ? `امتداد الملف المسموح به هو ${errorValue?.allowedFileExtensions}، امتداد الملف الحالي هو ${errorValue?.actualFileExtension}` : `File Extension allowed is ${errorValue?.allowedFileExtensions}, current file extension is ${errorValue?.actualFileExtension}`;
        break;
      case 'filenameForbiddenCharacters':
        message = language === LanguageEnum.Ar ? `طول اسم الملف غير مسموح به هو ${errorValue?.forbiddenCharacters}، اسم الملف الحالي يحتوي على ${errorValue?.actualForbiddenCharacters} حرف` : `File name length not allowed is ${errorValue?.forbiddenCharacters}, current file name contains ${errorValue?.actualForbiddenCharacters} char`;
        break;
      case 'fileExtensionError':
        message = language === LanguageEnum.Ar ? `امتداد الملف المسموح به هو ${errorValue?.allowedFileExtensions}، امتداد الملف الحالي هو ${errorValue?.actualFileExtension}` : `File Extension allowed is ${errorValue?.allowedFileExtensions}, current file extension is ${errorValue?.actualFileExtension}`;
        break;
      case 'maxFilenameLength':
        message = language === LanguageEnum.Ar ? `طول اسم الملف المسموح به هو ${errorValue?.maxFilenameLength}، طول اسم الملف الحالي هو ${errorValue?.acturalFilenameLength}` : `File name length allowed is ${errorValue?.maxFilenameLength}, current file name length is ${errorValue?.acturalFilenameLength}`;
        break;
      case 'uploadInProgress':
        message = language === LanguageEnum.Ar ? `جاري تحميل الملف  ` : `Uploading file ... `;
        // console.log("uploadInProgress- testing");
        // console.log(errorValue);
        break;
      case 'uploadError':
        message = language === LanguageEnum.Ar ? `خطأ في تحميل الملف  ` : `Invalid file `;
        break;
      default:
        message = language === LanguageEnum.Ar ? `حقل غير صالح: ${errorKey}` : `Invalid field: ${errorKey}`;
    }

    return message;
  }
}
