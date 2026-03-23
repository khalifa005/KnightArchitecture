---
title: Use Signal Content Queries
impact: HIGH
impactDescription: Reactive, no lifecycle hooks
tags: signals, contentChild, contentChildren
---

## Use Signal Content Queries

Use `contentChild()` and `contentChildren()` signal-based queries instead of `@ContentChild` and `@ContentChildren` decorators.

**Incorrect:**

```typescript
@ContentChild(TemplateRef) template!: TemplateRef<unknown>;
ngAfterContentInit() { this.renderTemplate(this.template); }
```

**Correct:**

```typescript
template = contentChild.required(TemplateRef);
// Reactive: use template() in computed() or effect(), no lifecycle hooks needed
```
