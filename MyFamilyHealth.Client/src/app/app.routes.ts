import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./core/layout/layout.component').then(c => c.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./features/test-page/test-page.component').then(c => c.TestPageComponent)
      },
      {
        path: 'medical-records',
        loadComponent: () => import('./features/medical-records/medical-records').then(c => c.MedicalRecords)
      },
      {
        path: 'medications',
        loadComponent: () => import('./features/medication-manager/medication-manager').then(c => c.MedicationManager)
      },
      {
        path: 'ai-assistant',
        loadComponent: () => import('./features/ai-assistant/ai-assistant').then(c => c.AiAssistant)
      },
      {
        path: 'access-control',
        loadComponent: () => import('./features/access-control/access-control').then(c => c.AccessControl)
      }
    ]
  }
];
