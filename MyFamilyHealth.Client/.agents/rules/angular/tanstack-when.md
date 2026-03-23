---
title: Know When to Use TanStack Query
impact: MEDIUM
impactDescription: Right tool for the job
tags: state, tanstack, architecture
---

## Know When to Use TanStack Query

Use TanStack Query for server state (data from APIs); use signals for client state (UI state like form dirty, modal open).

```typescript
// Server state → TanStack Query (caching, refetching)
users = injectQuery(() => ({ queryKey: ['users'], queryFn: fetchUsers }));
// Client state → Signals (no caching needed)
formDirty = signal(false);
```
