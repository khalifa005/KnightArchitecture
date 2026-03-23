---
title: Use model() for Two-Way Binding
impact: HIGH
impactDescription: Simplified two-way binding
tags: signals, model, two-way-binding
---

## Use model() for Two-Way Binding

Use `model()` instead of input+output pairs when parent and child need synchronized state via `[(value)]` syntax.

**Incorrect:**

```typescript
@Input() value = 0;
@Output() valueChange = new EventEmitter<number>();
update(v: number) { this.value = v; this.valueChange.emit(v); }
```

**Correct:**

```typescript
value = model<number>(0);
update(v: number) { this.value.set(v); }
// Parent: <my-comp [(value)]="count" />
```
