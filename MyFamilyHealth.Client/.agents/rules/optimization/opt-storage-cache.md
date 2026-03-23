---
title: Cache API Results with Interceptors
impact: MEDIUM
impactDescription: Eliminates redundant network requests
tags: caching, performance, http
---

## Cache API Results with Interceptors

Use an HTTP interceptor or service-level caching with signals to avoid redundant API calls — prefer Angular-idiomatic patterns over raw `sessionStorage`.

```typescript
// Service-level caching with signals
private configCache = signal<Config | null>(null);
loadConfig = () => this.configCache() ?? this.http.get<Config>('/api/config').pipe(
  tap(d => this.configCache.set(d))
);
```
