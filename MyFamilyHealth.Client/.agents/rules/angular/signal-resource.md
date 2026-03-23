---
title: Use resource() for Async Data Loading
impact: HIGH
impactDescription: Built-in loading/error/cancellation
tags: signals, resource, async
---

## Use resource() for Async Data Loading

Use the `resource()` API for generic async operations with built-in loading, error, and cancellation state via `ResourceRef`.

**Incorrect:**

```typescript
data = signal<User | undefined>(undefined);
loading = signal(false);
error = signal<Error | null>(null);
loadUser() { this.loading.set(true); fetch(`/api/users/${this.id()}`).then(r => r.json()).then(d => this.data.set(d)).finally(() => this.loading.set(false)); }
```

**Correct:**

```typescript
userResource = resource({
  params: () => ({ id: this.userId() }),
  loader: ({ params }) => fetch(`/api/users/${params.id}`).then(r => r.json()),
});
// Access: userResource.value(), userResource.isLoading(), userResource.error()
```
