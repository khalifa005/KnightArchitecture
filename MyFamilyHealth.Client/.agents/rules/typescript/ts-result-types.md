---
title: Use Result Types Instead of Throwing
impact: MEDIUM
impactDescription: Explicit error handling, type-safe
tags: typescript, errors, result-type
---

## Use Result Types Instead of Throwing

For operations that can fail, return a Result type instead of throwing. Makes error handling explicit and type-safe.

```typescript
type Result<T> = { ok: true; value: T } | { ok: false; error: Error };
function parseJson(input: string): Result<unknown> {
  try { return { ok: true, value: JSON.parse(input) }; }
  catch (e) { return { ok: false, error: e as Error }; }
}
```
