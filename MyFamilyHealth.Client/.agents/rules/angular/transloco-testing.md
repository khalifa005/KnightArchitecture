---
title: Mock Translations in Tests
impact: MEDIUM
impactDescription: Fast tests without HTTP translation loading
tags: transloco, testing, mocks
---

## Mock Translations in Tests

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
