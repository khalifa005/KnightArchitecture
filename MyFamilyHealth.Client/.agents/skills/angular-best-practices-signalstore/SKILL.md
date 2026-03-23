---
name: angular-best-practices-signalstore
description: >-
  NgRx SignalStore best practices for Angular. Covers shared state,
  computed state, entity management, and RxJS integration with rxMethod.
  Activates when working with @ngrx/signals and @ngrx/signals/entities.
  Do not use for NgRx Store or class-based state services.
  Install alongside angular-best-practices for full coverage.
license: MIT
metadata:
  author: alfredoperez
  version: "1.2.0"
tags: [angular, ngrx-signals, signalstore, state-management]
globs:
  - "**/*.ts"
  - "**/*.store.ts"
---

# Angular SignalStore Best Practices

NgRx SignalStore rules for signal-based local and feature state management. Use with the core
[angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices)
skill for comprehensive Angular coverage.

## Links

- [Core Skill: angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices)
- [Browse All Skills](https://skills.sh/alfredoperez/angular-best-practices)
- [GitHub Repository](https://github.com/alfredoperez/angular-best-practices)

## When to Apply

- Creating or modifying SignalStore-based state management
- Integrating RxJS side effects with `rxMethod`
- Managing collections with `withEntities`

## Rules

| Rule | Impact | Description |
|------|--------|-------------|
| Use rxMethod for RxJS Integration | MEDIUM | Debounce, switchMap, and other RxJS operators in stores |
| Use SignalStore for Shared State | HIGH | Signal-based reactivity without full NgRx overhead |
| Use withComputed for Derived State | MEDIUM | Centralized memoized derivation logic |
| Use withEntities for Collections | MEDIUM | O(1) lookups and standardized CRUD operations |

## Install

Install from [skills.sh/alfredoperez/angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices):

- Core skill: [angular-best-practices](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices)
- This add-on: [angular-best-practices-signalstore](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices-signalstore)
