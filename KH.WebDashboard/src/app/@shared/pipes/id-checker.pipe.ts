import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'idChecker'
})
export class IdCheckerPipe implements PipeTransform {

  transform(taskCode: string, codeList: number[]): boolean {
    const taskCodeNumber = Number(taskCode);
    if (isNaN(taskCodeNumber) || !Array.isArray(codeList)) {
      return false;
    }
    return codeList.includes(taskCodeNumber);
  }
}
