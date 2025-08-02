import { ChangeDetectionStrategy, Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { _HttpClient } from '@delon/theme';
import { SHARED_IMPORTS } from '@shared';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalRef } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-add-role',
  imports: [...SHARED_IMPORTS, ReactiveFormsModule],
  templateUrl: './add-role.component.html',
  styleUrl: './add-role.component.less',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddRoleComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly http = inject(_HttpClient);
  private readonly msg = inject(NzMessageService);
  private readonly modalRef = inject(NzModalRef);

  roleForm!: FormGroup;
  loading = false;

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.roleForm = this.fb.group({
      name_en: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      name_ar: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      description_en: ['', [Validators.maxLength(500)]],
      description_ar: ['', [Validators.maxLength(500)]],
      is_active: [true],
      permissions: [[]]
    });
  }

  onSubmit(): void {
    if (this.roleForm.valid) {
      this.loading = true;
      const formData = this.roleForm.value;

      this.http.post('/api/roles', formData).subscribe({
        next: (res: any) => {
          this.msg.success('Role created successfully');
          this.modalRef.close(res);
        },
        error: (error) => {
          this.msg.error('Failed to create role');
          this.loading = false;
        }
      });
    } else {
      this.markFormGroupTouched();
    }
  }

  onCancel(): void {
    this.modalRef.close();
  }

  private markFormGroupTouched(): void {
    Object.keys(this.roleForm.controls).forEach(key => {
      const control = this.roleForm.get(key);
      control?.markAsTouched();
    });
  }

  // Helper method to check if a field is invalid
  isFieldInvalid(fieldName: string): boolean {
    const field = this.roleForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched));
  }

  // Helper method to get error message
  getErrorMessage(fieldName: string): string {
    const field = this.roleForm.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) {
        return `${fieldName.replace('_', ' ')} is required`;
      }
      if (field.errors['minlength']) {
        return `${fieldName.replace('_', ' ')} must be at least ${field.errors['minlength'].requiredLength} characters`;
      }
      if (field.errors['maxlength']) {
        return `${fieldName.replace('_', ' ')} must not exceed ${field.errors['maxlength'].requiredLength} characters`;
      }
    }
    return '';
  }
}
