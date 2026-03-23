# Angular Spartan Best Practices

> Use with the core `angular-best-practices` skill.

---

## 1. Spartan UI

**Impact: MEDIUM** (Headless UI)

### 1.1 Configure Tailwind with Spartan Helm

**Impact: MEDIUM** (Consistent utility-first styling)

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

### 1.2 Use Spartan Brain for Accessible Behavior

**Impact: MEDIUM** (WCAG-compliant interactions for free)

Spartan Brain directives handle ARIA attributes, keyboard navigation, and focus management automatically. Use Brain primitives (`brnAccordion`, `brnMenu`, `brnTabs`) instead of building custom a11y logic.

**Incorrect:**

```typescript
// Custom tabs — missing aria-selected, arrow key navigation, focus management
@Component({ template: '@for (t of tabs; track t.id) { <div (click)="select(t)">{{t.label}}</div> }' })
```

**Correct:**

```html
<brn-tabs defaultValue="tab1">
  <brn-tabs-list hlmTabsList>
    <button brnTabsTrigger="tab1" hlmTabsTrigger>Tab 1</button>
  </brn-tabs-list>
</brn-tabs>
```

### 1.3 Use Spartan UI Headless Components

**Impact: MEDIUM** (Full styling control with a11y built-in)

Use Spartan's Brain (headless) directives for behavior and accessibility, paired with Helm (styled) components for UI. Install only the primitives you need via `@spartan-ng/brain/*`.

**Incorrect:**

```typescript
// Building custom dialog from scratch — missing a11y, focus trap, esc handling
@Component({ template: '<div class="overlay"><div class="panel">...</div></div>' })
```

**Correct:**

```html
<brn-dialog>
  <brn-dialog-trigger><button hlmBtn>Open</button></brn-dialog-trigger>
  <brn-dialog-content hlmDialogContent>Content here</brn-dialog-content>
</brn-dialog>
```

---
