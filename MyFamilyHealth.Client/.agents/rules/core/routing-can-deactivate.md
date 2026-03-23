---
title: Use CanDeactivate Guard for Unsaved Changes
impact: MEDIUM
impactDescription: Prevents accidental data loss
tags: routing, guards, forms
---

## Use CanDeactivate Guard for Unsaved Changes

Implement `CanDeactivateFn` to warn users before navigating away from forms with unsaved data.

**Incorrect:**

```typescript
// No guard — user navigates away and loses unsaved changes
{ path: 'edit/:id', component: EditComponent }
```

**Correct:**

```typescript
export const unsavedChangesGuard: CanDeactivateFn<{ hasUnsavedChanges(): boolean }> =
  (component) => component.hasUnsavedChanges() ? confirm('Discard changes?') : true;
// Usage: { path: 'edit/:id', canDeactivate: [unsavedChangesGuard] }
```
