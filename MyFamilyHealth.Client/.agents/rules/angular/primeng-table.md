---
title: Use PrimeNG Table with Lazy Loading
impact: HIGH
impactDescription: Handles 100k+ rows efficiently
tags: primeng, table, lazy-loading, virtual-scroll
---

## Use PrimeNG Table with Lazy Loading

Use `[lazy]="true"` with `(onLazyLoad)` for server-side pagination, sorting, and filtering. For large client-side datasets, enable `[virtualScroll]="true"` with `[virtualScrollItemSize]`.

**Incorrect:**

```html
<!-- Loads all 10,000 rows into DOM -->
<p-table [value]="allData"></p-table>
```

**Correct:**

```html
<p-table [value]="data" [lazy]="true" [totalRecords]="total"
         (onLazyLoad)="load($event)" [paginator]="true" [rows]="20">
</p-table>
```
