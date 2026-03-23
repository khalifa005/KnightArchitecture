---
title: Conditional Imports for Features
impact: MEDIUM
impactDescription: Excludes unused code from bundle
tags: bundle, conditional, feature-flags, tree-shaking
---

## Conditional Imports for Features

Use conditional imports and feature flags to exclude unused code from production bundles.

```typescript
providers: [
  provideRouter(routes),
  ...(isDevMode() ? [provideDevTools()] : []),
]
```
