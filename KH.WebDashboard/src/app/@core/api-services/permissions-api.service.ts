import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { StorageService } from '../utils.ts/index';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/base/response/custom-api-response';
import { LookupResponse } from '../models/base/response/lookup.model';
import { RoleFilterRequest } from '../models/requests/role-filter-request';
import { PagedResponse } from '../models/base/response/custom-paged-list-response.model';
import { CraeteLookupRequest } from '../models/base/request/create-lookup.request';
import { RolesResponse } from '../models/responses/roles-response';
import { PermissionResponse } from '../models/responses/permission-response';
import { PermissionFilterRequest } from '../models/requests/permission-filter-request';

@Injectable({
  providedIn: 'root',
})
export class PermissionsApiService {
  private readonly apiUrl = `${environment.apiBaseUrl}/Permissions`;
  private readonly jsonHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private http: HttpClient, private storageService: StorageService) {}

  getPermissionById(id: number): Observable<ApiResponse<PermissionResponse>> {
    return this.http.get<ApiResponse<PermissionResponse>>(`${this.apiUrl}/${id}`);
  }

  listAllPermissions(): Observable<ApiResponse<PermissionResponse[]>> {
    return this.http.get<ApiResponse<PermissionResponse[]>>(`${this.apiUrl}/ListAll`);
  }

  getPagedPermissions(filterRequest: PermissionFilterRequest): Observable<ApiResponse<PagedResponse<PermissionResponse>>> {
    return this.http.post<ApiResponse<PagedResponse<PermissionResponse>>>(
      `${this.apiUrl}/PagedList`,
      filterRequest
    );
  }

  // createPermission(request: PermissionResponse): Observable<ApiResponse<string>> {
  //   return this.http.post<ApiResponse<string>>(`${this.apiUrl}`, request);
  // }

  updatePermission(request: PermissionResponse): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(`${this.apiUrl}`, request);
  }

  deletePermission(id: number): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/${id}`);
  }
}


