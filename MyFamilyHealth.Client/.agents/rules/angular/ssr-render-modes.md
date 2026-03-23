---
title: Configure Server Route Render Modes
impact: HIGH
impactDescription: Per-route rendering strategy
tags: ssr, rendering, routes
---

## Configure Server Route Render Modes

Use `ServerRoute` with `RenderMode.Prerender`, `RenderMode.Server`, or `RenderMode.Client` to control rendering strategy per route.

**Incorrect:**

```typescript
const serverRoutes: ServerRoute[] = [
  { path: '**', renderMode: RenderMode.Server }
];
```

**Correct:**

```typescript
const serverRoutes: ServerRoute[] = [
  { path: '', renderMode: RenderMode.Prerender },
  { path: 'dashboard', renderMode: RenderMode.Client },
  { path: '**', renderMode: RenderMode.Server }
];
```
