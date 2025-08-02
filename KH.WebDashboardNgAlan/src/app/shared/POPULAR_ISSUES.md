# Popular Issues & Solutions

This document outlines common issues encountered during Angular development with custom form components, validation pipes, and circular dependencies. Each issue includes the problem description, root cause, and solution.

## 🚨 Issue 1: Infinite Loading (Circular Dependency)

### **Problem Description**
```
The application gets stuck in an infinite loading state with a spinning loader (cs-loader) that never disappears.
This commonly happens when navigating to specific routes or refreshing the browser.
```

### **Root Cause**
Circular dependency caused by including custom form input components in SHARED_IMPORTS:

```typescript
// ❌ PROBLEMATIC STRUCTURE
// shared-imports.ts
export const SHARED_IMPORTS = [
  // ... basic modules
  ...SHARED_FORM_INPUT_COMPONENTS  // ❌ This causes circular dependency
];

// custom-text-input.component.ts
import { FieldInvalidPipe, FieldErrorPipe } from '../../pipes'; // Direct import

// The problem:
// 1. Custom form components import validation pipes directly
// 2. SHARED_IMPORTS includes these components
// 3. When a component imports SHARED_IMPORTS, it gets the components
// 4. Components try to import pipes, creating circular reference
```

### **Debugging Steps**
1. **Check Browser Console** - Look for circular dependency errors
2. **Identify the Route** - Note which page causes the infinite loading
3. **Check Component Imports** - Verify if the component imports SHARED_IMPORTS only
4. **Check Template Usage** - Look for validation pipe usage in templates
5. **Check Network Tab** - See if any requests are stuck in pending state

### **Solution: Separate Import Strategy**
```typescript
// ✅ SOLUTION: Keep SHARED_IMPORTS clean
// shared-imports.ts
export const SHARED_IMPORTS = [
  FormsModule,
  ReactiveFormsModule,
  // ... basic modules only (NO custom components)
];

export const SHARED_PIPES = [FieldInvalidPipe, FieldErrorPipe];
export const SHARED_FORM_INPUT_COMPONENTS = [
  CustomTextInputComponent,
  CustomSelectInputComponent,
  // ...
];

// ✅ List components - Only need basic imports
import { SHARED_IMPORTS } from 'src/app/shared/shared-imports';
@Component({
  imports: [SHARED_IMPORTS]  // No pipes or custom components needed
})

// ✅ Form components - Only need custom components (pipes are included)
import { SHARED_IMPORTS, SHARED_FORM_INPUT_COMPONENTS } from 'src/app/shared/shared-imports';
@Component({
  imports: [
    ...SHARED_IMPORTS,
    ...SHARED_FORM_INPUT_COMPONENTS  // Pipes are already included in custom components
  ]
})
```

### **Prevention Tips**
- ✅ Always separate pipes and components into different import arrays
- ✅ Import only what you need in each component
- ✅ Use direct imports in components, not through shared-imports
- ✅ Test imports incrementally to catch circular dependencies early
- ✅ **Never include SHARED_FORM_INPUT_COMPONENTS in SHARED_IMPORTS**
- ✅ **Always import SHARED_PIPES separately when using validation pipes**

---

## 🚨 Issue 2: "Cannot find pipe 'fieldInvalid'" Error

### **Problem Description**
```
Error: No pipe found with name 'fieldInvalid'
```

### **Root Cause**
The validation pipe is not imported in the component where it's being used.

### **Solution**
```typescript
// ✅ SOLUTION: Import the pipe
import { SHARED_PIPES } from 'src/app/shared/shared-imports';

@Component({
  imports: [
    ...SHARED_IMPORTS,
    ...SHARED_PIPES  // Add this line
  ]
})
```

### **Prevention Tips**
- ✅ Always import SHARED_PIPES when using validation pipes
- ✅ Check component imports before using pipes in templates
- ✅ Use TypeScript strict mode to catch missing imports

---

## 🔧 Infinite Loading Debugging Guide

### **Step-by-Step Debugging Process**

When you encounter infinite loading on a specific route:

#### **1. Immediate Actions**
```bash
# 1. Open Browser DevTools (F12)
# 2. Go to Console tab
# 3. Go to Network tab
# 4. Refresh the problematic page
```

#### **2. Check Console for Errors**
Look for these specific error patterns:
- `Circular dependency detected`
- `Cannot resolve module`
- `No pipe found with name 'fieldInvalid'`
- `No component found with name 'app-custom-text-input'`

#### **3. Check Network Tab**
- Look for requests stuck in "pending" status
- Check if any module files are failing to load
- Look for infinite redirects

#### **4. Identify the Problematic Component**
Based on the route, identify which component is causing the issue:
```typescript
// Example: /user-management/list-user-profiles
// Problematic component: ListUserProfilesComponent
```

#### **5. Check Component Imports**
```typescript
// ❌ PROBLEMATIC - Missing pipes
@Component({
  imports: [SHARED_IMPORTS]  // Only basic imports
})

// ✅ SOLUTION - Include pipes
@Component({
  imports: [
    ...SHARED_IMPORTS,
    ...SHARED_PIPES  // Add this
  ]
})
```

#### **6. Check shared-imports.ts**
```typescript
// ❌ PROBLEMATIC - Includes form components
export const SHARED_IMPORTS = [
  // ... basic modules
  ...SHARED_FORM_INPUT_COMPONENTS  // This causes circular dependency
];

// ✅ SOLUTION - Only basic modules
export const SHARED_IMPORTS = [
  FormsModule,
  ReactiveFormsModule,
  // ... other basic modules only
];
```

#### **7. Quick Fix Template**
```typescript
// For any component with infinite loading:
import { SHARED_IMPORTS, SHARED_PIPES } from 'src/app/shared/shared-imports';

@Component({
  imports: [
    ...SHARED_IMPORTS,
    ...SHARED_PIPES
  ]
})
```

### **Common Routes That Cause Issues**
- `/user-management/list-user-profiles` - Missing SHARED_PIPES import
- `/user-management/user-details` - Missing SHARED_PIPES import
- Any route using validation pipes without proper imports

### **Prevention Checklist**
- [ ] Never include SHARED_FORM_INPUT_COMPONENTS in SHARED_IMPORTS
- [ ] Always import SHARED_PIPES separately when using validation pipes
- [ ] Test routes after making import changes
- [ ] Check browser console for circular dependency warnings
- [ ] Use the separate import strategy consistently

### **Emergency Fix for Infinite Loading**
If you need to fix infinite loading immediately:

1. **Find the problematic component** (based on the route)
2. **Add SHARED_PIPES import**:
   ```typescript
   import { SHARED_IMPORTS, SHARED_PIPES } from 'src/app/shared/shared-imports';
   
   @Component({
     imports: [
       ...SHARED_IMPORTS,
       ...SHARED_PIPES
     ]
   })
   ```
3. **Save and test** - The page should load immediately
4. **Check for other components** with the same issue

This debugging guide should help you quickly identify and fix infinite loading issues in your Angular application.
