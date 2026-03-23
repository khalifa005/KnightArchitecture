---
name: angular-best-practices-spartan
description: >-
  Spartan UI (shadcn for Angular) best practices. Covers Brain/Helm architecture,
  Tailwind integration, and accessible headless components.
  Activates when working with @spartan-ng/brain and @spartan-ng/ui.
  Do not use for Angular Material, PrimeNG, or other component libraries.
  Install alongside angular-best-practices for full coverage.
license: MIT
metadata:
  author: alfredoperez
  version: "1.2.0"
tags: [angular, spartan, headless-ui, tailwind]
globs:
  - "**/*.ts"
  - "**/*.component.ts"
  - "**/*.html"
---

# Angular Spartan UI Best Practices

Spartan UI rules for headless components with Brain (behavior) and Helm (styling). Use with the core
[angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices)
skill for comprehensive Angular coverage.

## Links

- [Core Skill: angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices)
- [Browse All Skills](https://skills.sh/alfredoperez/angular-best-practices)
- [GitHub Repository](https://github.com/alfredoperez/angular-best-practices)

## When to Apply

- Adding Spartan Brain directives for accessible behavior
- Styling components with Helm and Tailwind CSS
- Building dialogs, tabs, menus, or accordions with Spartan primitives

## Rules

| Rule | Impact | Description |
|------|--------|-------------|
| Configure Tailwind with Spartan Helm | MEDIUM | Consistent utility-first styling via hlm directives |
| Use Spartan Brain for Accessible Behavior | MEDIUM | WCAG-compliant ARIA, keyboard nav, and focus management |
| Use Spartan UI Headless Components | MEDIUM | Full styling control with built-in accessibility |

## Install

Install from [skills.sh/alfredoperez/angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices):

- Core skill: [angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices)
- This add-on: [angular-best-practices-spartan](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices-spartan)
