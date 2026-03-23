---
title: Keep Reducers Pure
impact: HIGH
impactDescription: Predictable state, testable, time-travel debugging
tags: ngrx, state, reducers
---

## Keep Reducers Pure

Reducers must be pure functions: no side effects, no HTTP calls, no subscriptions. Only compute new state from action and current state. Move side effects to Effects.

```typescript
on(UsersActions.load, state => ({ ...state, loading: true }));
// HTTP call goes in UsersEffects
```
