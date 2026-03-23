---
title: Handle Errors in Streams
impact: HIGH
impactDescription: Prevents stream termination
tags: rxjs, errors, resilience
---

## Handle Errors in Streams

Use `catchError` inside `switchMap` to handle errors without terminating the outer stream.

**Incorrect:**

```typescript
// Error terminates entire stream - no more searches work
search$ = this.term$.pipe(
  switchMap(term => this.api.search(term)),
  catchError(() => of([])) // Too late - outer stream dead
);
```

**Correct:**

```typescript
search$ = this.term$.pipe(
  switchMap(term => this.api.search(term).pipe(
    catchError(() => of([])) // Recovers per-request
  ))
);
```
