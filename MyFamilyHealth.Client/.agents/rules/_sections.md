# Sections

This file defines all rule categories, their ordering, impact levels, and descriptions.

---

## 1. Eliminating Waterfalls (async-, opt-async-)

**Impact:** CRITICAL (2-10× improvement)
**Description:** Execute independent async operations in parallel using Promise.all() or forkJoin().

## 2. Bundle Optimization (bundle-)

**Impact:** CRITICAL (Reduces initial load)
**Description:** Reduce bundle size through tree-shaking, dynamic imports, and optimized barrel files.

## 3. JavaScript Performance (opt-)

**Impact:** HIGH (Runtime performance)
**Description:** Optimize loops, data structures, caching, and DOM operations for better runtime performance.

## 4. TypeScript Best Practices (ts-)

**Impact:** MEDIUM (Type safety & maintainability)
**Description:** Type safety patterns that prevent bugs and improve code quality.

## 5. Signals & Reactivity (signal-)

**Impact:** HIGH (Reactive state management)
**Description:** Use Angular Signals for reactive local state with fine-grained updates.

## 6. Component Patterns (component-)

**Impact:** HIGH (Component architecture)
**Description:** Container/presentation, content projection, compound components, and host directives.

## 7. RxJS Patterns (rxjs-)

**Impact:** HIGH (Memory leaks & cancellation)
**Description:** Proper subscription management, multicasting, error handling, and operator selection.

## 8. Change Detection (cd-)

**Impact:** HIGH (Rendering performance)
**Description:** OnPush strategy, trackBy functions, async pipe, and zoneless patterns.

## 9. Template Optimization (template-)

**Impact:** HIGH (Lazy loading & pipes)
**Description:** Use @defer for lazy loading, pure pipes for transforms, and optimized control flow.

## 10. SSR & Hydration (ssr-)

**Impact:** HIGH (Initial render & SEO)
**Description:** Server-side rendering, hydration, TransferState, and platform checks.

## 11. Forms (form-)

**Impact:** MEDIUM (Form handling)
**Description:** Typed reactive forms, custom validators, and ControlValueAccessor.

## 12. NgRx State Management (ngrx-)

**Impact:** HIGH (Global state)
**Description:** Action groups, selectors, pure reducers, and entity adapter patterns.

## 13. SignalStore (signalstore-)

**Impact:** HIGH (Local/feature state)
**Description:** Feature stores with computed state, entities, and rxMethod.

## 14. TanStack Query (tanstack-)

**Impact:** HIGH (Server state)
**Description:** Query and mutation patterns for server state management.

## 15. Architecture (arch-)

**Impact:** HIGH (Scalability)
**Description:** DDD structure, module boundaries, and barrel file organization.

## 16. Testing (test-)

**Impact:** HIGH (Reliability)
**Description:** Unit tests, component tests, E2E tests, harnesses, and mocking patterns.

## 17. Infrastructure (core/)

**Impact:** MEDIUM (Cross-cutting concerns)
**Description:** Routing, security, error handling, and observability patterns.

## 18. UI & Accessibility (ui/, a11y-)

**Impact:** MEDIUM (User experience)
**Description:** Accessibility, loading states, dialogs, and theming.

## 19. Data Handling (http-, mappers-)

**Impact:** MEDIUM (API integration)
**Description:** HTTP client patterns and data transformation.

## 20. Angular Material (material-)

**Impact:** MEDIUM (UI components)
**Description:** Angular Material component usage, theming, CDK utilities, and test harnesses.

## 21. PrimeNG (primeng-)

**Impact:** MEDIUM (UI components)
**Description:** PrimeNG component imports, theming system, and table patterns.

## 22. Spartan UI (spartan-)

**Impact:** MEDIUM (Headless UI)
**Description:** Spartan Brain/Helm headless components, Tailwind integration, and accessibility.

## 23. Internationalization (transloco-)

**Impact:** MEDIUM (Internationalization)
**Description:** Transloco runtime i18n, lazy-loaded translations, and test mocking.

