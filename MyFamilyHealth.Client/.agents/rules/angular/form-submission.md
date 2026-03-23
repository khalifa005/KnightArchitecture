---
title: Handle Form Submission Properly
impact: MEDIUM
impactDescription: Better UX, proper error handling
tags: forms, submission, loading
---

## Handle Form Submission Properly

Check validity and mark touched before submit, track loading state with a signal, and use `getRawValue()` to include disabled fields.

```typescript
async onSubmit() {
  if (this.form.invalid) { this.form.markAllAsTouched(); return; }
  this.loading.set(true);
  try { await this.api.submit(this.form.getRawValue()); }
  finally { this.loading.set(false); }
}
```
