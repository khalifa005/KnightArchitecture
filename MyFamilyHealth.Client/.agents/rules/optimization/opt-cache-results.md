---
title: Memoize Expensive Function Results
impact: MEDIUM
impactDescription: avoid redundant computation
tags: caching, memoization, performance, computed
---

## Memoize Expensive Function Results

Use `computed()` for automatic memoization of expensive calculations. Computed signals only recalculate when dependencies change.

```typescript
statistics = computed(() =>
  this.data().reduce((stats, point) => computeStats(stats, point), initial)
);

// Or use TanStack Query with staleTime for API caching
```
