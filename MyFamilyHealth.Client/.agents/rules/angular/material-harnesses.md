---
title: Use Test Harnesses for Material Components
impact: HIGH
impactDescription: Stable tests across Material versions
tags: material, testing, harness, cdk
---

## Use Test Harnesses for Material Components

Use Material's built-in test harnesses (`MatSelectHarness`, `MatInputHarness`, etc.) instead of querying DOM elements. Harnesses are maintained by the Angular team and survive internal DOM changes.

**Incorrect:**

```typescript
const select = fixture.debugElement.query(By.css('.mat-mdc-select'));
select.nativeElement.click(); // Fragile — breaks on Material updates
```

**Correct:**

```typescript
const select = await loader.getHarness(MatSelectHarness.with({ selector: '#role' }));
await select.open();
await select.clickOptions({ text: 'Admin' });
```
