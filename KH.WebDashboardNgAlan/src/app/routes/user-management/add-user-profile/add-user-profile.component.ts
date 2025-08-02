import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { NzMessageService } from 'ng-zorro-antd/message';
import { ALAIN_I18N_TOKEN, SettingsService } from '@delon/theme';
import { Subject, takeUntil, Subscription, of, delay } from 'rxjs';
import { SHARED_IMPORTS, SHARED_FORM_INPUT_COMPONENTS } from '@shared';
import { UserForm } from './user.form';
import { UserService, User } from '../user.service';

@Component({
  selector: 'app-add-user-profile',
  imports: [
    ...SHARED_IMPORTS,
    ...SHARED_FORM_INPUT_COMPONENTS
  ],
  templateUrl: './add-user-profile.component.html',
  styleUrl: './add-user-profile.component.less'
})
export class AddUserProfileComponent implements OnInit, OnDestroy {
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  private readonly fb = inject(FormBuilder);
  private readonly modalRef = inject(NzModalRef);
  private readonly msg = inject(NzMessageService);
  private readonly i18nSrv = inject(ALAIN_I18N_TOKEN);
  private readonly userService = inject(UserService);
  private readonly settingsService = inject(SettingsService);

  // Input parameters
  userId?: number;
  mode: 'add' | 'edit' = 'add';

  // Form
  userForm!: UserForm;
  loading = false;
  avatarUrl?: string;
  avatarFile?: File; // Store the actual file for FormData

  // Branch field state
  isBranchDisabled = false;
  currentUserRole?: string;
  currentUserDepartment?: string;
  currentUserBranch?: string;

  // Options for dropdowns
  roles = [
    'Doctor',
    'Nurse',
    'Receptionist',
    'Pharmacist',
    'Lab Technician',
    'Radiologist',
    'Administrator',
    'Accountant',
    'Security Guard',
    'Cleaner',
    'system-admin'
  ];

  branches = [
    'Cairo Main Branch',
    'Alexandria Branch',
    'Giza Branch',
    'Sharm El Sheikh Branch',
    'Hurghada Branch',
    'Luxor Branch',
    'Aswan Branch',
    'Port Said Branch',
    'Suez Branch',
    'Ismailia Branch'
  ];

  // Convert arrays to SelectOption format for custom select components
  get roleOptions() {
    return this.roles.map(role => ({ value: role, label: role }));
  }

  get branchOptions() {
    return this.branches.map(branch => ({ value: branch, label: branch }));
  }

  ngOnInit(): void {
    // Get current user settings
    this.currentUserRole = this.settingsService.user.role;
    this.currentUserDepartment = this.settingsService.user.department;
    this.currentUserBranch = this.settingsService.user.branch;

    console.log('Current user role:', this.currentUserRole);
    console.log('Current user department:', this.currentUserDepartment);

    // Get modal data from parent component
    const modalData = this.modalRef.getConfig().nzData;
    console.log('Modal data received:', modalData);

    if (modalData) {
      this.userId = modalData.userId;
      this.mode = modalData.mode || 'add';
      console.log('Component initialized with:', { userId: this.userId, mode: this.mode });
    }

    this.initializeForm();
    if (this.mode === 'edit' && this.userId) {
      console.log('Loading user data for ID:', this.userId);
      this.loadUserData();
    } else {
      // Set default avatar for new users
      this.avatarUrl = 'https://api.dicebear.com/7.x/avataaars/svg?seed=default';
    }
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  private initializeForm(): void {
    this.userForm = new UserForm(this.fb, {});

    // Set up role change listener for dependency validation
    this.userForm.roleControl.valueChanges
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(selectedRole => {
        this.handleRoleChange(selectedRole);
      });

    // Initial role validation
    this.handleRoleChange(this.userForm.roleControl.value);
  }

  private handleRoleChange(selectedRole: string): void {
    console.log('Role changed to:', selectedRole);
    // Use the form's validation method
    this.userForm.validateBranchByRole(selectedRole, this.currentUserBranch);
    // Update the disabled state for the UI
    // this.isBranchDisabled = selectedRole !== 'super-admin';
    this.isBranchDisabled = selectedRole !== 'system-admin';
  }

  private getBranchFromUserSettings(): string {
    // Map department to branch or use a default
    if (this.currentUserDepartment) {
      // You can implement your own mapping logic here
      // For now, using the department as branch
      return this.currentUserDepartment;
    }

    // Default branch if no department is set
    return 'Cairo Main Branch';
  }

  private loadUserData(): void {
    if (this.userId) {
      console.log('Starting to load user data for ID:', this.userId);
      // Load user data from the service
      this.loading = true;

      // Get user data from the service
      this.userService.getUserById(this.userId).subscribe({
        next: (user: User | null) => {
          console.log('User data received:', user);
          if (user) {
            // Ensure form is initialized before patching
            if (!this.userForm) {
              this.initializeForm();
            }

            console.log('Patching form with user data:', user);
            this.userForm.patchValue(user);
            this.avatarUrl = user.avatar;

            // Apply role-based validation after patching user data
            const selectedRole = this.userForm.roleControl.value;
            this.handleRoleChange(selectedRole);

            this.loading = false;
            console.log('User data successfully loaded and form populated');
            console.log('Form values after patching:', this.userForm.value);
          } else {
            console.error('User not found for ID:', this.userId);
            this.msg.error('User not found');
            this.loading = false;
            this.modalRef.close();
          }
        },
        error: (error: any) => {
          console.error('Error loading user data:', error);
          this.msg.error('Failed to load user data');
          this.loading = false;
          this.modalRef.close();
        }
      });
    } else {
      console.error('No user ID provided for edit mode');
    }
  }

  onAvatarUpload(info: any): void {
    if (info.file.status === 'done') {
      // Store the file for FormData submission
      this.avatarFile = info.file.originFileObj;

      // Create a preview URL for the avatar
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.avatarUrl = e.target.result;
        // Update the form control with the file name
        this.userForm.avatarControl.setValue(info.file.name);
      };
      reader.readAsDataURL(info.file.originFileObj);

      this.msg.success(this.i18nSrv.fanyi('user.form.actions.upload_avatar'));
    } else if (info.file.status === 'error') {
      this.msg.error(this.i18nSrv.fanyi('user.form.error.upload_failed'));
    }
  }

