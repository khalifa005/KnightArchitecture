---
title: Enable Scroll Position Restoration
impact: MEDIUM
impactDescription: Better navigation UX
tags: routing, scroll, ux
---

## Enable Scroll Position Restoration

Use `withInMemoryScrolling({ scrollPositionRestoration: 'enabled' })` to automatically restore scroll positions when navigating back.

**Incorrect:**

```typescript
// Manually scrolling to top on every navigation — loses back-button position
router.events.pipe(filter(e => e instanceof NavigationEnd)).subscribe(() => window.scrollTo(0, 0));
```

**Correct:**

```typescript
provideRouter(routes,
  withInMemoryScrolling({ scrollPositionRestoration: 'enabled', anchorScrolling: 'enabled' }),
)
```
