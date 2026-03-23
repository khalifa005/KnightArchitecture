---
title: Use Object Mothers for Test Data
impact: MEDIUM
impactDescription: Consistent, maintainable test fixtures
tags: testing, factories, test-data, object-mother
---

## Use Object Mothers for Test Data

Create factory functions (Object Mothers) that return valid default objects with optional overrides. Centralizes test data and ensures consistency across test suites.

**Incorrect:**

```typescript
const user = { id: '1', name: 'John', email: 'j@e.com', role: 'admin', createdAt: new Date() };
// Duplicated in every test file
```

**Correct:**

```typescript
export const createUser = (overrides: Partial<User> = {}): User => ({
  id: crypto.randomUUID(), name: 'John', email: 'john@example.com', ...overrides,
});
```
