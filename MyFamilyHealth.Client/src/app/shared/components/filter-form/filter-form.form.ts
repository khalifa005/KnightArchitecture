import { FormBuilder, FormGroup, Validators } from '@angular/forms';

export function buildFeatureFilterForm(fb: FormBuilder): FormGroup {
  return fb.group({
    searchQuery: ['', []],
    dateRange: [null, []],
    type: [null, []],
    provider: [null, []]
  });
}
