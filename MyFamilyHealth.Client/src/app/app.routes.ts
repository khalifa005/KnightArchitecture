import { Routes } from '@angular/router';
import { provideTranslocoScope } from '@jsverse/transloco';

export const routes: Routes = [
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register.component').then(c => c.RegisterComponent),
    providers: [provideTranslocoScope('register')]
  },
  {
    path: 'welcome',
    loadComponent: () => import('./features/landing/landing.component').then(m => m.LandingComponent),
    providers: [provideTranslocoScope('landing')]
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(c => c.LoginComponent),
    providers: [provideTranslocoScope('login')]
  },
  {
    path: '',
    loadComponent: () => import('./core/layout/layout.component').then(c => c.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./features/test-page/test-page.component').then(c => c.TestPageComponent),
        providers: [provideTranslocoScope('dashboard')]
      },
      {
        path: 'medical-records',
        loadComponent: () => import('./features/medical-records/medical-records').then(c => c.MedicalRecords),
        providers: [provideTranslocoScope('medical-records')]
      },
      {
        path: 'medications',
        loadComponent: () => import('./features/medication-manager/medication-manager').then(c => c.MedicationManager),
        providers: [provideTranslocoScope('medication-manager')]
      },
      {
        path: 'ai-assistant',
        loadComponent: () => import('./features/ai-assistant/ai-assistant').then(c => c.AiAssistant),
        providers: [provideTranslocoScope('ai-assistant')]
      },
      {
        path: 'access-control',
        loadComponent: () => import('./features/access-control/access-control').then(c => c.AccessControl),
        providers: [provideTranslocoScope('access-control')]
      }
    ]
  }
];
