import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@jsverse/transloco';
import { TableModule } from 'primeng/table';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { ListboxModule } from 'primeng/listbox';
import { TooltipModule } from 'primeng/tooltip';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-permission-management',
  standalone: true,
  imports: [
    CommonModule, 
    TranslocoModule, 
    TableModule, 
    CheckboxModule, 
    ButtonModule, 
    CardModule, 
    ListboxModule, 
    TooltipModule,
    FormsModule
  ],
  template: `
    <div class="permission-management-container p-4 lg:p-6" *transloco="let t; read: 'permission-management'">
      <div class="mb-6">
        <h1 class="text-3xl font-bold text-900 m-0">{{ t('title') }}</h1>
        <p class="text-600 m-0 mt-1">{{ t('subtitle') }}</p>
      </div>

      <div class="grid">
        <!-- Roles Sidebar -->
        <div class="col-12 lg:col-3 p-2">
          <p-card [header]="t('roles_title')" class="shadow-2 h-full">
            <p-listbox 
              [options]="roles" 
              [(ngModel)]="selectedRole" 
              optionLabel="name" 
              styleClass="w-full border-none"
              [listStyle]="{'max-height': 'calc(100vh - 400px)'}">
              <ng-template pTemplate="item" let-role>
                <div class="flex align-items-center gap-2 p-2 w-full">
                  <i class="pi" [ngClass]="role.icon"></i>
                  <span>{{ role.name }}</span>
                </div>
              </ng-template>
            </p-listbox>
          </p-card>
        </div>

        <!-- Permission Matrix -->
        <div class="col-12 lg:col-9 p-2">
          <p-card class="shadow-2 h-full" [header]="selectedRole().name + ' Permissions'">
            <p-table [value]="modules()" styleClass="p-datatable-sm p-datatable-gridlines">
              <ng-template pTemplate="header">
                <tr>
                  <th class="bg-primary-50">{{ t('matrix.module') }}</th>
                  <th class="text-center bg-primary-50" style="width: 6rem">{{ t('matrix.view') }}</th>
                  <th class="text-center bg-primary-50" style="width: 6rem">{{ t('matrix.create') }}</th>
                  <th class="text-center bg-primary-50" style="width: 6rem">{{ t('matrix.edit') }}</th>
                  <th class="text-center bg-primary-50" style="width: 6rem">{{ t('matrix.delete') }}</th>
                </tr>
              </ng-template>
              <ng-template pTemplate="body" let-module>
                <tr>
                  <td class="font-bold text-900">
                    <div class="flex align-items-center gap-2">
                      <i class="pi" [ngClass]="module.icon"></i>
                      {{ t('modules.' + module.key) }}
                    </div>
                  </td>
                  <td class="text-center">
                    <p-checkbox [(ngModel)]="module.permissions.view" [binary]="true"></p-checkbox>
                  </td>
                  <td class="text-center">
                    <p-checkbox [(ngModel)]="module.permissions.create" [binary]="true"></p-checkbox>
                  </td>
                  <td class="text-center">
                    <p-checkbox [(ngModel)]="module.permissions.edit" [binary]="true"></p-checkbox>
                  </td>
                  <td class="text-center">
                    <p-checkbox [(ngModel)]="module.permissions.delete" [binary]="true"></p-checkbox>
                  </td>
                </tr>
              </ng-template>
            </p-table>

            <div class="flex justify-content-end gap-3 mt-6">
              <p-button [label]="t('reset_btn')" [text]="true" severity="secondary"></p-button>
              <p-button [label]="t('save_btn')" icon="pi pi-check" styleClass="px-6 border-round-xl"></p-button>
            </div>
          </p-card>
        </div>
      </div>
    </div>
  `,
  styles: [`
    :host ::ng-deep .p-listbox .p-listbox-list .p-listbox-item.p-highlight {
      background: var(--primary-100);
      color: var(--primary-700);
      font-weight: 600;
    }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PermissionManagementComponent {
  roles = [
    { name: 'Administrator', icon: 'pi-shield', key: 'admin' },
    { name: 'Physician', icon: 'pi-user-edit', key: 'physician' },
    { name: 'Nurse', icon: 'pi-id-card', key: 'nurse' },
    { name: 'Patient', icon: 'pi-user', key: 'patient' },
    { name: 'Auditor', icon: 'pi-eye', key: 'auditor' }
  ];

  selectedRole = signal(this.roles[0]);

  modules = signal([
    { key: 'medical_records', icon: 'pi-folder', permissions: { view: true, create: false, edit: false, delete: false } },
    { key: 'medications', icon: 'pi-tablet', permissions: { view: true, create: true, edit: true, delete: false } },
    { key: 'upload_center', icon: 'pi-upload', permissions: { view: true, create: true, edit: false, delete: false } },
    { key: 'billing', icon: 'pi-wallet', permissions: { view: true, create: false, edit: false, delete: false } },
    { key: 'user_mgmt', icon: 'pi-users', permissions: { view: true, create: true, edit: true, delete: true } },
    { key: 'reports', icon: 'pi-chart-bar', permissions: { view: true, create: true, edit: false, delete: false } }
  ]);
}

