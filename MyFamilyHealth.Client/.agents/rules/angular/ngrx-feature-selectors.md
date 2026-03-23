---
title: Use Feature Selectors
impact: MEDIUM
impactDescription: Memoized selection, better performance
tags: ngrx, state, selectors
---

## Use Feature Selectors

Use `createFeatureSelector` and `createSelector` for memoized state selection. Selectors only recompute when their inputs change.

```typescript
const selectCounterState = createFeatureSelector<CounterState>('counter');
export const selectCount = createSelector(selectCounterState, s => s.count);
```
