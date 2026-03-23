---
title: Use ngx-formly for Dynamic Forms
impact: MEDIUM
impactDescription: Reduces form boilerplate by 60-80%
tags: forms, dynamic, formly, json
---

## Use ngx-formly for Dynamic Forms

Use ngx-formly for forms driven by configuration (JSON/API). Define field configs instead of writing template markup. Create reusable custom field types for domain-specific inputs.

**Incorrect:**

```html
<!-- Manually building 50+ fields with repeated template markup -->
@for (f of fields; track f.key) { <input [formControlName]="f.key" [type]="f.type" /> }
```

**Correct:**

```typescript
fields: FormlyFieldConfig[] = [
  { key: 'name', type: 'input', props: { label: 'Name', required: true } },
  { key: 'role', type: 'select', props: { label: 'Role', options: this.roles$ } },
];
```
