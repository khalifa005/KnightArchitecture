---
title: Preload Critical Resources
impact: LOW
impactDescription: Improves perceived performance
tags: bundle, preload, prefetch, performance
---

## Preload Critical Resources

Use `withPreloading(PreloadAllModules)` for routes. Use `@defer (prefetch on idle)` for components. Use `<link rel="preload">` for critical fonts.

```html
@defer (on viewport; prefetch on idle) { <app-heavy-component /> }
@defer (on interaction; prefetch on hover) { <app-modal /> }
```
