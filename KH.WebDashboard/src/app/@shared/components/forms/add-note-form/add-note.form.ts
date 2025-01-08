import { FormGroup, FormControl, FormBuilder, Validators } from "@angular/forms";

export class AddNoteForm extends FormGroup {

  readonly Notes = this.get('Notes') as FormControl;
  // readonly File = this.get('File') as FormControl;

  constructor(readonly model: any, readonly fb: FormBuilder = new FormBuilder())
  {
    super(

      fb.group({

        Notes: [model?.notes, [Validators.required]],
        // File: [model?.file, [Validators.required]],

    }).controls

    );
  }
}
