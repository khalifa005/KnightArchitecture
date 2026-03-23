---
title: Use Custom Preloading Strategy
impact: MEDIUM
impactDescription: Selective route preloading
tags: routing, preloading, performance
---

## Use Custom Preloading Strategy

Implement a selective preloading strategy instead of `PreloadAllModules` to control which lazy routes are preloaded based on route data flags.

**Incorrect:**

```typescript
// Preloads every lazy route — wastes bandwidth on rarely visited routes
provideRouter(routes, withPreloading(PreloadAllModules))
```

**Correct:**

```typescript
export class SelectivePreload extends PreloadingStrategy {
  preload = (route: Route, load: () => Observable<any>) =>
    route.data?.['preload'] ? load() : of(null);
}
// Route: { path: 'dashboard', loadComponent: ..., data: { preload: true } }
```
