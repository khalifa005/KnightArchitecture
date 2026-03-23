---
title: Use Pure Mapper Functions for DTOs
impact: MEDIUM
impactDescription: Decouples frontend from API, type-safe transforms
tags: data, mappers, dto
---

## Use Pure Mapper Functions for DTOs

Create pure functions in a `mappers/` folder to map between API DTOs and domain models. Keeps components decoupled from API structure.

```typescript
export function mapUserDto(dto: UserDto): User {
  return {
    fullName: `${dto.first_name} ${dto.last_name}`,
    email: dto.email_address,
    createdAt: new Date(dto.created_at),
  };
}
```
