---
title: Use Functional Route Resolvers
impact: MEDIUM
impactDescription: Preloads data before navigation
tags: routing, resolvers, data
---

## Use Functional Route Resolvers

Use functional `ResolveFn<T>` resolvers to preload data before component renders, avoiding empty state flicker.

```typescript
export const userResolver: ResolveFn<User | null> = (route) =>
  inject(UserService).getById(route.paramMap.get('id')!).pipe(catchError(() => of(null)));
```
