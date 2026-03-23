---
title: Mocking with ng-mocks and MSW
impact: MEDIUM
impactDescription: Isolated, reliable tests
tags: testing, mocking, ng-mocks, msw, http
---

## Mocking with ng-mocks and MSW

Use ng-mocks for component/service mocking. Use `HttpTestingController` for HTTP mocking in unit tests. Use MSW for integration tests.

```typescript
TestBed.configureTestingModule({
  imports: [ParentComponent, MockComponent(ChildComponent)],
  providers: [MockProvider(UserService, { getUsers: () => of([mockUser]) })],
});
```
