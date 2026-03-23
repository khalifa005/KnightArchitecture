---
title: Use rxMethod for RxJS Integration
impact: MEDIUM
impactDescription: Debounce, switchMap, and other RxJS operators
tags: state, signalstore, rxjs
---

## Use rxMethod for RxJS Integration

Use `rxMethod` from `@ngrx/signals/rxjs-interop` for RxJS-based side effects like debounced search.

```typescript
searchUsers: rxMethod<string>(pipe(
  debounceTime(300), switchMap(q => http.get<User[]>(`/api/users?q=${q}`)),
  tap(users => patchState(store, { users })),
))
```
