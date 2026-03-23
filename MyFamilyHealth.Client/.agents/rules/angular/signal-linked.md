---
title: Use linkedSignal() for Dependent Resettable State
impact: HIGH
impactDescription: Auto-resets derived state
tags: signals, linkedSignal, derived-state
---

## Use linkedSignal() for Dependent Resettable State

Use `linkedSignal()` when a writable signal's value should reset automatically when a source signal changes.

**Incorrect:**

```typescript
options = signal(['Ground', 'Air', 'Sea']);
selected = signal(this.options()[0]);
constructor() { effect(() => this.selected.set(this.options()[0])); }
```

**Correct:**

```typescript
options = signal(['Ground', 'Air', 'Sea']);
selected = linkedSignal(() => this.options()[0]);
// Writable: selected.set('Air') — auto-resets when options change
```
