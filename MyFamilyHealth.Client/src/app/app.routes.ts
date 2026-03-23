import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./core/layout/layout.component').then(c => c.LayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () => import('./features/test-page/test-page.component').then(c => c.TestPageComponent)
      }
    ]
  }
];
