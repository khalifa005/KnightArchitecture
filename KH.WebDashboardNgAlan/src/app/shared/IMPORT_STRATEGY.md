# Import Strategy: Avoiding Circular Dependencies

This document explains the strategy used to avoid circular dependencies between custom form input components and validation pipes.

## Problem Solved

**Circular Dependency Issue:**
- Custom form input components were importing validation pipes
- Shared imports were importing both components and pipes
- This created a circular dependency causing infinite loading

## Solution: Separate Import Strategy

### 1. **SHARED_PIPES** - Validation Pipes Only
```typescript
// src/app/shared/shared-imports.ts
export const SHARED_PIPES = [
  FieldInvalidPipe,
  FieldErrorPipe
];
```

### 2. **SHARED_FORM_INPUT_COMPONENTS** - Form Components Only
```typescript
// src/app/shared/shared-imports.ts
export const SHARED_FORM_INPUT_COMPONENTS = [
  CustomTextInputComponent,
  CustomSelectInputComponent,
  CustomSwitchInputComponent,
  CustomDateInputComponent
];
```

### 3. **Individual Component Imports** - Import Only What You Need

#### Option A: Import Everything (Full Features)
```typescript
import { SHARED_IMPORTS, SHARED_PIPES, SHARED_FORM_INPUT_COMPONENTS } from 'src/app/shared/shared-imports';

@Component({
  imports: [
    ...SHARED_IMPORTS,
    ...SHARED_PIPES,
    ...SHARED_FORM_INPUT_COMPONENTS
  ]
})
```

#### Option B: Import Only Pipes (For Regular Forms)
```typescript
import { SHARED_IMPORTS, SHARED_PIPES } from 'src/app/shared/shared-imports';

@Component({
  imports: [
    ...SHARED_IMPORTS,
    ...SHARED_PIPES
  ]
})
```

#### Option C: Import Only Form Components (If Pipes Already in SHARED_IMPORTS)
```typescript
import { SHARED_IMPORTS, SHARED_FORM_INPUT_COMPONENTS } from 'src/app/shared/shared-imports';

@Component({
  imports: [
    ...SHARED_IMPORTS,
    ...SHARED_FORM_INPUT_COMPONENTS
  ]
})
```

## Usage Examples

### Example 1: Using Custom Form Input Components
```typescript
// Component that needs both pipes and custom form inputs
import { SHARED_IMPORTS, SHARED_PIPES, SHARED_FORM_INPUT_COMPONENTS } from 'src/app/shared/shared-imports';

@Component({
  imports: [
    ...SHARED_IMPORTS,
    ...SHARED_PIPES,
    ...SHARED_FORM_INPUT_COMPONENTS
  ]
})
export class MyFormComponent {
  // Use custom form input components
  // <app-custom-text-input [control]="formControl" ...>
}
```

### Example 2: Using Only Validation Pipes
```typescript
// Component that only needs validation pipes
import { SHARED_IMPORTS, SHARED_PIPES } from 'src/app/shared/shared-imports';

@Component({
  imports: [
    ...SHARED_IMPORTS,
    ...SHARED_PIPES
  ]
})
export class MySimpleFormComponent {
  // Use pipes directly in template
  // [nzValidateStatus]="(control | fieldInvalid) ? 'error' : ''"
  // [nzErrorTip]="control | fieldError:'fieldName'"
}
```

### Example 3: Using Only Basic Shared Imports
```typescript
// Component that only needs basic shared imports
import { SHARED_IMPORTS } from 'src/app/shared/shared-imports';

@Component({
  imports: [SHARED_IMPORTS]
})
export class MyBasicComponent {
  // Basic functionality only
}
```

## Benefits of This Strategy

### ✅ **No Circular Dependencies**
- Pipes and components are imported separately
- No infinite loading issues
- Clean dependency graph

### ✅ **Flexible Imports**
- Import only what you need
- Reduce bundle size for components that don't need everything
- Better performance

### ✅ **Maintainable**
- Clear separation of concerns
- Easy to understand what each component needs
- Simple to add new pipes or components

### ✅ **Scalable**
- Easy to add new validation pipes
- Easy to add new form input components
- No breaking changes when adding new features

## File Structure

```
src/app/shared/
├── shared-imports.ts          # Main import orchestrator
├── pipes/
│   ├── field-validation.pipe.ts
│   ├── field-error.pipe.ts
│   └── index.ts
├── components/
│   └── form-inputs/
│       ├── custom-text-input.component.ts
│       ├── custom-select-input.component.ts
│       ├── custom-switch-input.component.ts
│       ├── custom-date-input.component.ts
│       └── index.ts
└── IMPORT_STRATEGY.md         # This file
```

## Best Practices

### 1. **Always Import SHARED_IMPORTS First**
```typescript
imports: [
  ...SHARED_IMPORTS,  // Always first
  ...SHARED_PIPES,    // Then pipes
  ...SHARED_FORM_INPUT_COMPONENTS  // Then components
]
```

### 2. **Use Specific Imports for Performance**
- Only import what you actually use
- Don't import SHARED_FORM_INPUT_COMPONENTS if you're not using custom form inputs

### 3. **Keep Components Lightweight**
- Custom form input components import pipes directly
- No need to import through shared-imports in components

### 4. **Document Your Imports**
- Add comments explaining why specific imports are needed
- Makes it easier for other developers to understand

## Migration Guide

### From Old Approach to New Strategy

**Before (Circular Dependency):**
```typescript
// ❌ This caused circular dependency
import { SHARED_IMPORTS } from 'src/app/shared/shared-imports';
// SHARED_IMPORTS included both pipes and components
```

**After (Clean Separation):**
```typescript
// ✅ Clean, no circular dependency
import { SHARED_IMPORTS, SHARED_PIPES, SHARED_FORM_INPUT_COMPONENTS } from 'src/app/shared/shared-imports';

@Component({
  imports: [
    ...SHARED_IMPORTS,
    ...SHARED_PIPES,
    ...SHARED_FORM_INPUT_COMPONENTS
  ]
})
```

## Troubleshooting

### Issue: "Cannot find pipe 'fieldInvalid'"
**Solution:** Import SHARED_PIPES
```typescript
import { SHARED_PIPES } from 'src/app/shared/shared-imports';
```

### Issue: "Cannot find component 'app-custom-text-input'"
**Solution:** Import SHARED_FORM_INPUT_COMPONENTS
```typescript
import { SHARED_FORM_INPUT_COMPONENTS } from 'src/app/shared/shared-imports';
```

### Issue: Infinite Loading
**Solution:** Check for circular imports and use the separate import strategy outlined above.

## Conclusion

This import strategy provides a clean, maintainable, and scalable solution for avoiding circular dependencies while maintaining all the functionality of custom form input components and validation pipes. 