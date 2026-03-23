# Angular Primeng Best Practices

> Use with the core `angular-best-practices` skill.

---

## 1. PrimeNG

**Impact: MEDIUM** (UI components)

### 1.1 Tree-Shake PrimeNG Imports

**Impact: MEDIUM** (Reduces bundle size)

Import PrimeNG components individually using standalone component imports. Avoid importing entire PrimeNG modules. Use the standalone API available since PrimeNG v17+.

**Incorrect:**

```typescript
// Importing full module pulls in all components
import { ButtonModule } from 'primeng/button';
@NgModule({ imports: [ButtonModule, TableModule, DialogModule] })
```

**Correct:**

```typescript
@Component({
  imports: [Button, Select], // Standalone components — only what's needed
})
```

### 1.2 Use PrimeNG Table with Lazy Loading

**Impact: HIGH** (Handles 100k+ rows efficiently)

Use `[lazy]="true"` with `(onLazyLoad)` for server-side pagination, sorting, and filtering. For large client-side datasets, enable `[virtualScroll]="true"` with `[virtualScrollItemSize]`.

**Incorrect:**

```html
<!-- Loads all 10,000 rows into DOM -->
<p-table [value]="allData"></p-table>
```

**Correct:**

```html
<p-table [value]="data" [lazy]="true" [totalRecords]="total"
         (onLazyLoad)="load($event)" [paginator]="true" [rows]="20">
</p-table>
```

### 1.3 Use PrimeNG Theme System

**Impact: MEDIUM** (Consistent design with design tokens)

Use PrimeNG's styled mode with Aura or Lara presets. Customize via design tokens in `providePrimeNG()` instead of overriding CSS classes. Use `dt()` function for accessing tokens in custom styles.

**Incorrect:**

```css
.p-button { background: #1976d2 !important; } /* Breaks theming */
```

**Correct:**

```typescript
providePrimeNG({
  theme: { preset: Aura, options: { darkModeSelector: '.dark' } }
})
```

---
