import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { StorageService } from '../../@core/utils.ts/index';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from '../../../environments/environment';
import { AuthUserModel } from '@app/@auth/models/auth-user.model';
import { UserRole } from '../models/user-role-response';


// make api call  to get roles permissions by taking multiples role ids to replace static permissions
//store user permissions from api in storage 
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  readonly accessTokenKeyword = 'access_token';
  readonly rolePermissionsKey:string = "role-permissions";
  readonly selectedRoleKey:string = "selected-role";

  private apiURL = `${environment.apiBaseUrl}/Authentication/Login`;
  private isAuthenticated = new BehaviorSubject<boolean>(this.hasToken());
  // private userInfo = new BehaviorSubject<any>(null);
  // private user: UserModel | null = null;
  private userSubject = new BehaviorSubject<AuthUserModel | null>(null);

  constructor(private http: HttpClient, private router: Router, private storageService: StorageService,
    private jwtHelper: JwtHelperService
  ) {
    this.initializeUserFromStorage(); // Restore state on service initialization
  }

  /**
  * Logs in the user with the provided credentials.
  * @param credentials Object containing `Username` and `Password`.
  * @returns Observable for the API response.
  */
  login(credentials: { Username: string; Password: string }): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http.post<any>(this.apiURL, credentials, { headers }).pipe(
      tap((response) => {
        if (response?.data?.accessToken) {
          // Store session data and update authentication state
          this.setSession(response.data);
          this.initializeUserInfo();
          this.isAuthenticated.next(true);
        } else {
          throw new Error('Invalid login response: accessToken is missing.');
        }
        // .shareReplay();
      }),
      catchError((error) => {
        console.error('Login failed:', error);
        throw error; // Re-throw the error to be handled by the caller
      })
    );
  }

  logout(): void {
    // permissionsService.flushPermissions();
    localStorage.removeItem(this.accessTokenKeyword);
    localStorage.removeItem('refreshToken');
    localStorage.clear();

    // this.userInfo.next(null);
    // this.isAuthenticated.next(null);
    // this.router.navigate(['/auth/login']);
  }

  private setSession(authResult: any): void {
    this.storageService.secureStorage.setItem(this.accessTokenKeyword, authResult.accessToken);
    this.storageService.secureStorage.setItem('refreshToken', authResult.refreshToken);
  }

  private hasToken(): boolean {
    return !!this.storageService.secureStorage.getItem(this.accessTokenKeyword);
  }

  getAuthStatus(): Observable<boolean> {
    return this.isAuthenticated.asObservable();
  }

  getAccessToken(): string | null {
    return this.storageService.secureStorage.getItem(this.accessTokenKeyword);
  }

  decodeToken() {
    const token = this.getAccessToken();
    return this.jwtHelper.decodeToken(token as string);
  }

  isTokenExpired() {
    const token = this.getAccessToken();
    if (token)
      return this.jwtHelper.isTokenExpired(token)

    return true
  }

  getUser(): AuthUserModel | null {

    // if(!this.userSubject.getValue() && this.userSubject.getValue.length == 0 ){
    //   this.initializeUserInfo();
    // }

    return this.userSubject.getValue(); // Direct access to current value
  }

  getUserObservable(): Observable<AuthUserModel | null> {
    return this.userSubject.asObservable(); // Reactive access
  }

  initializeUserInfo(): void {
    const decodedToken = this.decodeToken();
    if (decodedToken) {
      const user = {
        id: decodedToken.nameid,
        firstName: decodedToken.unique_name,
        username: decodedToken.unique_name,
        lastName: decodedToken.family_name,
        fullName: `${decodedToken.unique_name}_${decodedToken.family_name}`,
        email: decodedToken.email,
        mobile: decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone"],
        roles: decodedToken.role
      };

      if(user.roles && user.roles.length > 0){
        this.setSelectedRole(user.roles[0])
      }
      this.userSubject.next(user);
      // this.user = user;
      // this.userInfo.next(this.user);
    }
    else {
      // this.userInfo.next(null);
      // this.user = null;
      this.userSubject.next(null);

    }
  }

  doesUserHasRquiredRoles(requiredRoles: string[]): boolean {
    const user = this.getUser();
    if (user && user.roles) {
      return requiredRoles.every(role => user.roles.includes(role));
    }
    return false;
  }

  initializeUserFromStorage(): void {
    const token = this.getAccessToken();
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const user = this.decodeTokenToUser();
      this.userSubject.next(user);
    } else {
      this.logout(); // Clear any invalid/expired token
    }
  }

  private decodeTokenToUser(): AuthUserModel | null {
    const decoded = this.decodeToken();
    if (decoded) {
      return {
        id: decoded.nameid,
        firstName: decoded.unique_name,
        username: decoded.unique_name,
        lastName: decoded.family_name,
        fullName: `${decoded.unique_name}_${decoded.family_name}`,
        email: decoded.email,
        mobile: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone"],
        roles: decoded.role || [],
      };
    }
    return null;
  }


  setSelectedRole(roleId: any){
    this.storageService.secureStorage.setItem(this.selectedRoleKey, roleId);
  }
  
  getSelectedRole(): number{
    const selectedRole = this.storageService.secureStorage.getItem(this.selectedRoleKey);

    if(selectedRole){
      return parseInt(selectedRole);
    }
    else{
      return -1;
    }
  
  }
  
  setUserPermissions(permissions: string[]){
    this.storageService.secureStorage.setItem(this.rolePermissionsKey, JSON.stringify(permissions));
  }
    
  getUserPermissions(): string[]{

    if(!this.storageService.secureStorage.getItem(this.rolePermissionsKey))
      return ;

    const rolePermissions = JSON.parse(this.storageService.secureStorage.getItem(this.rolePermissionsKey));

    if(rolePermissions)
    {
        return rolePermissions as string[];
    }
    return [];
  }
  
}
