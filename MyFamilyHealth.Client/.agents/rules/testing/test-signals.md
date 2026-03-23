---
title: Test Signals and Computed Values
impact: HIGH
impactDescription: Verifies reactive state logic
tags: testing, signals, computed, effect
---

## Test Signals and Computed Values

Test signals by setting values and asserting computed results. Use `TestBed.flushEffects()` to trigger pending effects. Wrap signal reads in `TestBed.runInInjectionContext` when needed.

**Incorrect:**

```typescript
// Testing implementation details instead of behavior
expect(component['_count']).toBe(1);
```

**Correct:**

```typescript
component.count.set(5);
expect(component.doubled()).toBe(10);
TestBed.flushEffects(); // Flush pending effects if needed
```
