import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { StorageService } from '../utils.ts/index';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/base/response/custom-api-response';
import { PermissionResponse } from '../models/responses/permission-response';

@Injectable({
  providedIn: 'root',
})
export class UserApiService {
  private readonly apiURL = `${environment.apiBaseUrl}/users`;
  private readonly jsonHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private http: HttpClient, private storageService: StorageService) {}

  /**
   * Fetches the user's permissions from the API.
   * @returns An observable of ApiResponse containing a list of PermissionResponse objects.
   */
  getUserPermissions(): Observable<ApiResponse<PermissionResponse[]>> {
    return this.http
      .get<ApiResponse<PermissionResponse[]>>(`${this.apiURL}/GetUserPermissions`, {
        headers: this.jsonHeaders,
      })
      .pipe(
        tap((response) => {
          if (!response?.data) {
            console.warn('No permissions data found.');
          }
        }),
        catchError((error) => {
          console.error('Failed to fetch user permissions:', error);
          return throwError(() => new Error('Failed to fetch user permissions.'));
        })
      );
  }
}
