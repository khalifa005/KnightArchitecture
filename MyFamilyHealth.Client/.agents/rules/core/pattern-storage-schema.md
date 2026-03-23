---
title: Schema-Validate LocalStorage Data
impact: MEDIUM
impactDescription: Prevents runtime errors from corrupted storage
tags: storage, validation, localStorage
---

## Schema-Validate LocalStorage Data

Use typed schemas (Zod) with versioning for localStorage to catch corrupted data and enable migrations.

```typescript
const result = StorageSchema.safeParse(JSON.parse(localStorage.getItem('user') ?? 'null'));
const user = result.success ? result.data.user : migrateOrDefault(result); // fallback on failure
```
