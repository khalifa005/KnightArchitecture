---
title: Use Facade Pattern for Complex Domains
impact: HIGH
impactDescription: Simplifies component code, single entry point
tags: patterns, architecture, facade
---

## Use Facade Pattern for Complex Domains

Use a Facade when components orchestrate 3+ services with interdependent async operations. Use when business logic spans multiple services; skip for simple service composition.

```typescript
@Injectable({ providedIn: 'root' })
export class OrdersFacade {
  private store = inject(OrdersStore);
  private api = inject(OrdersApi);
  readonly orders = this.store.orders;
  async loadOrders() { /* orchestrates store + api */ }
}
```
