---
title: Enable Client Hydration
impact: HIGH
impactDescription: Preserves server-rendered DOM
tags: ssr, hydration, performance
---

## Enable Client Hydration

Enable `provideClientHydration(withEventReplay())` to reuse server-rendered DOM instead of destroying and rebuilding it on the client.

```typescript
bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideClientHydration(withEventReplay())
  ]
});
```
