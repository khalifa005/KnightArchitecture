---
title: When to Use JSDoc Comments
impact: LOW
impactDescription: Improves discoverability for non-obvious behavior
tags: typescript, jsdoc, documentation
---

## When to Use JSDoc Comments

Use JSDoc for `@deprecated`, `@throws`, and non-obvious behavior. Don't repeat type information already in the signature.

```typescript
/** @deprecated Use getUserById instead. Removed in v3.0. */
function fetchUser(id: string): Observable<User> { }
```
