---
title: Import Angular Material Modules Selectively
impact: MEDIUM
impactDescription: Reduces bundle size
tags: material, imports, tree-shaking
---

## Import Angular Material Modules Selectively

Import individual Material component modules (e.g., `MatButtonModule`) directly in standalone components instead of creating shared material modules. This enables tree-shaking of unused components.

**Incorrect:**

```typescript
// shared-material.module.ts — imports everything, tree-shaking fails
@NgModule({ exports: [MatButtonModule, MatCardModule, MatTableModule, /* 30+ modules */] })
```

**Correct:**

```typescript
@Component({
  imports: [MatButtonModule, MatIconModule], // Only what this component needs
})
```
