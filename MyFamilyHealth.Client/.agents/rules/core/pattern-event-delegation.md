---
title: Use Event Delegation for Lists
impact: MEDIUM
impactDescription: Reduces memory from O(n) to O(1) listeners
tags: events, performance, delegation
---

## Use Event Delegation for Lists

Attach one event listener to a parent instead of listeners on each child. Use `data-*` attributes and `event.target` to identify clicked item.

```html
<div (click)="handleClick($event)">
  @for (item of items(); track item.id) {
    <button [attr.data-id]="item.id">{{ item.name }}</button>
  }
</div>
<!-- One listener handles all clicks via event.target -->
```
