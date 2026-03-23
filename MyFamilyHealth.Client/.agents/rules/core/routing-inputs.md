---
title: Use Route Input Binding
impact: MEDIUM
impactDescription: Cleaner component code, no ActivatedRoute
tags: routing, inputs, params
---

## Use Route Input Binding

Enable `withComponentInputBinding()` in router config to bind route params, query params, and resolver data directly to component inputs. Use `id = input.required<string>()` for `:id` params instead of injecting `ActivatedRoute`.
