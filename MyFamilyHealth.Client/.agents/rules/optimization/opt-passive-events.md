---
title: Use Passive Event Listeners
impact: LOW
impactDescription: improves scroll performance
tags: events, scroll, performance, passive
---

## Use Passive Event Listeners

Add `{ passive: true }` to `addEventListener()` for `scroll`, `wheel`, `touchstart`, `touchmove`, and `touchend` events. Passive listeners improve scroll performance by telling the browser that `preventDefault()` won't be called. Avoid when you need to call `preventDefault()`.
