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
        path: 'family-profiles',
        // Force esbuild re-resolution
        loadComponent: () => import('./features/family-profiles/family-profiles.component').then(c => c.FamilyProfilesComponent),
        providers: [provideTranslocoScope('family-profiles')]
      },
      {
        path: 'about-us',
        loadComponent: () => import('./features/about-us/about-us.component').then(c => c.AboutUsComponent),
        providers: [provideTranslocoScope('about-us')]
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
      },
      {
        path: 'doctor-dashboard',
        loadComponent: () => import('./features/doctor-dashboard/doctor-dashboard.component').then(c => c.DoctorDashboardComponent),
        providers: [provideTranslocoScope('doctor-dashboard')]
      },
      {
        path: 'medical-records-review',
        loadComponent: () => import('./features/medical-records-review/medical-records-review.component').then(c => c.MedicalRecordsReviewComponent),
        providers: [provideTranslocoScope('medical-records-review')]
      },
      {
        path: 'upload-imaging',
        loadComponent: () => import('./features/upload-imaging/upload-imaging.component').then(c => c.UploadImagingComponent),
        providers: [provideTranslocoScope('upload-imaging')]
      },
      {
        path: 'upload-center',
        loadComponent: () => import('./features/upload-center/upload-center.component').then(c => c.UploadCenterComponent),
        providers: [provideTranslocoScope('upload-center')]
      },
      {
        path: 'add-prescription',
        loadComponent: () => import('./features/add-prescription/add-prescription.component').then(c => c.AddPrescriptionComponent),
        providers: [provideTranslocoScope('add-prescription')]
      },
      {
        path: 'upload-lab-result',
        loadComponent: () => import('./features/upload-lab-result/upload-lab-result.component').then(c => c.UploadLabResultComponent),
        providers: [provideTranslocoScope('upload-lab-result')]
      },
      {
        path: 'help-center',
        loadComponent: () => import('./features/help-center/help-center.component').then(c => c.HelpCenterComponent),
        providers: [provideTranslocoScope('help-center')]
      },
      {
        path: 'contact-us',
        loadComponent: () => import('./features/contact-us/contact-us.component').then(c => c.ContactUsComponent),
        providers: [provideTranslocoScope('contact-us')]
      },
      {
        path: 'admin/user-management',
        loadComponent: () => import('./features/admin/user-management/user-management.component').then(c => c.UserManagementComponent),
        providers: [provideTranslocoScope('user-management')]
      },
      {
        path: 'admin/permission-management',
        loadComponent: () => import('./features/admin/permission-management/permission-management.component').then(c => c.PermissionManagementComponent),
        providers: [provideTranslocoScope('permission-management')]
      },
      {
        path: 'admin/analytics',
        loadComponent: () => import('./features/admin/analytics/analytics.component').then(c => c.AdminAnalyticsComponent),
        providers: [provideTranslocoScope('admin-analytics')]
      },
      {
        path: 'clinical-report',
        loadComponent: () => import('./features/clinical-report/clinical-report.component').then(c => c.ClinicalReportComponent),
        providers: [provideTranslocoScope('clinical-report')]
      }
    ]
  }
];
