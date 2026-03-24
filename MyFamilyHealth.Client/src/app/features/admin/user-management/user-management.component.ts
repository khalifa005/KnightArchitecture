import { Component, ChangeDetectionStrategy, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@jsverse/transloco';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { SelectModule } from 'primeng/select';
import { AvatarModule } from 'primeng/avatar';
import { CardModule } from 'primeng/card';
import { TooltipModule } from 'primeng/tooltip';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [
    CommonModule, 
    TranslocoModule, 
    TableModule, 
    TagModule, 
    ButtonModule, 
    InputTextModule, 
    IconFieldModule, 
    InputIconModule, 
    SelectModule,
    AvatarModule,
    CardModule,
    TooltipModule
  ],
  template: `
    <div class="user-management-container p-4 lg:p-6" *transloco="let t; read: 'user-management'">
      <div class="flex flex-column md:flex-row md:align-items-center justify-content-between mb-6 gap-4">
        <div>
          <h1 class="text-3xl font-bold text-900 m-0">{{ t('title') }}</h1>
          <p class="text-600 m-0 mt-1">{{ t('subtitle') }}</p>
        </div>
        <p-button label="Add New User" icon="pi pi-plus" rounded="true" size="small"></p-button>
      </div>

      <!-- Stats Grid -->
      <div class="grid mb-6">
        <div class="col-12 md:col-4 p-2">
          <div class="stats-card p-4 border-round-xl border-1 border-200 bg-white shadow-2">
            <div class="flex align-items-center justify-content-between mb-3">
              <span class="text-600 font-medium">{{ t('stats.total_users') }}</span>
              <div class="p-2 border-round-lg bg-primary-100 text-primary">
                <i class="pi pi-users text-xl"></i>
              </div>
            </div>
            <div class="text-3xl font-bold text-900">1,284</div>
            <div class="text-green-500 font-medium mt-2 text-sm">
              <i class="pi pi-arrow-up"></i> +12% from last month
            </div>
          </div>
        </div>
        <div class="col-12 md:col-4 p-2">
          <div class="stats-card p-4 border-round-xl border-1 border-200 bg-white shadow-2">
            <div class="flex align-items-center justify-content-between mb-3">
              <span class="text-600 font-medium">{{ t('stats.active_now') }}</span>
              <div class="p-2 border-round-lg bg-green-100 text-green-600">
                <i class="pi pi-bolt text-xl"></i>
              </div>
            </div>
            <div class="text-3xl font-bold text-900">56</div>
            <div class="text-green-500 font-medium mt-2 text-sm">
              <span class="inline-block w-2 h-2 border-round-circle bg-green-500 mr-2"></span>
              Live sessions
            </div>
          </div>
        </div>
        <div class="col-12 md:col-4 p-2">
          <div class="stats-card p-4 border-round-xl border-1 border-200 bg-white shadow-2">
            <div class="flex align-items-center justify-content-between mb-3">
              <span class="text-600 font-medium">{{ t('stats.pending') }}</span>
              <div class="p-2 border-round-lg bg-orange-100 text-orange-600">
                <i class="pi pi-clock text-xl"></i>
              </div>
            </div>
            <div class="text-3xl font-bold text-900">12</div>
            <div class="text-orange-500 font-medium mt-2 text-sm italic">
              Verification required
            </div>
          </div>
        </div>
      </div>

      <!-- Filters & Search -->
      <div class="flex flex-column md:flex-row align-items-center justify-content-between gap-3 mb-4 bg-white p-3 border-round-xl border-1 border-200 shadow-1">
        <div class="flex align-items-center gap-3 w-full md:w-auto">
          <p-iconField iconPosition="left" class="w-full md:w-20rem">
            <p-inputIcon class="pi pi-search"></p-inputIcon>
            <input type="text" pInputText [placeholder]="t('search')" class="w-full" />
          </p-iconField>
        </div>
        <div class="flex align-items-center gap-2 w-full md:w-auto">
          <p-select [options]="[]" [placeholder]="t('filters.role')" styleClass="w-full md:w-12rem"></p-select>
          <p-select [options]="[]" [placeholder]="t('filters.status')" styleClass="w-full md:w-10rem"></p-select>
        </div>
      </div>

      <!-- Main Table -->
      <div class="card shadow-2 border-round-xl overflow-hidden bg-white">
        <p-table 
          [value]="users" 
          [rows]="10" 
          [paginator]="true" 
          responsiveLayout="scroll"
          styleClass="p-datatable-gridlines p-datatable-striped">
          <ng-template pTemplate="header">
            <tr>
              <th>{{ t('columns.user') }}</th>
              <th>{{ t('columns.role') }}</th>
              <th>{{ t('columns.status') }}</th>
              <th>{{ t('columns.last_active') }}</th>
              <th style="width: 8rem">{{ t('columns.actions') }}</th>
            </tr>
          </ng-template>
          <ng-template pTemplate="body" let-user>
            <tr>
              <td>
                <div class="flex align-items-center gap-3">
                  <p-avatar [label]="user.initials" shape="circle" size="large" [style]="{'background-color': user.color, 'color': '#ffffff'}"></p-avatar>
                  <div class="flex flex-column">
                    <span class="font-bold text-900">{{ user.name }}</span>
                    <span class="text-sm text-600">{{ user.email }}</span>
                  </div>
                </div>
              </td>
              <td>
                <div class="flex align-items-center gap-2">
                   <i class="pi" [ngClass]="user.roleIcon"></i>
                   {{ t('roles.' + user.role) }}
                </div>
              </td>
              <td>
                <p-tag [value]="t('status.' + user.status)" [severity]="user.statusSeverity"></p-tag>
              </td>
              <td class="text-600">{{ user.lastActive }}</td>
              <td>
                <div class="flex gap-2">
                  <p-button icon="pi pi-pencil" [text]="true" [rounded]="true" size="small" [pTooltip]="t('actions.edit')"></p-button>
                  <p-button 
                    [icon]="user.status === 'suspended' ? 'pi pi-refresh' : 'pi pi-ban'" 
                    [text]="true" 
                    [rounded]="true" 
                    size="small" 
                    [severity]="user.status === 'suspended' ? 'success' : 'danger'"
                    [pTooltip]="user.status === 'suspended' ? t('actions.activate') : t('actions.suspend')">
                  </p-button>
                </div>
              </td>
            </tr>
          </ng-template>
        </p-table>
      </div>
    </div>
  `,
  styles: [`
    :host ::ng-deep .p-datatable .p-datatable-thead > tr > th {
      background: var(--primary-50);
      color: var(--primary-900);
      font-weight: 700;
    }
    .stats-card {
      transition: transform 0.2s;
    }
    .stats-card:hover {
      transform: translateY(-2px);
    }
  `],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserManagementComponent {
  users = [
    { name: 'Dr. Sarah Johnson', email: 'sarah.j@dhm-health.ai', initials: 'SJ', color: '#6366f1', role: 'physician', roleIcon: 'pi-user-edit', status: 'active', statusSeverity: 'success' as const, lastActive: '2 mins ago' },
    { name: 'Alex Miller', email: 'alex.m@gmail.com', initials: 'AM', color: '#10b981', role: 'patient', roleIcon: 'pi-user', status: 'active', statusSeverity: 'success' as const, lastActive: '10 mins ago' },
    { name: 'Nurse Maria Garcia', email: 'maria.g@dhm-health.ai', initials: 'MG', color: '#f59e0b', role: 'nurse', roleIcon: 'pi-id-card', status: 'pending', statusSeverity: 'warn' as const, lastActive: '5 hours ago' },
    { name: 'Mark Davis', email: 'mark.d@outlook.com', initials: 'MD', color: '#64748b', role: 'patient', roleIcon: 'pi-user', status: 'suspended', statusSeverity: 'danger' as const, lastActive: '2 days ago' },
    { name: 'Admin Khalifa', email: 'admin@dhm.ai', initials: 'AK', color: '#ec4899', role: 'admin', roleIcon: 'pi-shield', status: 'active', statusSeverity: 'success' as const, lastActive: 'Now' }
  ];
}

