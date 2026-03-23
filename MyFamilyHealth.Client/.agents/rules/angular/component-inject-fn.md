---
title: Use inject() Function for Dependencies
impact: HIGH
impactDescription: Cleaner DI, works in functions
tags: components, dependency-injection, inject
---

## Use inject() Function for Dependencies

Prefer the `inject()` function over constructor injection for cleaner syntax, better type inference, and compatibility with functional guards/interceptors.

**Incorrect:**

```typescript
@Component({ selector: 'app-user' })
export class UserComponent {
  constructor(private userService: UserService, private router: Router) {}
}
```

**Correct:**

```typescript
@Component({ selector: 'app-user' })
export class UserComponent {
  private userService = inject(UserService);
  private router = inject(Router);
}
```
