---
title: Use Functional HTTP Interceptors
impact: HIGH
impactDescription: Simpler, composable request handling
tags: http, interceptors, functional
---

## Use Functional HTTP Interceptors

Write interceptors as `HttpInterceptorFn` functions and register with `withInterceptors()` instead of class-based `HTTP_INTERCEPTORS`.

**Incorrect:**

```typescript
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler) { /* ... */ }
}
// { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
```

**Correct:**

```typescript
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = inject(AuthService).getToken();
  return next(req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }));
};
// provideHttpClient(withInterceptors([authInterceptor]))
```
