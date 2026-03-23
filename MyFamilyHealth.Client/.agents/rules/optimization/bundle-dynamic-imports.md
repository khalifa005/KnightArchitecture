---
title: Use @defer for Lazy Loading
impact: HIGH
impactDescription: reduces initial bundle size
tags: bundle, lazy-loading, dynamic-import, defer
---

## Use @defer for Lazy Loading

Use `@defer` for component-level lazy loading. Triggers: `on viewport`, `on interaction`, `on idle`, `on timer(ms)`, `when condition()`.

```html
@defer (on viewport) {
  <app-heavy-chart [data]="chartData()" />
} @placeholder {
  <div class="chart-skeleton"></div>
} @loading (minimum 200ms) {
  <app-spinner />
}
```
