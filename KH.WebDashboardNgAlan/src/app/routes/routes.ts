import { Routes } from '@angular/router';
import { startPageGuard } from '@core';
import { authJWTCanActivate, authJWTCanActivateChild, authSimpleCanActivate, authSimpleCanActivateChild } from '@delon/auth';

import { LayoutBasicComponent, LayoutBlankComponent } from '../layout';
import { aclCanActivate, ACLGuardType } from '@delon/acl';

export const routes: Routes = [
  {
    path: '',
    component: LayoutBasicComponent,
    // canActivate: [startPageGuard, authSimpleCanActivate],
    canActivate: [startPageGuard, authJWTCanActivate, aclCanActivate],
    // canActivateChild: [authSimpleCanActivateChild],
    canActivateChild: [authJWTCanActivateChild],
    data: {
      guard: {
        role: ['super-admin'],
        ability: [10, 'USER-EDIT'],
        mode: 'allOf'
      } as ACLGuardType,
      guard_url: '/no-permisseion'
    },
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadChildren: () => import('./dashboard/routes').then(m => m.routes)
      },
      {
        path: 'widgets',
        loadChildren: () => import('./widgets/routes').then(m => m.routes)
      },
      { path: 'style', loadChildren: () => import('./style/routes').then(m => m.routes) },
      { path: 'delon', loadChildren: () => import('./delon/routes').then(m => m.routes) },
      { path: 'extras', loadChildren: () => import('./extras/routes').then(m => m.routes) },
      { path: 'pro', loadChildren: () => import('./pro/routes').then(m => m.routes) }
    ]
  },
  // Blak Layout 空白布局
  {
    path: 'data-v',
    component: LayoutBlankComponent,
    children: [{ path: '', loadChildren: () => import('./data-v/routes').then(m => m.routes) }]
  },
  // passport
  { path: '', loadChildren: () => import('./passport/routes').then(m => m.routes) },
  { path: 'exception', loadChildren: () => import('./exception/routes').then(m => m.routes) },
  { path: '**', redirectTo: 'exception/404' }
];
