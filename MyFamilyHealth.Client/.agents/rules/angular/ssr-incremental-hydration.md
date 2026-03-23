---
title: Use Incremental Hydration with @defer
impact: HIGH
impactDescription: Reduces hydration cost
tags: ssr, hydration, defer
---

## Use Incremental Hydration with @defer

Enable `provideClientHydration(withIncrementalHydration())` and use `@defer (hydrate on ...)` triggers to hydrate components progressively instead of all at once.

**Incorrect:**

```html
<hero-banner />
<comments-section />
<footer-widgets />
```

**Correct:**

```html
<hero-banner />
@defer (hydrate on viewport) {
  <comments-section />
}
```
