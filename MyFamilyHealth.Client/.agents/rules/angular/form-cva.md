---
title: Use ControlValueAccessor for Custom Controls
impact: MEDIUM
impactDescription: Form integration, reusable custom inputs
tags: forms, cva, custom-controls
---

## Use ControlValueAccessor for Custom Controls

Implement `ControlValueAccessor` to make custom components work with reactive forms via `formControlName` and `[(ngModel)]`.

```typescript
@Component({ providers: [{ provide: NG_VALUE_ACCESSOR, useExisting: RatingComponent, multi: true }] })
export class RatingComponent implements ControlValueAccessor {
  writeValue(v: number) { this.value.set(v); }
}
```
