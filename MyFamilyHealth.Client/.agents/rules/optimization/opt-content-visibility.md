---
title: Use content-visibility for Off-Screen Content
impact: HIGH
impactDescription: 10x faster initial render for long pages
tags: css, rendering, performance
---

## Use content-visibility for Off-Screen Content

Apply `content-visibility: auto` to skip rendering of off-screen content until scrolled into view. Add `contain-intrinsic-size` to reserve space for scroll calculations: `.card { content-visibility: auto; contain-intrinsic-size: 0 200px; }`.
