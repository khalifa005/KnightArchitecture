---
title: Use Barrel Files for Public APIs
impact: MEDIUM
impactDescription: Clear public contracts, encapsulation
tags: architecture, exports, api
---

## Use Barrel Files for Public APIs

Export only what other domains should access via `index.ts` barrel files. This rule is about **creating** focused barrel files for your domain boundaries. For avoiding **consuming** large barrel files from libraries, see `bundle-barrel-imports.md`.

```typescript
// customers/index.ts - only export public API
export { Customer } from './model/customer.model';
export { CustomerService } from './data/customer.service';
```
