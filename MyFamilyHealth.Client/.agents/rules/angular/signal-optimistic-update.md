---
title: Use Optimistic Updates with Signals
impact: MEDIUM
impactDescription: Instant UI feedback
tags: signals, optimistic, ux
---

## Use Optimistic Updates with Signals

Update signal state immediately before API calls and rollback on error to provide instant UI feedback.

**Incorrect:**

```typescript
async delete(id: string) {
  await firstValueFrom(this.http.delete(`/api/items/${id}`));
  this.items.update(items => items.filter(i => i.id !== id));
}
```

**Correct:**

```typescript
delete(id: string) {
  const prev = this.items();
  this.items.update(items => items.filter(i => i.id !== id));
  this.http.delete(`/api/items/${id}`).subscribe({ error: () => this.items.set(prev) });
}
```
