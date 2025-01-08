import { FormGroup, FormControl, FormBuilder, Validators } from "@angular/forms";
import { StoreFileParameter } from "../../../../@core/models/client/store-file-parameters";
import { fileExtensionsValidator, RquiredFileUploadValidationSizeInMB } from "../../../../@core/utils.ts/form/validations/form.validation-helpers";

const allowedExtensions: string[] = ['csv', 'xlsx', 'png', 'jpg', 'pdf', 'mp4'];

export class UploadFilesForm extends FormGroup {

  readonly File = this.get('File') as FormControl;
  readonly NOTES = this.get('NOTES') as FormControl;

  constructor(readonly model: StoreFileParameter, readonly fb: FormBuilder = new FormBuilder())
  {
    super(

      fb.group({
        File: [model?.files, [fileExtensionsValidator(allowedExtensions), RquiredFileUploadValidationSizeInMB(20), Validators.required]],
        NOTES: [model?.NOTES],
    }).controls

    );
  }
}
