---
title: Create Reusable Validators
impact: MEDIUM
impactDescription: DRY validation, consistent error handling
tags: forms, validators, validation
---

## Create Reusable Validators

Extract common validation logic into reusable `ValidatorFn` functions instead of inline validators or repeated patterns.

```typescript
export const emailValidator: ValidatorFn = (control) =>
  control.value?.includes('@') ? null : { invalidEmail: true };

// Usage: email: ['', [Validators.required, emailValidator]]
```
