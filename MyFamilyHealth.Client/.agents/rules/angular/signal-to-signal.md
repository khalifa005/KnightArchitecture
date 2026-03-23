---
title: Use toSignal for Observables
impact: MEDIUM
impactDescription: Cleaner templates, zoneless-ready
tags: signals, rxjs, interop
---

## Use toSignal for Observables

Use `toSignal()` to bridge RxJS observables to signals for simpler template usage. In templates, use `params()?.id` instead of `(params$ | async)?.id`.

```typescript
// Converts observable to signal with automatic subscription management
params = toSignal(this.route.params);
```
