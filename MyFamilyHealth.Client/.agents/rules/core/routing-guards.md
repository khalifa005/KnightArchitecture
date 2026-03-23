---
title: Use Functional Route Guards
impact: HIGH
impactDescription: Protects routes, cleaner than class guards
tags: routing, guards, auth
---

## Use Functional Route Guards

Use functional `CanActivateFn` guards with `inject()` instead of class-based guards. Return `true`, `false`, or `UrlTree` for redirects.

```typescript
export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  const router = inject(Router);
  return auth.isAuthenticated() || router.createUrlTree(['/login']);
};
// Usage: { path: 'dashboard', canActivate: [authGuard] }
```
