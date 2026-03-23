---
title: Declare Return Types for Exported Functions
impact: LOW
impactDescription: Improves AI comprehension, serves as documentation
tags: typescript, functions, documentation
---

## Declare Return Types for Exported Functions

Add explicit return types to exported/public functions: `function getUser(id: string): User`.

This helps AI tools understand your API contracts and catches accidental return type changes. Internal/private functions can rely on inference.
