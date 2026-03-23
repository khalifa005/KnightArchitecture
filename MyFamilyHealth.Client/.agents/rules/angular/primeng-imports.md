---
title: Tree-Shake PrimeNG Imports
impact: MEDIUM
impactDescription: Reduces bundle size
tags: primeng, imports, tree-shaking, standalone
---

## Tree-Shake PrimeNG Imports

Import PrimeNG components individually using standalone component imports. Avoid importing entire PrimeNG modules. Use the standalone API available since PrimeNG v17+.

**Incorrect:**

```typescript
// Importing full module pulls in all components
import { ButtonModule } from 'primeng/button';
@NgModule({ imports: [ButtonModule, TableModule, DialogModule] })
```

**Correct:**

```typescript
@Component({
  imports: [Button, Select], // Standalone components — only what's needed
})
```
