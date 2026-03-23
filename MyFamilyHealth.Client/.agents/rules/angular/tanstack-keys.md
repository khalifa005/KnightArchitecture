---
title: Use Query Key Factories
impact: MEDIUM
impactDescription: Consistent invalidation, hierarchical keys
tags: state, tanstack, query-keys
---

## Use Query Key Factories

Define query key factories for consistent key structure and easy hierarchical invalidation.

```typescript
const userKeys = {
  all: ['users'] as const,
  detail: (id: string) => ['users', 'detail', id] as const,
};
// Invalidate all user queries: queryClient.invalidateQueries({ queryKey: userKeys.all })
```
