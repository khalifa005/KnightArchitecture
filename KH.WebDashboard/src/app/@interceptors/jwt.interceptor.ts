import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../@auth/services/custom-auth-service';

@Injectable()
export class CustomJwtInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService,
    // private i18nService: I18nService
   
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    let token = this.authService.getAccessToken();

    if (token) {
      // Check if the token is expired
      if (this.authService.isTokenExpired()) {
        console.warn('JWT token has expired. You may need to refresh it.');
        // Optionally handle token refresh here, or logout user if refresh is not implemented
        this.authService.logout();
        return next.handle(request); // Proceed without a token
      }

      // Decode token to extract user details
      const userDataFromToken = this.authService.getUser();
      const userId = userDataFromToken.id; // Extract user ID
    //   const userLang = this.i18nService.language; // Extract language from token later on
      const userLang = "en"; // Extract language from token later on

      // Clone the request to include additional headers
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
          'User-ID': userId || '', // Add user ID
          'Accept-Language': userLang, // Add user language
        },
      });
    }

    return next.handle(request);
  }
}
