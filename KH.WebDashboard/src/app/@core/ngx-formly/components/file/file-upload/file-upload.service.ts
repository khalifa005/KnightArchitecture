import { HttpClient, HttpErrorResponse, HttpEvent, HttpEventType, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, filter, map } from 'rxjs/operators';
import { FileUploadState } from './file-upload-state';

@Injectable()
export class FileUploadService {

  constructor(private readonly http: HttpClient) { }

  public upload(taskCode: string,
    FBFId: string,
    fieldId: string,
    file: File,
    url: string,
    paramName = 'file'): Observable<FileUploadState> {
    const request: HttpRequest<FormData> = this.createRequest(taskCode, FBFId, fieldId, file, url, paramName);

    return this.http.request(request).pipe(
      filter(this.isSupportedEvent),
      map(this.createFileUploadState),
      catchError((error: HttpErrorResponse) => {
        throw error.statusText;
      })
    );
  }

  private createRequest(
    taskCode: string,
    FBFId: string,
    fieldId: string,
    file: File,
    url: string,
    paramName: string): HttpRequest<FormData> {
    const formData: FormData = new FormData();
    formData.append(paramName, file, file.name);

    if (taskCode) {
      formData.append("taskCode", taskCode);
    }

    if (fieldId) {
      formData.append("fieldId", fieldId);
    }

    if (FBFId) {
      formData.append("FBFId", FBFId);
    }

    return new HttpRequest('POST', url, formData, {
      reportProgress: true
    });
  }

  private isSupportedEvent(event: HttpEvent<unknown>): boolean {
    return event.type === HttpEventType.UploadProgress || event.type === HttpEventType.Response;
  }

  private createFileUploadState(event: HttpEvent<unknown>): FileUploadState {
    if (event.type === HttpEventType.UploadProgress && event.total) {
      const percentDone = Math.round(100 * event.loaded / event.total);
      return { progress: percentDone };
    }
    // /data - because api response
    if (event.type === HttpEventType.Response) {
      if (event.ok) {
        const body: any = event.body; // Cast to any to access the response body
        return {
          progress: 100,
          location: event.headers.get('Location') as string,
          filePath: body.data?.filePath, // Add this line to extract the file path
          fileId: body.data?.fileId
        };
      } else {
        throw event.statusText;
      }
    }

    throw 'upload error';
  }

}
