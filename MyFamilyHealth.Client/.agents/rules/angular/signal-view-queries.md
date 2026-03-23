---
title: Use Signal View Queries
impact: HIGH
impactDescription: Reactive, no lifecycle hooks
tags: signals, viewChild, viewChildren
---

## Use Signal View Queries

Use `viewChild()` and `viewChildren()` signal-based queries instead of `@ViewChild` and `@ViewChildren` decorators.

**Incorrect:**

```typescript
@ViewChild('chart') chart!: ElementRef;
ngAfterViewInit() { this.initChart(this.chart.nativeElement); }
```

**Correct:**

```typescript
chart = viewChild.required<ElementRef>('chart');
// Reactive: use chart() in computed() or effect(), no lifecycle hooks needed
```
