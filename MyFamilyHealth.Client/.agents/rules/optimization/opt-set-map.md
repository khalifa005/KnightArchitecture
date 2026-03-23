---
title: Use Set/Map for O(1) Lookups
impact: MEDIUM
impactDescription: O(n) to O(1) lookup performance
tags: data-structures, set, map, performance
---

## Use Set/Map for O(1) Lookups

Convert arrays to Set/Map for repeated membership checks. Array `includes()` is O(n); Set `has()` is O(1). Convert when checking membership more than once or arrays have >10 items.

**Incorrect (O(n) per check):**
```typescript
const allowedIds = ['a', 'b', 'c'];
const filtered = items.filter(item => allowedIds.includes(item.id));
```

**Correct (O(1) per check):**
```typescript
const allowedIds = new Set(['a', 'b', 'c']);
const filtered = items.filter(item => allowedIds.has(item.id));
```
