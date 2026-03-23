---
title: Ensure Keyboard Navigation
impact: MEDIUM
impactDescription: Keyboard-only user access
tags: a11y, keyboard, navigation, tabindex
---

## Ensure Keyboard Navigation

All interactive elements must be keyboard-accessible. Use native `<button>` and `<a>` elements. For custom widgets, add `tabindex="0"` and handle `keydown.enter`/`keydown.space` events.

**Incorrect:**

```html
<div class="tab" (click)="selectTab(i)">{{ tab.label }}</div>
```

**Correct:**

```html
<button role="tab" [attr.aria-selected]="isActive(i)" (click)="selectTab(i)">
  {{ tab.label }}
</button>
```
