---
title: Pre-build Lookup Maps from Arrays
impact: MEDIUM
impactDescription: O(n²) to O(n) for repeated lookups
tags: data-structures, map, indexing, performance
---

## Pre-build Lookup Maps from Arrays

When looking up items multiple times, build a Map first. Converts O(n) lookups to O(1).

```typescript
const userMap = new Map(users.map(u => [u.id, u]));
return orders.map(order => ({
  ...order,
  user: userMap.get(order.userId),
}));
```
