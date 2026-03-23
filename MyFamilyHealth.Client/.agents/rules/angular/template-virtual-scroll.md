---
title: Use Virtual Scrolling for Large Lists
impact: HIGH
impactDescription: Renders only visible items
tags: template, cdk, performance
---

## Use Virtual Scrolling for Large Lists

Use `CdkVirtualScrollViewport` with `*cdkVirtualFor` to render only visible items in long lists, reducing DOM nodes from thousands to ~20.

**Incorrect:**

```html
@for (item of items; track item.id) {
  <div>{{ item.name }}</div>
}
```

**Correct:**

```html
<cdk-virtual-scroll-viewport itemSize="48">
  <div *cdkVirtualFor="let item of items">{{ item.name }}</div>
</cdk-virtual-scroll-viewport>
```
