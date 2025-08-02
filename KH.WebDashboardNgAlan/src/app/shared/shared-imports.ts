import { AsyncPipe, DatePipe, JsonPipe, NgClass, NgStyle, NgTemplateOutlet } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterOutlet, RouterLink } from '@angular/router';
import { I18nPipe } from '@delon/theme';

import { SHARED_DELON_MODULES } from './shared-delon.module';
import { SHARED_ZORRO_MODULES } from './shared-zorro.module';

// Import custom pipes and form components
import { SHARED_FORM_INPUT_COMPONENTS } from './shared-form-inputs.module';
import { SHARED_PIPES } from './shared-pipes.module';

// Group custom pipes

// ✅ SOLUTION: Keep SHARED_IMPORTS clean - basic modules only
export const SHARED_IMPORTS = [
  FormsModule,
  ReactiveFormsModule,
  RouterLink,
  RouterOutlet,
  NgTemplateOutlet,
  NgClass,
  NgStyle,
  I18nPipe,
  JsonPipe,
  DatePipe,
  AsyncPipe,
  ...SHARED_PIPES,
  // ❌ REMOVED: ...SHARED_FORM_INPUT_COMPONENTS, // This causes circular dependency
  ...SHARED_DELON_MODULES,
  ...SHARED_ZORRO_MODULES
];

// ✅ Export form components separately for components that need them
export { SHARED_FORM_INPUT_COMPONENTS } from './shared-form-inputs.module';
export { SHARED_PIPES } from './shared-pipes.module';
