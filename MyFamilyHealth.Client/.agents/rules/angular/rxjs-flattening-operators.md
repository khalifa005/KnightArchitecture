---
title: Choose the Right Flattening Operator
impact: HIGH
impactDescription: Prevents race conditions
tags: rxjs, operators, switchMap
---

## Choose the Right Flattening Operator

Use `switchMap` to cancel previous (search/autocomplete), `exhaustMap` to ignore new (form submit), `concatMap` for ordered sequential, `mergeMap` for parallel — choosing wrong causes race conditions or lost requests.

**Correct:**

```typescript
search$ = this.query$.pipe(switchMap(q => this.api.search(q)));       // Cancel previous
submit$ = this.click$.pipe(exhaustMap(() => this.api.save(this.form))); // Ignore while busy
ordered$ = this.ids$.pipe(concatMap(id => this.api.process(id)));      // Preserve order
```
