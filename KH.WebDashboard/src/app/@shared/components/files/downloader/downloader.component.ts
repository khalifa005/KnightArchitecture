import { HttpEventType } from '@angular/common/http';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { NotitficationsDefaultValues } from '../../../../@core/const/notitfications-default-values';
import { Logger } from '../../../../@core/utils.ts/logger.service';
import { ToastNotificationService } from '../../../../@core/utils.ts/toast-notification.service';

@Component({
  selector: 'app-downloader',
  templateUrl: './downloader.component.html',
  styleUrls: ['./downloader.component.scss']
})
export class DownloaderComponent implements OnInit , OnDestroy  {
  private log = new Logger(DownloaderComponent.name);

  @Input() public mediaId: number;

  private ngUnsubscribe: Subject<void> = new Subject<void>();

  message: string;
  progress: number;
  
  constructor( 
    private toastNotificationService:ToastNotificationService) {}
  
  ngOnInit(): void {}
  
onDownloadClicked() {

//   this.fileApiService.downloadTaskFileUsingGet(this.mediaId)
//   .pipe(takeUntil(this.ngUnsubscribe))
//   .subscribe((response : any) => {

//     if (response.type === HttpEventType.UploadProgress)
//               this.progress = Math.round((100 * response.loaded) / response.total);
//     else if (response.type === HttpEventType.Response) {
//               // this.message = 'Download success.';
//               this.toastNotificationService.showToast(NotitficationsDefaultValues.Success, 'file download ', '');
//               let downloadLink = document.createElement('a');
//               downloadLink.href = window.URL.createObjectURL(response.body);
//               downloadLink.setAttribute('download', 'file'+ this.mediaId);
//               document.body.appendChild(downloadLink);
//               downloadLink.click();
//               downloadLink.remove();
//           }
//  },
//  (erorr:any) => {
//    this.log.error(erorr);
//   //  this.toastNotificationService.showToast(NotitficationsDefaultValues.Danger, 'download service ', erorr);
//  });


 }

ngOnDestroy() {
  this.ngUnsubscribe.next();
  this.ngUnsubscribe.complete();
}

  

}
