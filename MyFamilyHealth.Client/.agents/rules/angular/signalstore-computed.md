---
title: Use withComputed for Derived State
impact: MEDIUM
impactDescription: Memoized derivation, cleaner store API
tags: state, signalstore, computed
---

## Use withComputed for Derived State

Add derived state to SignalStore using `withComputed` instead of computing in components. Keeps derivation logic centralized and memoized.

```typescript
withComputed((store) => ({
  filteredUsers: computed(() => store.users().filter(u => u.name.includes(store.filter()))),
}))
```
