---
title: Use Host Object for Element Bindings
impact: HIGH
impactDescription: Centralizes host interactions
tags: components, host, bindings
---

## Use Host Object for Element Bindings

Use the `host` property in `@Component`/`@Directive` instead of `@HostBinding` and `@HostListener` decorators for centralized, declarative host element interactions.

**Incorrect:**

```typescript
@HostBinding('class.active') isActive = false;
@HostListener('click') onClick() { this.isActive = true; }
```

**Correct:**

```typescript
@Component({
  host: { '[class.active]': 'isActive()', '(click)': 'activate()' }
})
```
