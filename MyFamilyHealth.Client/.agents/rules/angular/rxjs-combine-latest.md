---
title: Use combineLatest for Multiple Streams
impact: MEDIUM
impactDescription: Combines dependent data
tags: rxjs, operators, composition
---

## Use combineLatest for Multiple Streams

Use `combineLatest({ user: user$, permissions: perms$ })` to combine multiple streams instead of nested subscribes. Emits when any source emits, providing the latest value from each.

```typescript
vm$ = combineLatest({
  user: this.user$,
  permissions: this.permissions$,
  settings: this.settings$
});
```
