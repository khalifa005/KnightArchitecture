---
title: Use Domain-Driven Folder Structure
impact: MEDIUM
impactDescription: Better organization, clear boundaries
tags: architecture, ddd, structure
---

## Use Domain-Driven Folder Structure

Organize by domain (feature area) with layers inside each domain instead of grouping by technical type.

```
src/app/domains/
  customers/feature/  # Smart components
  customers/data/     # Services, state
  customers/ui/       # Presentation components
  customers/model/    # Types, interfaces
```
