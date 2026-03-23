---
title: Run Non-Critical Work After Response
impact: MEDIUM
impactDescription: Faster Time to First Byte
tags: ssr, async, performance
---

## Run Non-Critical Work After Response

Send the response first, then perform analytics, logging, or cache warming fire-and-forget style without blocking TTFB.

```typescript
async handleRequest(req: Request) {
  const html = await this.render(req);
  // Fire-and-forget: don't await non-critical work
  this.analytics.track(req).catch(console.error);
  this.cache.warm(req).catch(console.error);
  return html;
}
```
