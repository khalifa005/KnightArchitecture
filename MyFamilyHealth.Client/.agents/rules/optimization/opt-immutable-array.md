---
title: Use Immutable Array Methods
impact: MEDIUM
impactDescription: Cleaner code, no accidental mutations
tags: arrays, immutability, ES2023
---

## Use Immutable Array Methods

Use ES2023's `toSorted()`, `toReversed()`, and `toSpliced()` instead of mutating methods that require copying. Instead of `[...items].sort()`, use `items.toSorted()`. Instead of `[...items].reverse()`, use `items.toReversed()`. Instead of splice with a copy, use `items.toSpliced(index, 1)`.
