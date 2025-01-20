import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { CreateUserRequest } from 'src/open-api';

export class UserForm extends FormGroup {
  readonly firstNameControl = this.get('firstName') as FormControl;
  readonly middleNameControl = this.get('middleName') as FormControl;
  readonly lastNameControl = this.get('lastName') as FormControl;
  readonly emailControl = this.get('email') as FormControl;
  readonly usernameControl = this.get('username') as FormControl;

  readonly mobileNumberControl = this.get('mobileNumber') as FormControl;
  readonly birthDateControl = this.get('birthDate') as FormControl;
  readonly passwordControl = this.get('password') as FormControl;
  readonly departmentIdControl = this.get('departmentId') as FormControl;
  readonly roleIdsControl = this.get('roleIds') as FormControl;
  constructor(
   readonly fb: FormBuilder = new FormBuilder(),
   readonly model: CreateUserRequest,
   readonly isEditMode: boolean = false // New parameter to indicate edit mode

  ) {
    super(
      fb.group({
        id: [model?.id],
        firstName: [model?.firstName, [Validators.required, Validators.minLength(2)]],
        middleName: [model?.middleName, [Validators.required, Validators.minLength(2)]],
        lastName: [model?.lastName, [Validators.required, Validators.minLength(2)]],
        email: [model?.email, [Validators.required, Validators.email]],

        // username: [model?.username, [Validators.required]],
        // password: [model?.password, [Validators.required, Validators.minLength(8)]],
        
        username: [
          model?.username,
          isEditMode ? [] : [Validators.required], // Remove required if in edit mode and username exists
        ],
        password: [
          model?.password,
          isEditMode ? [] : [Validators.required, Validators.minLength(8)], // Remove required if in edit mode and password exists
        ],

        mobileNumber: [model?.mobileNumber, [Validators.required]],
        birthDate: [model?.birthDate, Validators.required],
        // groupId: [model?.groupId, Validators.required],
        departmentId: [model?.departmentId, Validators.required],
        roleIds: [model?.roleIds, Validators.required],
      }).controls
    );
  }
}
