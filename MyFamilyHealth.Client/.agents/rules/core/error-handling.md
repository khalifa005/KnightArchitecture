---
title: Error Handling Patterns
impact: MEDIUM
impactDescription: Resilience and user feedback
tags: errors, resilience, interceptors
---

## Error Handling Patterns

Use global `ErrorHandler` for unhandled exceptions. Centralize HTTP errors (401, 403, 500) in an interceptor. Use `retry({ count: 3, delay: 1000 })` for idempotent requests.

```typescript
export const errorInterceptor: HttpInterceptorFn = (req, next) =>
  next(req).pipe(catchError(err => {
    if (err.status === 401) inject(Router).navigate(['/login']);
    return throwError(() => err);
  }));
```
