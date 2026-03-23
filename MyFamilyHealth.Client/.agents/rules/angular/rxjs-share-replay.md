---
title: Use shareReplay for Multicasting
impact: MEDIUM
impactDescription: Avoids duplicate HTTP calls
tags: rxjs, caching, multicasting
---

## Use shareReplay for Multicasting

Use `shareReplay({ bufferSize: 1, refCount: true })` to share results among multiple subscribers and avoid duplicate HTTP requests.

```typescript
users$ = this.http.get<User[]>('/api/users').pipe(
  shareReplay({ bufferSize: 1, refCount: true })
);
```
