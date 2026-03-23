---
title: Use Compound Components
impact: MEDIUM
impactDescription: Cleaner APIs, avoids input drilling
tags: components, patterns, di
---

## Use Compound Components

Share state between related components via dependency injection instead of input drilling. Child components inject the parent to coordinate state.

```html
<app-tabs>
  <app-tab id="a">Tab A</app-tab>
  <app-tab-panel id="a">Content A</app-tab-panel>
</app-tabs>
```
