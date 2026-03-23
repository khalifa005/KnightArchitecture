# Angular Transloco Best Practices

> Use with the core `angular-best-practices` skill.

---

## 1. Internationalization

**Impact: MEDIUM** (Internationalization)

### 1.1 Lazy Load Translation Files per Route

**Impact: MEDIUM** (Reduces initial bundle with translations)

Use Transloco scopes to load translation files only when a route is activated. Define scopes at the route level using `provideTranslocoScope()` to avoid loading all translations upfront.

**Incorrect:**

```typescript
// Single large translation file loaded at startup
// en.json — 500+ keys for all features
```

**Correct:**

```typescript
{ path: 'admin', loadChildren: () => import('./admin/admin.routes'),
  providers: [provideTranslocoScope('admin')] }
// Loads admin/en.json only when visiting /admin
```

### 1.2 Mock Translations in Tests

**Impact: MEDIUM** (Fast tests without HTTP translation loading)

Provide inline translations in tests with `provideTranslocoTesting()` to avoid HTTP requests and ensure fast, deterministic runs.

**Incorrect:**

```typescript
// Loading real translation files in tests — slow, flaky, needs HTTP mock
TestBed.configureTestingModule({ providers: [provideTransloco({ /* full config */ })] });
```

**Correct:**

```typescript
TestBed.configureTestingModule({
  providers: [provideTranslocoTesting({
    langs: { en: { 'actions.save': 'Save' } }, defaultLang: 'en',
  })],
});
```

### 1.3 Use Transloco for Runtime i18n

**Impact: MEDIUM** (Dynamic language switching without rebuilds)

Use `provideTransloco()` for runtime internationalization with the `transloco` pipe in templates.

**Incorrect:**

```html
<!-- Hardcoded strings — no translation support -->
<h1>Welcome to the app</h1>
<button>Save Changes</button>
```

**Correct:**

```html
<h1>{{ 'home.welcome' | transloco }}</h1>
<button>{{ 'actions.save' | transloco }}</button>
```

---
