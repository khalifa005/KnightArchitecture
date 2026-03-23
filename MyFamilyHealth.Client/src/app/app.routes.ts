import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./features/test-page/test-page.component').then(c => c.TestPageComponent)
  }
];
