---
title: Set SEO Meta Tags for SSR Pages
impact: MEDIUM
impactDescription: Improves search indexing
tags: ssr, seo, meta
---

## Set SEO Meta Tags for SSR Pages

Use Angular's `Meta` and `Title` services to set Open Graph, Twitter Card, and canonical URL meta tags for server-rendered pages.

**Incorrect:**

```typescript
export class ProductPage {
  product = input.required<Product>();
}
```

**Correct:**

```typescript
#meta = inject(Meta);
#title = inject(Title);
product = input.required<Product>();
constructor() {
  effect(() => this.#title.setTitle(this.product().name));
}
```
