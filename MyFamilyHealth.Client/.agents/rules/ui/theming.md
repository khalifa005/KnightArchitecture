---
title: Theming Patterns
impact: MEDIUM
impactDescription: Consistent design and dark mode
tags: ui, css, theming, dark-mode
---

## Theming Patterns

Define theme values as CSS custom properties (`--border-color`). Support dark mode via `prefers-color-scheme` and `data-theme` attributes. Use CDK `BreakpointObserver` for responsive logic.

```css
:root { --border-color: #e5e7eb; }
@media (prefers-color-scheme: dark) { :root { --bg-color: #1f2937; } }
```

```typescript
isMobile = toSignal(this.breakpointObserver.observe('(max-width: 768px)').pipe(map(r => r.matches)));
```
