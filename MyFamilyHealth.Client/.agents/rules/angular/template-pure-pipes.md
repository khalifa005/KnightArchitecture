---
title: Use Pure Pipes for Transforms
impact: MEDIUM
impactDescription: Memoized, only recalculates on input change
tags: pipes, performance, templates
---

## Use Pure Pipes for Transforms

Use pure pipes instead of method calls in templates for memoized transformations.

**Incorrect:**

```html
<!-- Method runs on every change detection cycle -->
<span>{{ formatCurrency(price) }}</span>
```

**Correct:**

```html
<!-- Pure pipe only runs when price changes -->
<span>{{ price | currency:'USD' }}</span>
<!-- Or custom pipe -->
<span>{{ user.name | initials }}</span>
```
