---
title: Use Repository Pattern for Data Access
impact: MEDIUM
impactDescription: Abstracts data source, enables caching
tags: patterns, architecture, repository
---

## Use Repository Pattern for Data Access

Use Repository to abstract data access when you need caching, DTO-to-domain mapping, or data source swappability. Skip for simple CRUD with no transformation.

```typescript
@Injectable({ providedIn: 'root' })
export class UserRepository {
  private cache = new Map<string, User>();
  async findById(id: string): Promise<User | null> {
    if (this.cache.has(id)) return this.cache.get(id)!;
    const dto = await lastValueFrom(this.http.get<UserDto>(`/api/users/${id}`));
    return this.mapToEntity(dto);
  }
}
```