  customUploadRequest = (options: any): Subscription => {
    // Handle file upload locally without sending to external server
    const { file, onSuccess, onError } = options;

    // Create a subscription that handles the upload with a small delay
    return of(file).pipe(
      delay(300) // Add a small delay to simulate upload process
    ).subscribe({
      next: (uploadedFile) => {
        if (uploadedFile) {
          // Store the file for later use in FormData
          this.avatarFile = uploadedFile;

          // Create preview URL
          const reader = new FileReader();
          reader.onload = (e: any) => {
            this.avatarUrl = e.target.result;
            this.userForm.avatarControl.setValue(uploadedFile.name);
          };
          reader.readAsDataURL(uploadedFile);

          // Mark as successful
          onSuccess({ url: this.avatarUrl }, uploadedFile);
          this.msg.success('Avatar uploaded successfully');
        } else {
          onError(new Error('Upload failed'));
          this.msg.error('Upload failed');
        }
      },
      error: (error) => {
        onError(error);
        this.msg.error('Upload failed');
      }
    });
  };

  beforeUpload = (file: any): boolean => {
    const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';
    if (!isJpgOrPng) {
      this.msg.error('You can only upload JPG/PNG files!');
      return false;
    }
    const isLt2M = file.size / 1024 / 1024 < 2;
    if (!isLt2M) {
      this.msg.error('Image must be smaller than 2MB!');
      return false;
    }
    return true;
  };

  save(): void {
    const selectedRole = this.userForm.roleControl.value;
    if (this.userForm.isFormValidForRole(selectedRole)) {
      this.loading = true;
   
      // Create FormData for file upload
      const formData = new FormData();

      // Add all form fields to FormData
      const formValue = this.userForm.value;
      Object.keys(formValue).forEach(key => {
        if (key === 'avatar' && this.avatarFile) {
          // Add the actual file for avatar
          formData.append('avatar', this.avatarFile, this.avatarFile.name);
        } else if (formValue[key] !== null && formValue[key] !== undefined) {
          // Add other form fields
          formData.append(key, formValue[key]);
        }
      });

      // Also add the avatar file separately if it exists
      if (this.avatarFile) {
        formData.append('avatar_file', this.avatarFile, this.avatarFile.name);
      }

      // Add user ID if in edit mode
      if (this.mode === 'edit' && this.userId) {
        formData.append('id', this.userId.toString());
      }

      // Send FormData to the service based on mode
      const operation = this.mode === 'edit'
        ? this.userService.updateUser(formData)
        : this.userService.createUser(formData);

      operation.subscribe({
        next: (response: User) => {
          this.loading = false;

          // Log the FormData contents for debugging
          console.log('FormData contents:');
          formData.forEach((value, key) => {
            console.log(key + ': ' + value);
          });

          const successMessage = this.mode === 'edit'
            ? this.i18nSrv.fanyi('user.form.success.updated')
            : this.i18nSrv.fanyi('user.form.success.saved');

          this.msg.success(successMessage);
          this.modalRef.close(response);
        },
        error: (error: any) => {
          this.loading = false;
          this.msg.error(this.i18nSrv.fanyi('user.form.error.saving'));
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  cancel(): void {
    this.modalRef.close(false);
  }

  private markFormGroupTouched(): void {
    Object.keys(this.userForm.controls).forEach(key => {
      const control = this.userForm.get(key);
      control?.markAsTouched();
    });
  }

  // These methods are now replaced by the standalone pipes
  // isFieldInvalid and getFieldError are now handled by FieldInvalidPipe and FieldErrorPipe
}
