---
title: Prefer Interface Extends Over Intersections
impact: LOW
impactDescription: Better performance, clearer error messages
tags: typescript, interfaces, types
---

## Prefer Interface Extends Over Intersections

Use `interface Dog extends Animal` instead of `type Dog = Animal & { breed: string }`. Interface extends produces clearer error messages (shows "Dog" vs full intersection) and has better TypeScript compiler performance.
