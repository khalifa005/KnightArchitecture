---
title: Use InjectionToken with Factory for Config
impact: MEDIUM
impactDescription: Type-safe, tree-shakeable config
tags: dependency-injection, tokens, config
---

## Use InjectionToken with Factory for Config

Create self-providing `InjectionToken` instances with `factory` for browser globals and app configuration instead of string tokens or direct global access.

**Incorrect:**

```typescript
// String tokens lose type safety and aren't tree-shakeable
constructor(@Inject('API_URL') private apiUrl: string) {}
const el = window.document.getElementById('app');
```

**Correct:**

```typescript
export const API_URL = new InjectionToken<string>('API_URL', {
  factory: () => environment.apiUrl,
});
// Usage: private apiUrl = inject(API_URL);
```
