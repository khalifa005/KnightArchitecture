---
title: Promise.all() for Independent Operations
impact: CRITICAL
impactDescription: 2-10x improvement, eliminates waterfalls
tags: async, parallelization, promises, performance
---

## Promise.all() for Independent Operations

Execute independent async operations concurrently instead of sequentially. Use `forkJoin()` for RxJS, `Promise.allSettled()` for partial failures.

```typescript
const [user, posts] = await Promise.all([fetchUser(id), fetchPosts(id)]);
```
