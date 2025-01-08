import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()

// not used yet
export class UtcDateInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const updatedReq = req.clone({
      body: this.convertDatesToUtc(req.body),
    });

    return next.handle(updatedReq).pipe(
      map((event: HttpEvent<any>) => {
        if (event && event['body']) {
          this.convertDatesFromUtc(event['body']);
        }
        return event;
      })
    );
  }

  private convertDatesToUtc(obj: any): any {
    if (!obj || typeof obj !== 'object') return obj;

    Object.keys(obj).forEach(key => {
      if (obj[key] instanceof Date) {
        obj[key] = obj[key].toISOString(); // Convert to UTC ISO string
      } else if (typeof obj[key] === 'object') {
        this.convertDatesToUtc(obj[key]);
      }
    });

    return obj;
  }

  private convertDatesFromUtc(obj: any): any {
    if (!obj || typeof obj !== 'object') return obj;

    Object.keys(obj).forEach(key => {
      if (typeof obj[key] === 'string' && obj[key].match(/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}.\d{3}Z$/)) {
        obj[key] = new Date(obj[key]); // Convert ISO string back to Date object
      } else if (typeof obj[key] === 'object') {
        this.convertDatesFromUtc(obj[key]);
      }
    });

    return obj;
  }
}
