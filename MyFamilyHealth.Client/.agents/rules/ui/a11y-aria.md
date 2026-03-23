---
title: Use ARIA Roles and Live Regions
impact: MEDIUM
impactDescription: Screen reader compatibility
tags: a11y, aria, screen-reader
---

## Use ARIA Roles and Live Regions

Use semantic HTML elements first (`<nav>`, `<main>`, `<button>`). Add ARIA roles only when native semantics are insufficient. Use `LiveAnnouncer` to announce dynamic content changes to screen readers.

**Incorrect:**

```html
<div class="nav-menu">
  <div (click)="navigate()">Home</div>
</div>
```

**Correct:**

```typescript
// Use semantic HTML for navigation
// template: <nav aria-label="Main"><a routerLink="/">Home</a></nav>
// Announce dynamic changes for screen readers
this.liveAnnouncer.announce('Item saved successfully');
```
