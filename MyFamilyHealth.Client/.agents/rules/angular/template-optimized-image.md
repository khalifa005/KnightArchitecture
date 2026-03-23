---
title: Use NgOptimizedImage for Images
impact: HIGH
impactDescription: Optimizes LCP & Core Web Vitals
tags: template, images, performance
---

## Use NgOptimizedImage for Images

Use the `NgOptimizedImage` directive (`ngSrc`) with `priority` for above-fold images to enable automatic lazy loading, srcset generation, and preconnect hints.

**Incorrect:**

```html
<img src="/assets/hero.png" alt="Hero">
```

**Correct:**

```html
<img ngSrc="/assets/hero.png" width="800" height="400" priority alt="Hero">
```
