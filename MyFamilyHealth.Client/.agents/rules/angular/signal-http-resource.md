---
title: Use httpResource() for Signal-Based Data Fetching
impact: HIGH
impactDescription: Reactive GET with auto-refetch
tags: signals, http, resource
---

## Use httpResource() for Signal-Based Data Fetching

Use `httpResource()` for reactive GET requests that auto-refetch when signal dependencies change, replacing manual HttpClient subscribe patterns.

**Incorrect:**

```typescript
loading = signal(false);
data = signal<User | null>(null);
ngOnInit() {
  this.loading.set(true);
  this.http.get<User>(`/api/users/${this.userId()}`).subscribe(u => { this.data.set(u); this.loading.set(false); });
}
```

**Correct:**

```typescript
userId = input.required<string>();
user = httpResource<User>(() => `/api/users/${this.userId()}`);
// Access: user.value(), user.isLoading(), user.error()
```
