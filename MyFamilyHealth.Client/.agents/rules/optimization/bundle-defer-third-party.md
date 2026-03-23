---
title: Defer Heavy Third-Party Libraries
impact: MEDIUM
impactDescription: Reduces initial bundle size
tags: bundle, third-party, lazy-loading, defer
---

## Defer Heavy Third-Party Libraries

Load heavy libraries (Chart.js, PDF.js, Leaflet, Quill) only when needed using `@defer` or dynamic imports.

```typescript
async generatePdf(content: string) {
  const { jsPDF } = await import('jspdf');
  return new jsPDF().text(content, 10, 10);
}
```
