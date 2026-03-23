---
title: When any is Acceptable in Generics
impact: LOW
impactDescription: Practical type safety in complex generics
tags: typescript, any, generics
---

## When any is Acceptable in Generics

Use `as any` inside generic function bodies when TypeScript can't verify conditional types. The public API remains typed; internal casts don't leak.

```typescript
const flip = <T extends 'a' | 'b'>(x: T): T extends 'a' ? 'b' : 'a' => {
  return (x === 'a' ? 'b' : 'a') as any; // OK - public API is still typed
};
```
