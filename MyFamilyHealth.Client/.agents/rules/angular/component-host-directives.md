---
title: Use Host Directives for Composition
impact: MEDIUM
impactDescription: Composition over inheritance
tags: components, directives, patterns
---

## Use Host Directives for Composition

Add behavior to components via `hostDirectives` instead of inheritance. This enables composition of reusable behaviors (tooltips, focus management) without class hierarchies.

```typescript
@Component({
  selector: 'app-button',
  hostDirectives: [TooltipDirective, FocusDirective]
})
export class ButtonComponent { }
```
