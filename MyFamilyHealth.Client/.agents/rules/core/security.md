---
title: Security Patterns
impact: HIGH
impactDescription: Prevents vulnerabilities
tags: security, auth, xss, csrf
---

## Security Patterns

Never store tokens in `localStorage` (XSS-vulnerable). Use in-memory signals for access tokens and `HttpOnly` cookies for refresh tokens. Trust Angular's sanitization; avoid bypassing `DomSanitizer`. Enable CSRF with `withXsrfConfiguration()`.

**Incorrect:**
```typescript
localStorage.setItem('accessToken', token); // Vulnerable to XSS
```

**Correct:**
```typescript
private accessToken = signal<string | null>(null); // In-memory only
provideHttpClient(withXsrfConfiguration({ cookieName: 'XSRF-TOKEN', headerName: 'X-XSRF-TOKEN' }));
```
