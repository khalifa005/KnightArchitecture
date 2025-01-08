import { Component, EventEmitter, Inject, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import { FormlyField, FormlyFieldConfig } from '@ngx-formly/core';
import { Observable, of, Subscription } from 'rxjs';
import { FileUploadService } from './file-upload.service';
import { NotitficationsDefaultValues } from '../../../../const/notitfications-default-values';
import { ToastNotificationService } from '../../../../utils.ts/toast-notification.service';
// import { FILE_TYPE_CONFIG, FileTypeConfig } from '../../../formly-file-extentions/file-type-config';
import { SelectedFile } from '../../../formly-file-extentions/selected-file';
import { FileApiService } from '../../../../api-services/fileApiService';

@Component({
  selector: 'ngz-formly-material-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss'],
  providers: [FileUploadService]
})
export class FileUploadComponent implements OnInit, OnDestroy {

  @Input()
  index: number;

  @Input()
  field?: FormlyFieldConfig;

  @Input()
  mainField?: any;
  // mainField?: FormlyField;

  @Input()
  uploadUrl?: string;

  @Input()
  passedForm?: any;

  @Input()
  paramName?: string;

  @Output()
  deleteFile = new EventEmitter<any>();

  progress = 0;
  fakeProgress = 15;
  
  fileId:string | undefined;

  uploadError?: string;
  
  editMode?: boolean = false;

  file?: File;

  fileIcon = 'fileType:file';

  private progessSubscription?: Subscription;

  constructor(
    // @Inject(FILE_TYPE_CONFIG) public readonly fileTypeConfig: FileTypeConfig,
    private fileApiService:FileApiService,
    private readonly uploadService: FileUploadService,
    private toastNotificationService:ToastNotificationService) { }

  ngOnInit() {
    this.fileIcon = this.uploadUrl ? 'fileType:fileUpload' : 'fileType:file';
   
    if (this.field?.formControl) {
      const selectedFile: SelectedFile = this.field.formControl.value;
      this.file = selectedFile?.file;
      const FormMode = this.passedForm?.controls?.FormMode?.value ?? "";
      if(selectedFile.fileID){
      //FormMode == "EDIT"
        this.progress = 100;
        this.editMode = true;
        this.fileId = selectedFile.fileID;
      }else{
              //exist the
      if (!this.field.formControl.valid || !this.uploadUrl || !this.file) {
        return;
      }

      this.field.formControl.setAsyncValidators(this.validateUpload.bind(this));

      setTimeout(() => this.field?.formControl?.updateValueAndValidity(), 0);

      const taskCode = this.passedForm?.controls?.TaskCode?.value ?? ""; 
      const FBFId = this.passedForm?.controls?.FBFId?.value ?? ""; //report Id
      const fieldId = this.mainField?.id ?? "";

      this.progessSubscription = this.uploadService.upload(taskCode,FBFId,fieldId, this.file, this.uploadUrl, this.paramName!)
        .subscribe(
          uploadState => {
            
            this.progress = uploadState.progress;
            this.fakeProgress = uploadState.progress < 100 ? this.fakeProgress +20 : 100;
            
            // this.incrementProgressSmoothly(uploadState.progress);
            if (this.progress === 100) {
              // this.field!.formControl!.value!.location = uploadState.location;
              this.field!.formControl!.value!.filePath = uploadState?.filePath;
              this.field!.formControl!.value!.fileId = uploadState?.fileId;
              this.fileId = uploadState?.fileId ;
              // this.field!.formControl!.setValue(uploadState.filePath);
            }
          },
          error => {
            this.uploadError = error;
            this.field?.formControl?.updateValueAndValidity();
          },
          () => {
            this.field?.formControl?.updateValueAndValidity();
            if (this.progress === 100 && !this.uploadError) {
              this.fileIcon = 'fileType:file';
            }
          });
      }

    }
  }

  private validateUpload(): Observable<ValidationErrors | null> {
    if (this.uploadError) {
      return of({ uploadError: true });
    }

    if (this.progress === 100) {
      this.toastNotificationService.showToast(NotitficationsDefaultValues.Success, 'Uploaded', "");

      return of(null);
    }

    return of({ uploadInProgress: true });
  }

  ngOnDestroy() {
    this.cancelUpload();
  }

  removeFile() {
    this.cancelUpload();

    const test1 = this.passedForm;
    const test2 = this.field;
    const test3 = this.file;
    const test5 = this.mainField;
    const FBFId = this.passedForm?.controls?.FBFId?.value ?? ""; //report Id;
    const valuesOfTheFile = this.passedForm?.controls[this.mainField.key];
    const valuesOfTheFile2 = this.mainField.formControl.value;

    if(this.fileId){

      this.fileApiService.deleteFormlyFileById(this.fileId ?? "", FBFId ?? "").subscribe({
        next:(response) =>{
          console.log('File deleted successfully:', response);
        },
        error:(error) =>{}
  
      });
    }else{
      //delete from populated items in edit mode

      // const valuesOfTheFile = this.passedForm?.controls[this.mainField.key];
      const filesValue = this.mainField.formControl.value;
      const targetFile = filesValue.find((x:any)=> x.file.name == this.file?.name )

      const filesValues = filesValue[this.index];

      console.log("this.index");
      console.log(this.index);
      console.log(filesValues);
      console.log(filesValue[this.index]);

      this.fileApiService.deleteFormlyFileById(filesValues.fileId ?? "", FBFId ?? "").subscribe({
        next:(response) =>{
          console.log('File deleted successfully:', response);
        },
        error:(error) =>{}
  
      });
    }
    
    this.deleteFile.emit();
  }

  get showProgressBar(): boolean {
    if(!!this.progessSubscription){
      // console.log("ss");
    }
    
    if(!this.uploadError){
      // console.log("ss");
    }
    return !!this.progessSubscription && !this.uploadError;
  }

  get status() {
    if (this.fakeProgress <= 25) {
      return 'danger';
    } else if (this.fakeProgress <= 50) {
      return 'warning';
    } else if (this.fakeProgress <= 75) {
      return 'info';
    } else {
      return 'success';
    }
  }

  private cancelUpload() {
    if (this.progessSubscription) {
      this.progessSubscription.unsubscribe();
    }
  }

    // Smoothly increment the progress bar - not used
    incrementProgressSmoothly(targetProgress: number) {
      const step = 1; // Small step for smoothness
      const interval = 5; // Interval in ms for smoother animation
  
      const intervalId = setInterval(() => {
        if (this.progress < targetProgress) {
          this.progress += step;
        } else {
          clearInterval(intervalId);
          this.progress = targetProgress; // Ensure it reaches the exact target
        }
      }, interval);
    }

}
