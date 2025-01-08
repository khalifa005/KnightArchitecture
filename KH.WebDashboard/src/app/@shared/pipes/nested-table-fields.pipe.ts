import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'nestedFields',
})
export class NestedFieldsPipe implements PipeTransform {
  
  transform(columnKey: any, apiCustomColumns: any[]): { label: string; fieldKey: string }[] {
    const matchedColumn = apiCustomColumns.find((x) => x.key === columnKey);
    return matchedColumn?.relatedFields || [];
  }
}
