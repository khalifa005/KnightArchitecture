---
name: angular-best-practices
description: >-
  Curated Angular best practices for building performant, maintainable Angular 17+
  applications. Covers Signals, RxJS, performance, SSR, testing, forms, routing,
  and architecture. Library-specific add-ons available for NgRx, SignalStore,
  TanStack Query, Material, PrimeNG, Spartan UI, and Transloco.
  Do not use for AngularJS (1.x), React, Vue, or non-Angular TypeScript projects.
version: 1.2.0
author: alfredoperez
tags:
  - angular
  - typescript
  - signals
  - performance
  - testing
  - state-management
globs:
  - "**/*.ts"
  - "**/*.html"
  - "**/*.component.ts"
  - "**/*.service.ts"
---

# Modern Angular Best Practices

A complete set of rules covering everything from TypeScript strictness to signal-based reactivity, SSR hydration, bundle optimization, and accessible UI — so every component, service, and route you ship is fast, tested, and maintainable.

## TypeScript

- Use strict type checking with `strict: true` in tsconfig
- Avoid `any`; use `unknown` when type is uncertain
- Use `import type` for type-only imports
- Add explicit return types to exported functions
- Prefer `readonly` for data that should not be mutated

## Components

- Use standalone components with `ChangeDetectionStrategy.OnPush`
- Use `input()` and `output()` functions instead of `@Input()` / `@Output()` decorators
- Use `inject()` instead of constructor injection
- Use `computed()` for derived state — memoized, only recalculates when dependencies change
- Keep components small and single-responsibility
- Prefer inline templates for small components

## Templates

- Use `@if`, `@for`, `@switch` instead of `*ngIf`, `*ngFor`, `*ngSwitch`
- Use `[class.active]` bindings instead of `[ngClass]`
- Use `@defer` for heavy below-fold content
- Always provide `track` with `@for` loops

## Signals & Reactivity

- Use signals for local component state
- Use `computed()` for derived state instead of getters
- Use `effect()` for side effects only — never for state synchronization
- Use `toSignal()` to bridge RxJS observables into signal-based templates

## Performance

- Preload critical data with route resolvers — eliminate loading waterfalls
- Lazy-load routes and `@defer` heavy views
- Tree-shake imports — use standalone component imports, not full modules
- Avoid synchronous layout reads in loops — batch DOM operations

## RxJS

- Unsubscribe from observables — use `takeUntilDestroyed()` or `async` pipe
- Place `catchError` inside `switchMap` to keep the outer stream alive
- Use `switchMap` for latest-only, `exhaustMap` for ignore-while-busy

## Testing

- Use component harnesses over direct DOM queries
- Mock services with `jasmine.createSpyObj` or `jest.fn()`
- Test signal state changes and template output, not implementation details

## Architecture

- One feature per lazy-loaded route module
- Use facades when components orchestrate 3+ services with shared state
- Use environment-based configuration — no hardcoded URLs or API keys

## Quick Reference

| Pattern | Use | Avoid |
|---------|-----|-------|
| Signal inputs | `input<T>()` | `@Input()` |
| Signal outputs | `output<T>()` | `@Output()` |
| Dependency injection | `inject()` | Constructor injection |
| Control flow | `@if`, `@for`, `@switch` | `*ngIf`, `*ngFor` |
| Class binding | `[class.active]` | `[ngClass]` |
| Change detection | `OnPush` | Default |
| Derived state | `computed()` | Getters |

## Optional Library Skills

Install library-specific rules alongside this core skill:

| Library | Skill Page |
|---------|------------|
| NgRx | [angular-best-practices-ngrx](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices-ngrx) |
| SignalStore | [angular-best-practices-signalstore](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices-signalstore) |
| TanStack Query | [angular-best-practices-tanstack](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices-tanstack) |
| Angular Material | [angular-best-practices-material](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices-material) |
| PrimeNG | [angular-best-practices-primeng](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices-primeng) |
| Spartan UI | [angular-best-practices-spartan](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices-spartan) |
| Transloco | [angular-best-practices-transloco](https://skills.sh/alfredoperez/angular-best-practices/angular-best-practices-transloco) |

## Links

- [GitHub Repository](https://github.com/alfredoperez/angular-best-practices)
- [Submit a Rule](https://github.com/alfredoperez/angular-best-practices/issues/new) via GitHub Issues
- [Browse All Skills](https://skills.sh/alfredoperez/angular-best-practices)

## License

MIT
