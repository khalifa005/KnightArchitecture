import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/base/response/custom-api-response';

@Injectable({
  providedIn: 'root'
})
export class FileApiService  {

baseApiURL = environment.apiBaseUrl;

constructor(private http: HttpClient) {}

httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
})};


deleteFormlyFileById(fileId:string, FBFID:string): Observable<ApiResponse<string>> {
  const url = `${this.baseApiURL}/Attachment/DeleteAttachment`;

  // Set the query parameters
  const params = new HttpParams()
    .set('ID', fileId)
    .set('FBFID', FBFID);

  // Set the headers if necessary
  const headers = new HttpHeaders()
    .set('Content-Type', 'application/json');

  // Set any other options you need, such as responseType

  const options = {
    headers: headers,
    params: params,
    // Add more options as needed
  };

  return this.http.get<ApiResponse<string>>(url, options);
}

saveAttachment(formData:any): Observable<any> {
  return this.http
    .post<any>(
      this.baseApiURL + '/Attachment/SaveAttachment',
      formData
    );
}

}
