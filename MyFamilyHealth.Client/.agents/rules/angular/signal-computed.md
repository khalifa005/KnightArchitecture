---
title: Use Computed for Derived State
impact: HIGH
impactDescription: Memoized derivation, prevents redundant calculations
tags: signals, state, performance
---

## Use Computed for Derived State

Use `computed()` instead of getters for derived state. Getters re-run on every change detection; computed signals are memoized and only recalculate when dependencies change.

```typescript
// Memoized; only runs when firstName or lastName changes
fullName = computed(() => `${this.firstName()} ${this.lastName()}`);
```
