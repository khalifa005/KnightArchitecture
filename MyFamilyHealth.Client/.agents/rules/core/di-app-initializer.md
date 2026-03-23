---
title: Use provideAppInitializer for Startup Logic
impact: MEDIUM
impactDescription: Blocks bootstrap until ready
tags: dependency-injection, initialization, startup
---

## Use provideAppInitializer for Startup Logic

Use `provideAppInitializer()` (Angular 19+) or `{ provide: APP_INITIALIZER, useFactory: ..., multi: true }` (Angular 17-18) to run async setup code before the app renders.

**Incorrect:**

```typescript
// Startup logic in root component — app renders before init completes
export class AppComponent {
  constructor(private config: ConfigService) { config.load(); }
}
```

**Correct:**

```typescript
// App providers — blocks rendering until init resolves
export const appConfig: ApplicationConfig = {
  providers: [
    provideAppInitializer(() => inject(ConfigService).load()),
  ],
};
```
