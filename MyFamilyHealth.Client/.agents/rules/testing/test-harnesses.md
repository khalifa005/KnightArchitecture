---
title: Use CDK Component Test Harnesses
impact: HIGH
impactDescription: Stable tests decoupled from DOM structure
tags: testing, cdk, harness, material
---

## Use CDK Component Test Harnesses

Use CDK test harnesses (`HarnessLoader`) to interact with Angular Material and custom components in tests. Harnesses abstract DOM details, making tests resilient to template changes.

**Incorrect:**

```typescript
const button = fixture.debugElement.query(By.css('.mat-mdc-button'));
button.nativeElement.click(); // Breaks if class name changes
```

**Correct:**

```typescript
const loader = TestbedHarnessEnvironment.loader(fixture);
const button = await loader.getHarness(MatButtonHarness.with({ text: 'Save' }));
await button.click();
```
