---
title: Compose Mappers for Nested Data
impact: MEDIUM
impactDescription: Reusable, maintains single responsibility
tags: data, mappers, composition
---

## Compose Mappers for Nested Data

Compose smaller mappers for nested structures. Each mapper handles one entity type, enabling reuse and single responsibility.

```typescript
const mapOrderItem = (dto: OrderItemDto): OrderItem => ({
  productName: dto.product_name,
  subtotal: dto.quantity * dto.unit_price,
});

const mapOrder = (dto: OrderDto): Order => ({
  customer: mapCustomer(dto.customer),
  items: dto.items.map(mapOrderItem),
});
```
