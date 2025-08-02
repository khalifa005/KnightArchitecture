# Custom Form Input Components

This directory contains reusable form input components that provide consistent styling and validation across the application.

## Components

### CustomTextInputComponent
A wrapper for text, email, password, and number inputs with built-in validation.

```typescript
<app-custom-text-input
  [control]="userForm.usernameControl"
  label="user.form.fields.username"
  placeholder="user.form.fields.username"
  fieldName="username"
  [required]="true"
  inputType="text"
  direction="ltr">
</app-custom-text-input>
```

**Properties:**
- `control`: FormControl instance
- `label`: Label text (supports i18n)
- `placeholder`: Placeholder text (supports i18n)
- `fieldName`: Field name for validation messages
- `required`: Whether the field is required
- `inputType`: 'text' | 'email' | 'password' | 'number' (default: 'text')
- `direction`: 'ltr' | 'rtl' (default: 'ltr')

### CustomSelectInputComponent
A wrapper for select dropdowns with built-in validation.

```typescript
<app-custom-select-input
  [control]="userForm.roleControl"
  label="user.form.fields.role"
  placeholder="user.form.fields.role"
  fieldName="role"
  [options]="roleOptions"
  [required]="true">
</app-custom-select-input>

<!-- With disabled state -->
<app-custom-select-input
  [control]="userForm.branchControl"
  label="user.form.fields.branch"
  placeholder="user.form.fields.branch"
  fieldName="branch"
  [options]="branchOptions"
  [required]="!isBranchDisabled"
  [disabled]="isBranchDisabled">
</app-custom-select-input>
```

**Properties:**
- `control`: FormControl instance
- `label`: Label text (supports i18n)
- `placeholder`: Placeholder text (supports i18n)
- `fieldName`: Field name for validation messages
- `required`: Whether the field is required
- `options`: Array of SelectOption objects
- `allowClear`: Whether to show clear button (default: true)
- `disabled`: Whether the field is disabled (default: false)

### CustomSwitchInputComponent
A wrapper for switch/toggle inputs.

```typescript
<app-custom-switch-input
  [control]="userForm.isActiveControl"
  label="user.form.fields.is_active">
</app-custom-switch-input>
```

**Properties:**
- `control`: FormControl instance
- `label`: Label text (supports i18n)

### CustomDateInputComponent
A wrapper for date picker inputs with built-in validation.

```typescript
<app-custom-date-input
  [control]="userForm.birthDateControl"
  label="user.form.fields.birth_date"
  placeholder="user.form.fields.birth_date"
  fieldName="birth_date"
  [required]="true"
  dateFormat="yyyy-MM-dd"
  [showTime]="false">
</app-custom-date-input>
```

**Properties:**
- `control`: FormControl instance
- `label`: Label text (supports i18n)
- `placeholder`: Placeholder text (supports i18n)
- `fieldName`: Field name for validation messages
- `required`: Whether the field is required
- `dateFormat`: Date format string (default: 'yyyy-MM-dd')
- `showTime`: Whether to show time picker (default: false)

## Validation Pipes

### FieldInvalidPipe
Checks if a form control is invalid and has been touched or modified.

```typescript
[ngClass]="{'error': control | fieldInvalid}"
```

### FieldErrorPipe
Returns the appropriate error message for a form control.

```typescript
[errorMessage]="control | fieldError:'fieldName'"
```

## Usage Example

```typescript
// In your component
export class MyFormComponent {
  userForm = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.minLength(3)]),
    email: new FormControl('', [Validators.required, Validators.email]),
    role: new FormControl('', [Validators.required]),
    isActive: new FormControl(true)
  });

  roleOptions = [
    { value: 'admin', label: 'Administrator' },
    { value: 'user', label: 'User' }
  ];
}
```

```html
<!-- In your template -->
<form [formGroup]="userForm">
  <app-custom-text-input
    [control]="userForm.get('username')"
    label="Username"
    fieldName="username"
    [required]="true">
  </app-custom-text-input>

  <app-custom-text-input
    [control]="userForm.get('email')"
    label="Email"
    fieldName="email"
    inputType="email"
    [required]="true">
  </app-custom-text-input>

  <app-custom-select-input
    [control]="userForm.get('role')"
    label="Role"
    fieldName="role"
    [options]="roleOptions"
    [required]="true">
  </app-custom-select-input>

  <app-custom-switch-input
    [control]="userForm.get('isActive')"
    label="Active Status">
  </app-custom-switch-input>
</form>
```

## Import Strategy

To avoid circular dependencies, use the separate import strategy:

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

For detailed information, see `src/app/shared/IMPORT_STRATEGY.md`.

## Global Styles

The components use global styles defined in `src/styles/form-styles.less` which provide:
- Consistent form field styling with light gray background (`#f8f9fa`)
- Rounded input borders (8px border-radius)
- Hover and focus states with smooth transitions
- Responsive design for mobile devices
- Card styling for form containers with shadows
- Button styling with gradients and animations
- Avatar section styling with hover effects
- Form actions styling with background and borders
- Error state styling for validation
- All styling is centralized and reusable across the application 