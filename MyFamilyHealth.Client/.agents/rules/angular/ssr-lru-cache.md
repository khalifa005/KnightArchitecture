---
title: Use LRU Cache for SSR Computations
impact: HIGH
impactDescription: Reduces server response time 50-90%
tags: ssr, caching, performance
---

## Use LRU Cache for SSR Computations

Cache expensive server-side computations with an LRU cache and TTL to avoid recomputation on repeated requests.

```typescript
private cache = new Map<string, { data: Data; timestamp: number }>();
async renderPage(id: string) {
  const cached = this.cache.get(id);
  if (cached && Date.now() - cached.timestamp < 60000) return this.render(cached.data);
  const data = await this.expensiveComputation(id);
  this.cache.set(id, { data, timestamp: Date.now() });
  return this.render(data);
}
```
