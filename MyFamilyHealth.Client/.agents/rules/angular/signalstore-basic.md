---
title: Use SignalStore for Shared State
impact: HIGH
impactDescription: Simpler than NgRx, signal-based reactivity
tags: state, signalstore, ngrx-signals
---

## Use SignalStore for Shared State

Use `signalStore()` from `@ngrx/signals` for shared state across components without full NgRx overhead. Combine `withState()`, `withComputed()`, and `withMethods()` to define state shape, derived values, and actions.

```typescript
export const UsersStore = signalStore(
  withState({ users: [] as User[], loading: false }),
  withMethods((store) => ({ load: () => patchState(store, { loading: true }) })),
);
```
