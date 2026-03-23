---
title: Use CDK Utilities Over Custom Implementations
impact: MEDIUM
impactDescription: Battle-tested, accessible primitives
tags: material, cdk, overlay, drag-drop, a11y
---

## Use CDK Utilities Over Custom Implementations

Use Angular CDK for overlays (`Overlay`), drag-and-drop (`cdkDrag`), virtual scrolling (`cdkVirtualFor`), clipboard (`cdkCopyToClipboard`), and accessibility (`FocusTrap`, `LiveAnnouncer`) instead of custom solutions.

**Incorrect:**

```typescript
// Custom scroll handler — misses edge cases, no recycling
@HostListener('scroll') onScroll() { this.loadMore(); }
```

**Correct:**

```html
<cdk-virtual-scroll-viewport itemSize="48">
  <div *cdkVirtualFor="let item of items">{{ item.name }}</div>
</cdk-virtual-scroll-viewport>
```
