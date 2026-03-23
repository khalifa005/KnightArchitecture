---
title: Use Readonly Properties by Default
impact: MEDIUM
impactDescription: Prevents accidental mutation
tags: typescript, readonly, immutability
---

## Use Readonly Properties by Default

Mark properties as `readonly` by default; omit only when mutation is intentional. Use `Readonly<T>` for objects and `readonly T[]` or `ReadonlyArray<T>` for arrays to catch accidental mutations at compile time.
