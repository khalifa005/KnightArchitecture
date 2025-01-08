import { HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { NbWindowRef } from '@nebular/theme';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { FileApiService } from '../../../../@core/api-services/fileApiService';
import { Logger } from '../../../../@core/utils.ts/logger.service';

@Component({
  selector: 'app-file-preview',
  templateUrl: './file-preview.component.html',
  styleUrls: ['./file-preview.component.scss']
})
export class FilePreviewComponent implements OnInit , OnDestroy {

  private log = new Logger(FilePreviewComponent.name);
  private ngUnsubscribe: Subject<void> = new Subject<void>();
  private subs: Subscription[] = [];
  
  loadingSpinner = true;

  @Output() valueChange = new EventEmitter<any>();
  @Input() passedMediaId : any;
  @Input() passedMediaExtention : any;
  progress: number;
  public fileData: string | ArrayBuffer | null;
  // public videoData: string | ArrayBuffer | null;
  
  constructor(
    private fileApiService: FileApiService,
    public windowRef: NbWindowRef<FilePreviewComponent>
  )
  {
    
  }



public cancel(statusCode:number) {
  this.windowRef.close(statusCode);
}

ngOnInit() {

  // this.getLookups();

  if(this.passedMediaId){
    this.showImage(this.passedMediaId);
  }

}



public showImage(id:number): void {

  this.loadingSpinner = true;
//   this.fileApiService
//   .downloadTaskFileUsingGet(id)
//   .pipe(takeUntil(this.ngUnsubscribe))
//   .subscribe((response : any) => {

//     if (response.type === HttpEventType.UploadProgress)
//               this.progress = Math.round((100 * response.loaded) / response.total);
//     else if (response.type === HttpEventType.Response) {
    
//       const reader = new FileReader();
//       reader.onloadend = () => {
//         this.fileData = reader.result;
//       };
//       reader.readAsDataURL(response.body);
      
//       this.loadingSpinner = false;
           
//           }
//  },
//  (erorr:any) => {
//   this.loadingSpinner = false;
//  });


}


ngOnDestroy() {
  this.subs.forEach((s) => s.unsubscribe());
  this.ngUnsubscribe.next();
  this.ngUnsubscribe.complete();
}


}
