---
title: Use Discriminated Unions
impact: HIGH
impactDescription: Prevents impossible states
tags: typescript, types, unions
---

## Use Discriminated Unions

Model mutually exclusive states with a discriminant property to prevent impossible states at compile time.

```typescript
type State<T> =
  | { status: 'idle' }
  | { status: 'loading' }
  | { status: 'success'; data: T }
  | { status: 'error'; error: Error };
```
