---
title: Handle noUncheckedIndexedAccess
impact: LOW
impactDescription: Safer array/object access
tags: typescript, tsconfig, safety
---

## Handle noUncheckedIndexedAccess

With `noUncheckedIndexedAccess`, array/object indexing returns `T | undefined`. Always check before using.

```typescript
const first = users[0];
if (first) { console.log(first.name); }
// Or: console.log(first?.name ?? 'Unknown');
```
