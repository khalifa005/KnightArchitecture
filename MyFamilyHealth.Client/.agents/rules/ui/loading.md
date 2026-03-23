---
title: Loading State Patterns
impact: MEDIUM
impactDescription: Perceived performance and UX
tags: ui, loading, skeleton, empty-state
---

## Loading State Patterns

Use skeleton loaders matching content shape to prevent CLS. Disable buttons during async actions. Always show helpful `@empty` UI for zero-data states.

```html
@if (loading()) {
  <app-card-skeleton />
} @else {
  <app-card [data]="data()" />
}

@for (user of users(); track user.id) { ... } @empty {
  <app-empty-state title="No users found" />
}
```
