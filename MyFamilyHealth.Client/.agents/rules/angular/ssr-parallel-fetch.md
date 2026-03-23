---
title: Fetch Data in Parallel on Server
impact: HIGH
impactDescription: Reduces SSR response time by parallelization
tags: ssr, async, performance
---

## Fetch Data in Parallel on Server

Use `Promise.all()` or `forkJoin()` to fetch independent data sources concurrently during SSR to reduce total response time.

```typescript
async resolve() {
  const [user, posts] = await Promise.all([
    this.userService.get(),   // 200ms
    this.postService.get(),   // 300ms
  ]);
  return { user, posts };     // Total: 300ms (parallel, not 500ms)
}
```
