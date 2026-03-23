---
title: Use Factory Providers for Complex Setup
impact: MEDIUM
impactDescription: Conditional service creation
tags: dependency-injection, factory, providers
---

## Use Factory Providers for Complex Setup

Use `useFactory` with `inject()` for conditional service creation based on environment or runtime dependencies.

**Incorrect:**

```typescript
// Conditional logic in service constructor couples to environment
@Injectable({ providedIn: 'root' })
export class LogService {
  constructor() { if (environment.production) { /* ... */ } }
}
```

**Correct:**

```typescript
{ provide: LogService, useFactory: () => {
    const env = inject(ENVIRONMENT);
    return env.production ? new ProdLogService() : new DevLogService();
  },
}
```
