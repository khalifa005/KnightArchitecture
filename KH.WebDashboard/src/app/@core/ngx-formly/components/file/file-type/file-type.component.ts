import { Component } from '@angular/core';
import { FieldArrayType, FieldArrayTypeConfig, FormlyFieldProps } from '@ngx-formly/core';
import { SelectedFile } from '../../../formly-file-extentions/selected-file';

@Component({
  selector: 'ngz-formly-material-file-type',
  templateUrl: './file-type.component.html',
  styleUrls: ['./file-type.component.scss']
})
export class FileTypeComponent extends FieldArrayType {

  onSelectFiles(files: SelectedFile[]) {

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

}
