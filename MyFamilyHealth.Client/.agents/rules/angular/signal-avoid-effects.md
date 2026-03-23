---
title: Avoid Effects for State Propagation
impact: MEDIUM
impactDescription: Prevents circular dependencies, cleaner data flow
tags: signals, effects, state
---

## Avoid Effects for State Propagation

Use `computed()` for derived state, not `effect()` with `.set()`. Effects are for external side effects (logging, DOM manipulation, analytics), not for propagating state between signals.

```typescript
// Computed handles derivation without circular dependency risks
fullName = computed(() => `${this.firstName()} ${this.lastName()}`);
```
