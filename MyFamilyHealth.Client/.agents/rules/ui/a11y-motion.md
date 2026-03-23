---
title: Respect prefers-reduced-motion
impact: MEDIUM
impactDescription: Prevents motion sickness triggers
tags: a11y, motion, animation, media-query
---

## Respect prefers-reduced-motion

Disable or simplify animations for users who prefer reduced motion. Use the `prefers-reduced-motion` media query. In Angular animations, check the preference before applying transitions.

**Incorrect:**

```css
.card { transition: transform 0.5s ease; }
```

**Correct:**

```css
.card { transition: transform 0.5s ease; }
@media (prefers-reduced-motion: reduce) {
  .card { transition: none; }
}
```
