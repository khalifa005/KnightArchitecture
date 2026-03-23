---
title: Use Spartan UI Headless Components
impact: MEDIUM
impactDescription: Full styling control with a11y built-in
tags: spartan, headless, components, brain
---

## Use Spartan UI Headless Components

Use Spartan's Brain (headless) directives for behavior and accessibility, paired with Helm (styled) components for UI. Install only the primitives you need via `@spartan-ng/brain/*`.

**Incorrect:**

```typescript
// Building custom dialog from scratch — missing a11y, focus trap, esc handling
@Component({ template: '<div class="overlay"><div class="panel">...</div></div>' })
```

**Correct:**

```html
<brn-dialog>
  <brn-dialog-trigger><button hlmBtn>Open</button></brn-dialog-trigger>
  <brn-dialog-content hlmDialogContent>Content here</brn-dialog-content>
</brn-dialog>
```
