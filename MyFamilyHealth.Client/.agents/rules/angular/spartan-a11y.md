---
title: Use Spartan Brain for Accessible Behavior
impact: MEDIUM
impactDescription: WCAG-compliant interactions for free
tags: spartan, a11y, brain, headless
---

## Use Spartan Brain for Accessible Behavior

Spartan Brain directives handle ARIA attributes, keyboard navigation, and focus management automatically. Use Brain primitives (`brnAccordion`, `brnMenu`, `brnTabs`) instead of building custom a11y logic.

**Incorrect:**

```typescript
// Custom tabs — missing aria-selected, arrow key navigation, focus management
@Component({ template: '@for (t of tabs; track t.id) { <div (click)="select(t)">{{t.label}}</div> }' })
```

**Correct:**

```html
<brn-tabs defaultValue="tab1">
  <brn-tabs-list hlmTabsList>
    <button brnTabsTrigger="tab1" hlmTabsTrigger>Tab 1</button>
  </brn-tabs-list>
</brn-tabs>
```
