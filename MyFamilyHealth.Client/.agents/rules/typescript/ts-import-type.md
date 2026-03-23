---
title: Use import type for Type-Only Imports
impact: LOW
impactDescription: Smaller bundles, cleaner transpilation
tags: typescript, imports, bundle
---

## Use import type for Type-Only Imports

Use `import type { User } from './user'` instead of `import { type User } from './user'` — the statement-level form is clearer, always fully erased at transpilation, and aligns with `verbatimModuleSyntax` in TypeScript 5.0+.
