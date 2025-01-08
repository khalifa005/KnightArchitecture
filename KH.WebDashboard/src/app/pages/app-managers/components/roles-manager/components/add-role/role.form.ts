import { FormGroup, FormControl, FormBuilder, Validators } from "@angular/forms";
import { CraeteLookupRequest } from "@app/@core/models/base/request/create-lookup.request";

export class RoleForm extends FormGroup {

  readonly nameEnControl = this.get('nameEn') as FormControl;
  readonly nameArControl = this.get('nameAr') as FormControl;
  readonly descriptionControl = this.get('description') as FormControl;

  constructor(readonly fb: FormBuilder = new FormBuilder(), readonly model: CraeteLookupRequest)
  {
    super(
      fb.group({
        id: [model?.id],
        nameEn: [model?.nameEn, [Validators.required, Validators.maxLength(20), Validators.minLength(4) ]],
        nameAr: [model?.nameAr, [Validators.required, Validators.maxLength(20), Validators.minLength(4)]],
        description: [model?.description , [ Validators.required, Validators.maxLength(40), Validators.minLength(4)]],
    }).controls

    );
  }
}
