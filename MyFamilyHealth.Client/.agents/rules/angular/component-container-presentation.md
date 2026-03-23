---
title: Use Container/Presentation Pattern
impact: HIGH
impactDescription: Better reusability, testability, separation of concerns
tags: components, architecture, patterns
---

## Use Container/Presentation Pattern

Separate data retrieval (Container) from rendering (Presentation). Containers fetch data via services; presentation components receive data via inputs and use OnPush change detection.

```typescript
@Component({ changeDetection: ChangeDetectionStrategy.OnPush })
export class UserList { users = input.required<User[]>(); }
```
