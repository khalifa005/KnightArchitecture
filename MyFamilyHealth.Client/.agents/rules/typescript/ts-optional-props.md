---
title: Prefer Explicit Undefined Over Optional
impact: MEDIUM
impactDescription: Catches missing property bugs at compile time
tags: typescript, optional, undefined
---

## Prefer Explicit Undefined Over Optional

Use `prop: T | undefined` instead of `prop?: T` when omission is a bug. Optional props (`?`) allow complete omission, while `T | undefined` requires explicit `undefined` to signal intentional absence.
