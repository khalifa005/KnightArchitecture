import { Inject, LOCALE_ID, Pipe, PipeTransform } from "@angular/core";
import { I18nService } from "../../@i18n/services/i18n.service";
import { LanguageEnum } from "../../@i18n/models/language.enum";

//how to use it
//{{x.nameEn | translator : x.nameAr}}

@Pipe({ name: 'translator' , pure: false })
export class TranslatorPipe implements PipeTransform{

    constructor(private i18nService: I18nService) {}


    transform(englishLanguage:string, arabicLanguage:string):string {
        if( this.i18nService.language == LanguageEnum.En){
            if(!englishLanguage){
               return arabicLanguage;
            }
            return englishLanguage;
        }
        else{
            if(!arabicLanguage){
                return englishLanguage;
             }
            return arabicLanguage;
        }
    }


}

