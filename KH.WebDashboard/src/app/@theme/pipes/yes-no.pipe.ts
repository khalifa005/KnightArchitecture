import { Pipe, PipeTransform } from '@angular/core';
import { LanguageEnum } from '@app/@i18n/models/language.enum';
import { I18nService } from '@app/@i18n/services/i18n.service';

@Pipe({
  name: 'yesNo'
})
export class YesNoPipe implements PipeTransform {

  constructor(private i18nService: I18nService) {}


  transform(option:boolean):string {
      if( this.i18nService.language == LanguageEnum.En){
          if(option){
             return 'Yes';
          }
          return 'No';
      }
      else{
          if(option){
              return "نعم";
           }
          return "لا";
      }
  }


}
