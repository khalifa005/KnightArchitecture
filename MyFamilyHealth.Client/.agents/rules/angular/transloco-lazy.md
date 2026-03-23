---
title: Lazy Load Translation Files per Route
impact: MEDIUM
impactDescription: Reduces initial bundle with translations
tags: transloco, lazy-loading, scope, routes
---

## Lazy Load Translation Files per Route

Use Transloco scopes to load translation files only when a route is activated. Define scopes at the route level using `provideTranslocoScope()` to avoid loading all translations upfront.

**Incorrect:**

```typescript
// Single large translation file loaded at startup
// en.json — 500+ keys for all features
```

**Correct:**

```typescript
{ path: 'admin', loadChildren: () => import('./admin/admin.routes'),
  providers: [provideTranslocoScope('admin')] }
// Loads admin/en.json only when visiting /admin
```
