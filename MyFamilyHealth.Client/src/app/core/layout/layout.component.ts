import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AppStore } from '../state/app.store';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MenuModule } from 'primeng/menu';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslocoModule, ButtonModule, InputTextModule, MenuModule],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  store = inject(AppStore);
  isExpanded = signal(true);

  menuItems: MenuItem[] = [
    { label: 'nav.dashboard', icon: 'dashboard', routerLink: '/' },
    { label: 'nav.family_profiles', icon: 'family_restroom', routerLink: '/family-profiles' },
    { label: 'nav.medical_records', icon: 'folder_shared', routerLink: '/medical-records' },
    { label: 'nav.medications', icon: 'medication', routerLink: '/medications' },
    { label: 'nav.about_us', icon: 'info', routerLink: '/about-us' },
    { label: 'nav.ai_assistant', icon: 'smart_toy', routerLink: '/ai-assistant' },
    { label: 'nav.access_control', icon: 'lock_person', routerLink: '/access-control' },
    { label: 'nav.doctor_dashboard', icon: 'dashboard', routerLink: '/doctor-dashboard' },
    { label: 'nav.medical_records_review', icon: 'description', routerLink: '/medical-records-review' },
    { label: 'nav.upload_imaging', icon: 'radiology', routerLink: '/upload-imaging' },
    { label: 'nav.upload_center', icon: 'cloud_upload', routerLink: '/upload-center' },
    { label: 'nav.add_prescription', icon: 'prescriptions', routerLink: '/add-prescription' },
    { label: 'nav.upload_lab_result', icon: 'biotech', routerLink: '/upload-lab-result' }
  ];

  toggleLanguage() {
    const newLang = this.store.lang() === 'en-US' ? 'ar-SA' : 'en-US';
    this.store.setLanguage(newLang);
  }

  toggleSidebar() {
    this.isExpanded.update(val => !val);
  }
}
