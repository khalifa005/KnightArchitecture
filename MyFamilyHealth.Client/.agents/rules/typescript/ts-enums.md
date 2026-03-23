---
title: Avoid Enums, Use const Objects
impact: MEDIUM
impactDescription: Better tree-shaking, clearer behavior
tags: typescript, enums, const
---

## Avoid Enums, Use const Objects

Use `as const` objects instead of enums for predictable behavior and tree-shaking.

**Incorrect:**

```typescript
enum Direction { Up, Down } // Numeric enums create reverse mappings
Object.keys(Direction).length; // 4, not 2!
```

**Correct:**

```typescript
const Direction = { Up: 'UP', Down: 'DOWN' } as const;
type Direction = (typeof Direction)[keyof typeof Direction]; // 'UP' | 'DOWN'
```
