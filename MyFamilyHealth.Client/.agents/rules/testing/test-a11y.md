---
title: Automate Accessibility Testing with axe-core
impact: HIGH
impactDescription: Catches WCAG violations automatically
tags: testing, a11y, axe-core, vitest
---

## Automate Accessibility Testing with axe-core

Use `vitest-axe` or `jest-axe` to check rendered components for WCAG violations. Run `axe()` on the container element after rendering and assert no violations.

**Incorrect:**

```typescript
it('should be accessible', () => {
  // Manual visual check only — misses hidden violations
});
```

**Correct:**

```typescript
it('should have no a11y violations', async () => {
  const { container } = await render(MyComponent);
  expect(await axe(container)).toHaveNoViolations();
});
```
