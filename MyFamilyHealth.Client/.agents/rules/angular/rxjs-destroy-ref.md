---
title: Use DestroyRef and takeUntilDestroyed
impact: HIGH
impactDescription: No manual unsubscribe logic
tags: rxjs, destroyRef, cleanup
---

## Use DestroyRef and takeUntilDestroyed

Use `takeUntilDestroyed()` in injection context and `DestroyRef.onDestroy()` for cleanup instead of implementing `OnDestroy` with a Subject.

**Incorrect:**

```typescript
private destroy$ = new Subject<void>();
ngOnInit() { this.data$.pipe(takeUntil(this.destroy$)).subscribe(); }
ngOnDestroy() { this.destroy$.next(); this.destroy$.complete(); }
```

**Correct:**

```typescript
private destroyRef = inject(DestroyRef);
ngOnInit() { this.data$.pipe(takeUntilDestroyed(this.destroyRef)).subscribe(); }
// Outside injection context: pass DestroyRef explicitly
```
