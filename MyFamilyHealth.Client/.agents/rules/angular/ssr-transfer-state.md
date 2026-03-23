---
title: Use TransferState to Avoid Refetch
impact: HIGH
impactDescription: Eliminates duplicate API calls
tags: ssr, hydration, caching
---

## Use TransferState to Avoid Refetch

Use `TransferState` to pass server-fetched data to the client, avoiding duplicate API requests during hydration.

```typescript
data = this.transferState.hasKey(DATA_KEY)
  ? this.transferState.get(DATA_KEY, null)
  : this.http.get('/api/data').pipe(
      tap(d => this.transferState.set(DATA_KEY, d))
    );
```
