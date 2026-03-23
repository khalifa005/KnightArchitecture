---
title: Use PrimeNG Theme System
impact: MEDIUM
impactDescription: Consistent design with design tokens
tags: primeng, theming, design-tokens, styled
---

## Use PrimeNG Theme System

Use PrimeNG's styled mode with Aura or Lara presets. Customize via design tokens in `providePrimeNG()` instead of overriding CSS classes. Use `dt()` function for accessing tokens in custom styles.

**Incorrect:**

```css
.p-button { background: #1976d2 !important; } /* Breaks theming */
```

**Correct:**

```typescript
providePrimeNG({
  theme: { preset: Aura, options: { darkModeSelector: '.dark' } }
})
```
