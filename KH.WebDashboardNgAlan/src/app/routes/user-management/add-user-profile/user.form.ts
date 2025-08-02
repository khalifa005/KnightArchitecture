import { FormGroup, FormControl, FormBuilder, Validators } from "@angular/forms";

export class UserForm extends FormGroup {

   readonly idControl = this.get('id') as FormControl;
   readonly usernameControl = this.get('username') as FormControl;
   readonly emailControl = this.get('email') as FormControl;
   readonly phoneNumberControl = this.get('phone_number') as FormControl;
   readonly nationalIdControl = this.get('national_id') as FormControl;
   readonly branchControl = this.get('branch') as FormControl;
   readonly nameArControl = this.get('name_ar') as FormControl;
   readonly nameEnControl = this.get('name_en') as FormControl;
   readonly roleControl = this.get('role') as FormControl;
   readonly avatarControl = this.get('avatar') as FormControl;
   readonly isActiveControl = this.get('is_active') as FormControl;

   constructor(readonly fb: FormBuilder = new FormBuilder(), readonly model: any) {
      super(
         fb.group({
            id: [model?.id],
            username: [model?.username, [Validators.required, Validators.maxLength(50), Validators.minLength(3)]],
            email: [model?.email, [Validators.required, Validators.email]],
            phone_number: [model?.phone_number, [Validators.required, Validators.pattern(/^(\+20|0)?1[0125][0-9]{8}$/)]],
            national_id: [model?.national_id, [Validators.required, Validators.pattern(/^[0-9]{14}$/)]],
            branch: [model?.branch, [Validators.required]],
            name_ar: [model?.name_ar, [Validators.required, Validators.maxLength(100), Validators.minLength(2)]],
            name_en: [model?.name_en, [Validators.required, Validators.maxLength(100), Validators.minLength(2)]],
            role: [model?.role, [Validators.required]],
            avatar: [model?.avatar],
            is_active: [model?.is_active ?? true],
         }).controls
      );

      console.log('form model', this.model);
   }

   /**
    * Validates branch field based on selected role
    * @param selectedRole - The selected role value
    * @param userBranch - The branch from user settings (for non-system-admin roles)
    */
   validateBranchByRole(selectedRole: string, userBranch?: string): void {
      if (selectedRole === 'system-admin') {
         // For system-admin: branch is required and editable
         this.branchControl.enable();
         this.branchControl.setValidators([Validators.required]);
         this.branchControl.setValue('');
      } else {
         // For non-system-admin: branch is disabled and set from user settings
         this.branchControl.disable();
         this.branchControl.clearValidators();

         // Set branch from user settings
         const branchValue = userBranch || this.branchControl.value;
         this.branchControl.setValue(branchValue);
      }

      this.branchControl.updateValueAndValidity();
   }

   /**
    * Checks if the current form state is valid for the selected role
    * @param selectedRole - The selected role value
    * @returns boolean indicating if the form is valid for the role
    */
   isFormValidForRole(selectedRole: string): boolean {
      if (selectedRole === 'system-admin') {
         // For system-admin, branch must be filled
         return this.valid && this.branchControl.value;
      } else {
         // For non-system-admin, branch is auto-filled, so just check overall validity
         return this.valid;
      }
   }

   /**
    * Gets validation errors for the current role
    * @param selectedRole - The selected role value
    * @returns object with validation errors
    */
   getRoleValidationErrors(selectedRole: string): any {
      const errors: any = {};

      if (selectedRole === 'system-admin' && !this.branchControl.value) {
         errors.branch = 'Branch is required for system-admin role';
      }

      return errors;
   }
} 