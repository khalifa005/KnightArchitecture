---
name: angular-best-practices-transloco
description: >-
  Transloco i18n best practices for Angular. Covers runtime translation,
  lazy-loaded translation files per route, and test mocking.
  Activates when working with @jsverse/transloco.
  Do not use for ngx-translate or i18next.
  Install alongside angular-best-practices for full coverage.
license: MIT
metadata:
  author: alfredoperez
  version: "1.2.0"
tags: [angular, transloco, i18n, internationalization]
globs:
  - "**/*.ts"
  - "**/*.html"
  - "**/*.json"
---

# Angular Transloco Best Practices

Transloco rules for runtime internationalization, lazy-loaded translations, and testing. Use with the core
[angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices)
skill for comprehensive Angular coverage.

## Links

- [Core Skill: angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices)
- [Browse All Skills](https://skills.sh/alfredoperez/angular-best-practices)
- [GitHub Repository](https://github.com/alfredoperez/angular-best-practices)

## When to Apply

- Adding runtime i18n with the `transloco` pipe or directive
- Lazy-loading translation files per route with scopes
- Writing tests that use `TranslocoTestingModule`

## Rules

| Rule | Impact | Description |
|------|--------|-------------|
| Lazy Load Translation Files per Route | MEDIUM | Load scoped translations only when a route is activated |
| Mock Translations in Tests | MEDIUM | Fast deterministic tests without HTTP translation loading |
| Use Transloco for Runtime i18n | MEDIUM | Dynamic language switching without app rebuilds |

## Install

Install from [skills.sh/alfredoperez/angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices):

- Core skill: [angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices)
- This add-on: [angular-best-practices-transloco](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices-transloco)
