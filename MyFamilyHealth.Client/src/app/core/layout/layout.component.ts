import { Component, inject, signal, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AppStore } from '../state/app.store';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MenuModule } from 'primeng/menu';
import { BadgeModule } from 'primeng/badge';
import { Popover, PopoverModule } from 'primeng/popover';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, TranslocoModule, ButtonModule, InputTextModule, MenuModule, PopoverModule, BadgeModule],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  store = inject(AppStore);
  isExpanded = signal(true);

  @ViewChild('notifPanel') notifPanel!: Popover;

  notifications = [
    {
      icon: 'lab_research',
      iconColor: 'text-primary',
      iconBg: 'bg-primary-fixed',
      title: 'Lab Results Ready',
      desc: 'Your CBC panel results are now available.',
      time: '2 min ago',
      unread: true
    },
    {
      icon: 'medication',
      iconColor: 'text-secondary',
      iconBg: 'bg-secondary-fixed',
      title: 'Medication Reminder',
      desc: 'Time to take your evening Metformin dose.',
      time: '30 min ago',
      unread: true
    },
    {
      icon: 'calendar_month',
      iconColor: 'text-tertiary',
      iconBg: 'bg-tertiary-fixed',
      title: 'Appointment Tomorrow',
      desc: 'Dr. Ahmed Al-Rashid — 9:30 AM Cardiology.',
      time: '1 hr ago',
      unread: false
    },
    {
      icon: 'verified_user',
      iconColor: 'text-on-surface-variant',
      iconBg: 'bg-surface-container',
      title: 'Access Granted',
      desc: 'Dr. Sarah Mitchell now has view access.',
      time: '3 hr ago',
      unread: false
    }
  ];

  get unreadCount() {
    return this.notifications.filter(n => n.unread).length;
  }

  toggleNotifications(event: Event) {
    this.notifPanel.toggle(event);
  }

  markAllRead() {
    this.notifications.forEach(n => n.unread = false);
  }

  menuItems: MenuItem[] = [
    { label: 'nav.dashboard', icon: 'dashboard', routerLink: '/' },
    { label: 'nav.family_profiles', icon: 'family_restroom', routerLink: '/family-profiles' },
    { label: 'nav.medical_records', icon: 'folder_shared', routerLink: '/medical-records' },
    { label: 'nav.medications', icon: 'medication', routerLink: '/medications' },
    { label: 'nav.about_us', icon: 'info', routerLink: '/about-us' },
    { label: 'nav.ai_assistant', icon: 'smart_toy', routerLink: '/ai-assistant' },
    { label: 'nav.access_control', icon: 'lock_person', routerLink: '/access-control' },
    { label: 'nav.doctor_dashboard', icon: 'dashboard', routerLink: '/doctor-dashboard' },
    { label: 'nav.clinical_report', icon: 'analytics', routerLink: '/clinical-report' },
    { label: 'nav.help_center', icon: 'help_outline', routerLink: '/help-center' },
    { label: 'nav.contact_us', icon: 'mail', routerLink: '/contact-us' },
    { separator: true },
    { label: 'nav.admin_users', icon: 'group', routerLink: '/admin/user-management' },
    { label: 'nav.admin_permissions', icon: 'shield_person', routerLink: '/admin/permission-management' },
    { label: 'nav.admin_analytics', icon: 'monitoring', routerLink: '/admin/analytics' },
    { separator: true },
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

  toggleDarkMode() {
    this.store.toggleDarkMode();
  }
}
