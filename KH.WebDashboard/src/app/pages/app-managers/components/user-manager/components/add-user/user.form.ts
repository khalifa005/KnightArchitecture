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
  constructor(readonly fb: FormBuilder = new FormBuilder(), readonly model: CreateUserRequest) {
    super(
      fb.group({
        id: [model?.id],
        firstName: [model?.firstName, [Validators.required, Validators.minLength(2)]],
        middleName: [model?.middleName, [Validators.required, Validators.minLength(2)]],
        lastName: [model?.lastName, [Validators.required, Validators.minLength(2)]],
        email: [model?.email, [Validators.required, Validators.email]],
        username: [model?.username, [Validators.required]],
        password: [model?.password, [Validators.required, Validators.minLength(8)]],
        mobileNumber: [model?.mobileNumber, [Validators.required]],
        birthDate: [model?.birthDate, Validators.required],
        groupId: [model?.groupId, Validators.required],
        departmentId: [model?.departmentId, Validators.required],
        roleIds: [model?.roleIds, Validators.required],
      }).controls
    );
  }
}
