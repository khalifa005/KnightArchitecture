import { Pipe, PipeTransform } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Pipe({
   name: 'fieldInvalid',
   standalone: true
})
export class FieldInvalidPipe implements PipeTransform {
   transform(control: AbstractControl | null): boolean {
      return !!(control && control.invalid && (control.dirty || control.touched));
   }
} 