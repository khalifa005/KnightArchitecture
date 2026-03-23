---
title: Avoid Barrel File Imports
impact: CRITICAL
impactDescription: 200-800ms import cost, slow builds
tags: bundle, imports, tree-shaking, barrel-files, performance
---

## Avoid Barrel File Imports

Import directly from source files instead of barrel files (index.ts) to avoid loading thousands of unused modules. For **creating** barrel files for your domain's public API, see `arch-barrel-files.md`.

**Incorrect (imports entire library):**

```typescript
import { Check, X, Menu } from 'lucide-angular';
import { debounce } from 'lodash';
```

**Correct (direct imports):**

```typescript
import { LucideCheck } from 'lucide-angular/check';
import debounce from 'lodash/debounce';
```

**Libraries commonly affected:**
- `@angular/material` - import from submodules
- `rxjs` - import operators directly
- `lodash` - use `lodash-es` with direct imports
- Icon libraries - import individual icons
