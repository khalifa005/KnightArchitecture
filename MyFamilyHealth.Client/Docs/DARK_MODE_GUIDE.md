# Developer Guide: Supporting Dark Mode

This guide explains how to ensure new components in the Digital Health Memory (DHM) platform automatically support Dark Mode.

## 1. Use Material Design Tokens
Our Tailwind configuration is mapped to CSS custom properties that automatically swap values when the `.dark` class is applied to the `<html>` element. **Always prefer these tokens over hardcoded colors.**

### Common Background Tokens
| Light Value | Dark Value | Tailwind Class | Usage |
|-------------|------------|----------------|-------|
| `#ffffff` | `#0c0e13` | `bg-surface-container-lowest` | Main page/card background |
| `#f2f4f8` | `#191c20` | `bg-surface-container-low` | Section background |
| `#eceef2` | `#1d2024` | `bg-surface-container` | Input backgrounds / Dividers |

### Common Text Tokens
| Light Value | Dark Value | Tailwind Class | Usage |
|-------------|------------|----------------|-------|
| `#191c1f` | `#e0e2e7` | `text-on-surface` | Primary headings and text |
| `#40484f` | `#c0c7d0` | `text-on-surface-variant` | Secondary/hint text |
| `#004b71` | `#8ecdff` | `text-primary` | Link/Primary action text |

---

## 2. Accessibility & Contrast
When using Primary colors for backgrounds (e.g., buttons), **never hardcode `text-white`**. 

- **Incorrect**: `<button class="bg-primary text-white">`  
  *(Fails contrast in dark mode because primary becomes light blue)*
- **Correct**: `<button class="bg-primary text-on-primary">`  
  *(Automatically switches to dark text on light blue in dark mode)*

---

## 3. Custom Dark Mode Overrides
If a component needs a specific look that doesn't fit the token system, use the Tailwind `dark:` modifier.

```html
<div class="border-slate-200 dark:border-slate-800 bg-white dark:bg-slate-900">
  <!-- Content -->
</div>
```

---

## 4. Glassmorphism
For glass panels, use the `.glass-panel` class in CSS/SCSS which has built-in dark mode support via `:host-context(.dark)`.

```scss
.glass-panel {
  background: rgba(255, 255, 255, 0.4);
  backdrop-filter: blur(20px);
  
  :host-context(.dark) & {
    background: rgba(15, 23, 42, 0.6);
  }
}
```

---

## 5. Persistence & State
The theme state is managed by `AppStore`. You can access it in any component:

```typescript
private appStore = inject(AppStore);
isDarkMode = this.appStore.darkMode; // Signal<boolean>
```

The preference is automatically saved to `localStorage` under the key `dhm-dark-mode`.
