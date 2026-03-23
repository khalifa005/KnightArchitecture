---
title: Unit Testing with Vitest
impact: HIGH
impactDescription: Fast, reliable tests for services and logic
tags: testing, vitest, unit-tests, services
---

## Unit Testing with Vitest

Use Vitest for unit testing services, pipes, guards, and signals. For signals, set values directly and assert computed results. For pipes, instantiate and call `transform()`.

```typescript
it('should fetch users', async () => {
  TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting()] });
  const service = TestBed.inject(UserService);
  expect(await firstValueFrom(service.getUsers())).toBeDefined();
});
```
