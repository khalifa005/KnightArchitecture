---
title: Defer Await Until Needed
impact: HIGH
impactDescription: avoids blocking unused code paths
tags: async, await, conditional, optimization
---

## Defer Await Until Needed

Move `await` into branches where needed. Check cheapest conditions first (local state) before making expensive async calls.

```typescript
async function handleRequest(userId: string, skipProcessing: boolean) {
  if (skipProcessing) return { skipped: true }; // Fast path - no await
  const userData = await fetchUserData(userId);
  return processUserData(userData);
}
```
