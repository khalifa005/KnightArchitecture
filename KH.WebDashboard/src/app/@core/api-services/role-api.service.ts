import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { StorageService } from '../utils.ts/index';
import { environment } from '../../../environments/environment';
import { ApiResponse } from '../models/base/response/custom-api-response';
import { PermissionResponse } from '../models/responses/permission-response';
import { LookupResponse } from '../models/base/response/lookup.model';
import { RoleFilterRequest } from '../models/requests/role-filter-request';
import { PagedResponse } from '../models/base/response/custom-paged-list-response.model';
import { CraeteLookupRequest } from '../models/base/request/create-lookup.request';
import { RolesResponse } from '../models/responses/roles-response';
import { UpdateRolePermissionsRequest } from '../models/requests/update-role-permissions-request';

@Injectable({
  providedIn: 'root',
})
export class RoleApiService {
  private readonly apiUrl = `${environment.apiBaseUrl}/roles`;
  private readonly jsonHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });

  constructor(private http: HttpClient, private storageService: StorageService) {}

  getRole(id: number): Observable<ApiResponse<RolesResponse>> {
    return this.http.get<ApiResponse<RolesResponse>>(`${this.apiUrl}/${id}`);
  }

  listAllRoles(request: RoleFilterRequest): Observable<ApiResponse<RolesResponse[]>> {
    return this.http.post<ApiResponse<RolesResponse[]>>(`${this.apiUrl}/ListAll`, request);
  }

  getPagedRoles(request: RoleFilterRequest): Observable<ApiResponse<PagedResponse<LookupResponse>>> {
    return this.http.post<any>(`${this.apiUrl}/PagedList`, request);
  }

  addRole(request: CraeteLookupRequest): Observable<ApiResponse<string>> {
    return this.http.post<ApiResponse<string>>(this.apiUrl, request);
  }

  updateRole(request: CraeteLookupRequest): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(this.apiUrl, request);
  }

  reActivateRole(id: number): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(`${this.apiUrl}/ReActivate/${id}`, {});
  }
  

  updateRoleWithPermissions(request: CraeteLookupRequest): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(`${this.apiUrl}/UpdateBothRoleWithRelatedPermissions`, request);
  }
  
  UpdateRolePermissions(request: UpdateRolePermissionsRequest): Observable<ApiResponse<string>> {
    return this.http.put<ApiResponse<string>>(`${this.apiUrl}/UpdateRolePermissions`, request);
  }

  deleteRole(id: number): Observable<ApiResponse<string>> {
    return this.http.delete<ApiResponse<string>>(`${this.apiUrl}/${id}`);
  }
}


