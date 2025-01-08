import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpResponse,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastNotificationService } from '../@core/utils.ts/toast-notification.service';
import { NotitficationsDefaultValues } from '../@core/const/notitfications-default-values';

@Injectable()
export class ResponseInterceptor implements HttpInterceptor {
  constructor(
    private router: Router,
    private toastNotificationService: ToastNotificationService,
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      map((event: HttpEvent<any>) => {
        if (event instanceof HttpResponse) {
          const body = event.body;

          // event.body.success === false

          if (body && body.statusCode && typeof body === 'object') {
            if (body.statusCode === 200) {
              // Return the data object for successful responses
              return event;
            } 
            else {
              // Handle API-specific errors
              this.handleApiError(body);
              throw new Error(body.errorMessage || 'Unknown API Error');
            }
          }
        }
        return event;
      }),
      catchError((error: HttpErrorResponse) => {
        this.handleHttpError(error);
        return throwError(() => error); // Re-throw the error
      })
    );
  }

  private handleApiError(apiError: any): void {
    const { errorMessage, errorMessageAr, errors } = apiError;

    // Determine the language
    const isArabic = true;

    // Select the appropriate message
    const userMessage = isArabic
      ? errorMessageAr || 'حدث خطأ غير متوقع.'
      : errorMessage || 'An unexpected error occurred.';

    console.error('API Error:', apiError);

    // Show toast notification with the error message
    this.toastNotificationService.showToast(
      NotitficationsDefaultValues.Danger,
      isArabic ? 'خطأ في واجهة برمجة التطبيقات' : 'API Error',
      userMessage
    );
  }

  private handleHttpError(error: HttpErrorResponse): void {
    // const isArabic = this.i18nService.language === LanguageEnum.Ar;
    const isArabic = true;

    let userMessage = isArabic
      ? 'حدث خطأ غير متوقع. يرجى المحاولة مرة أخرى لاحقًا.'
      : 'An unexpected error occurred. Please try again later.';

    // Handle specific HTTP status codes
    switch (error.status) {
      case 0: // Network error or CORS issue
        userMessage = isArabic
          ? 'حدث خطأ في الشبكة. يرجى التحقق من الاتصال.'
          : 'A network error occurred. Please check your connection.';
        break;

      case 401: // Unauthorized
        userMessage = isArabic
          ? 'انتهت صلاحية الجلسة الخاصة بك. يرجى تسجيل الدخول مرة أخرى.'
          : 'Your session has expired. Please log in again.';
        this.router.navigate(['/auth/login']);
        break;

      case 403: // Forbidden
        userMessage = isArabic
          ? 'ليس لديك إذن للوصول إلى هذا المورد.'
          : 'You do not have permission to access this resource.';
        break;

      case 404: // Not Found
        userMessage = isArabic
          ? 'المورد المطلوب غير موجود.'
          : 'The requested resource was not found.';
        break;

      case 500: // Internal Server Error
        userMessage = isArabic
          ? 'حدث خطأ داخلي في الخادم. يرجى المحاولة مرة أخرى لاحقًا.'
          : 'An internal server error occurred. Please try again later.';
        break;

      default:
        userMessage = isArabic
          ? `حدث خطأ: ${error.status} ${error.statusText || 'غير معروف'}`
          : `An error occurred: ${error.status} ${error.statusText || 'Unknown'}`;
        break;
    }

    console.error('HTTP Error:', error);

    let errorMessage: string = userMessage;
    let errorMessageAr: string = userMessage;

    if (error.error) {
      //override withr esponse custom messages to be specific
      errorMessage = error.error.errorMessage || errorMessage;
      errorMessageAr = error.error.errorMessageAr || '';
    }

    // Show toast notification with the error message
    this.toastNotificationService.showToast(
      NotitficationsDefaultValues.Danger,
      isArabic ? 'خطأ ' : ' Error',
      errorMessage
    );
  }
}
