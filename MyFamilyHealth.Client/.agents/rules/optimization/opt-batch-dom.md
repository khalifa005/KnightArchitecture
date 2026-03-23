---
title: Batch DOM Reads and Writes
impact: MEDIUM
impactDescription: avoids layout thrashing
tags: dom, performance, layout, optimization
---

## Batch DOM Reads and Writes

When manipulating the DOM directly, batch all reads together and all writes together to avoid layout thrashing.

**Incorrect (interleaved reads/writes):**

```typescript
for (const el of elements) {
  const height = el.offsetHeight; // READ - triggers layout
  el.style.height = `${height * 2}px`; // WRITE - invalidates layout
}
```

**Correct (batch reads, then writes):**

```typescript
const heights = elements.map(el => el.offsetHeight);
elements.forEach((el, i) => el.style.height = `${heights[i] * 2}px`);
```

**In Angular, prefer template bindings** which let Angular handle DOM updates efficiently. When direct DOM access is needed, use `requestAnimationFrame()`.

**Properties that trigger layout:** `offsetTop/Left/Width/Height`, `scrollTop/Left/Width/Height`, `clientTop/Left/Width/Height`, `getComputedStyle()`, `getBoundingClientRect()`
