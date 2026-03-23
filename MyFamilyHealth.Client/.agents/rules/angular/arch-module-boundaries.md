---
title: Enforce Module Boundaries
impact: HIGH
impactDescription: Prevents spaghetti dependencies
tags: architecture, boundaries, sheriff
---

## Enforce Module Boundaries

Use Sheriff or Nx to enforce dependency rules between domains and layers.

**Incorrect:**

```typescript
// ui component imports from another domain's data layer
import { OrderService } from '../../orders/data/order.service'; // Cross-domain!
```

**Correct:**

```typescript
// ui component only imports from own domain or shared
import { OrderService } from '../data/order.service';
import { formatCurrency } from '@app/shared/util';
```

Use Sheriff for standalone projects, Nx for monorepos.
