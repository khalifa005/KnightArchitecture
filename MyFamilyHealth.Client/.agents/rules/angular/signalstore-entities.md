---
title: Use withEntities for Collections
impact: MEDIUM
impactDescription: O(1) lookups, standardized CRUD
tags: state, signalstore, entities
---

## Use withEntities for Collections

Use `withEntities<T>()` from `@ngrx/signals/entities` for collection management with O(1) lookups and standardized CRUD operations.

```typescript
withEntities<User>(),
withMethods((store) => ({
  update(id: string, changes: Partial<User>) { patchState(store, updateEntity({ id, changes })); },
}))
```
