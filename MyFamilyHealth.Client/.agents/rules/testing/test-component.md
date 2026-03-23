---
title: Component Testing with Angular Testing Library
impact: HIGH
impactDescription: user-centric, reliable component tests
tags: testing, testing-library, components
---

## Component Testing with Angular Testing Library

Use Angular Testing Library for user-centric tests. Prefer accessible queries: `getByRole`, `getByLabelText`, `getByText`. Use `getByTestId` as last resort.

```typescript
it('should increment count', async () => {
  await render(CounterComponent);
  await userEvent.setup().click(screen.getByRole('button', { name: /increment/i }));
  expect(screen.getByText('Count: 1')).toBeInTheDocument();
});
```
