---
title: Configure Tailwind with Spartan Helm
impact: MEDIUM
impactDescription: Consistent utility-first styling
tags: spartan, tailwind, helm, styling
---

## Configure Tailwind with Spartan Helm

Use Spartan Helm components with Tailwind CSS. Configure the `hlm` plugin in `tailwind.config.js`. Use `hlmInput`, `hlmBtn`, `hlmCard` directives to apply consistent Tailwind-based styles.

**Incorrect:**

```html
<!-- Manual Tailwind classes — inconsistent, no design system -->
<button class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600">Save</button>
```

**Correct:**

```html
<button hlmBtn variant="default">Save</button>
<button hlmBtn variant="outline" size="sm">Cancel</button>
```
