---
title: Manage Focus with CDK FocusTrap
impact: MEDIUM
impactDescription: Accessible modals and overlays
tags: a11y, focus, cdk, modal, trap
---

## Manage Focus with CDK FocusTrap

Use `cdkTrapFocus` for dialogs and overlays to prevent focus from escaping. Restore focus to the trigger element on close. Use `cdkFocusInitial` to set the initially focused element.

**Incorrect:**

```html
<div class="modal">
  <input placeholder="Name" />
  <button (click)="close()">Close</button>
</div>
```

**Correct:**

```html
<div class="modal" cdkTrapFocus [cdkTrapFocusAutoCapture]="true">
  <input cdkFocusInitial placeholder="Name" />
  <button (click)="close()">Close</button>
</div>
```
