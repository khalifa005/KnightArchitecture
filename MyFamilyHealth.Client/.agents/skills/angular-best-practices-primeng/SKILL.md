---
name: angular-best-practices-primeng
description: >-
  PrimeNG best practices for Angular. Covers tree-shaking imports, lazy-loaded
  tables, and the Aura/Lara theme system with design tokens.
  Activates when working with primeng components.
  Do not use for Angular Material, Spartan UI, or other component libraries.
  Install alongside angular-best-practices for full coverage.
license: MIT
metadata:
  author: alfredoperez
  version: "1.2.0"
tags: [angular, primeng, ui-components]
globs:
  - "**/*.ts"
  - "**/*.component.ts"
  - "**/*.html"
---

# Angular PrimeNG Best Practices

PrimeNG rules for component imports, table performance, and theming. Use with the core
[angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices)
skill for comprehensive Angular coverage.

## Links

- [Core Skill: angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices)
- [Browse All Skills](https://skills.sh/alfredoperez/angular-best-practices)
- [GitHub Repository](https://github.com/alfredoperez/angular-best-practices)

## When to Apply

- Importing PrimeNG standalone components
- Configuring tables with server-side pagination or virtual scrolling
- Setting up Aura/Lara themes with design tokens

## Rules

| Rule | Impact | Description |
|------|--------|-------------|
| Tree-Shake PrimeNG Imports | MEDIUM | Standalone component imports for smaller bundles |
| Use PrimeNG Table with Lazy Loading | HIGH | Server-side pagination for large datasets |
| Use PrimeNG Theme System | MEDIUM | Design tokens via providePrimeNG instead of CSS overrides |

## Install

Install from [skills.sh/alfredoperez/angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices):

- Core skill: [angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices)
- This add-on: [angular-best-practices-primeng](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices-primeng)
