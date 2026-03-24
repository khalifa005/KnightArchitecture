import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslocoModule } from '@jsverse/transloco';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { BadgeModule } from 'primeng/badge';

@Component({
  selector: 'app-doctor-dashboard',
  imports: [CommonModule, TranslocoModule, ButtonModule, TableModule, TagModule, BadgeModule],
  templateUrl: './doctor-dashboard.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: { class: 'block w-full' }
})
export class DoctorDashboardComponent {
  appointments = [
    { time: '09:30', period: 'AM', name: 'Elena Vance', detail: 'Post-op Consultation • Room 4B', icon: 'person', iconBg: 'bg-primary-fixed', iconColor: 'text-primary', highlighted: false },
    { time: '10:15', period: 'AM', name: 'Julian Thorne', detail: 'Telemedicine • Recurring Follow-up', icon: 'videocam', iconBg: 'bg-secondary-fixed', iconColor: 'text-secondary', highlighted: true },
    { time: '11:00', period: 'AM', name: 'Sarah Mitchell', detail: 'Initial Assessment • New Patient', icon: 'person', iconBg: 'bg-primary-fixed', iconColor: 'text-primary', highlighted: false },
  ];

  accessRequests = [
    { name: 'Mila K. Vance', requestKey: 'doctor-dashboard.requests.full_history', image: 'https://lh3.googleusercontent.com/aida-public/AB6AXuARlp34kZjrWQRfNGhH-yp2_4Cd3di2SjBfTdkzkHMZoSPFFV87BaHjSd3JerwEqYiB-iXhE6nNQ3EYrAdA25i2kHg9Yu3Nco1oEJ-cvNDlZkDKDtordprf7oO4CkBSG9gZy5I5e38Yo-byykl-VTlHtjzILxVH3jtwzYNf_3VQpPPrfzAIkzDi_GLjIQ6OmVg0AeB24PCwKM0XhJdvxgk5EO3TAO0BXB8u0zOKY3Cohkc6aaa0Cc4dF5AWp9P7zPfRZAqGzLxpSpcZ' },
    { name: 'Robert Chen', requestKey: 'doctor-dashboard.requests.lab_only', image: 'https://lh3.googleusercontent.com/aida-public/AB6AXuD4VKITHjBjSWAqyD3E2yC48q6XO6YPq9ErqXANtHl-QE0kEObTR_KdQvi1Tie4zND-WwoXhnkegPsZ3utPTQQuaW324W50YCJZ2OApHTlrBgXS4gRPlssHXE8-nX55KfgxtlEomopa-Sfl96yqUl_P-32OFsrVJLAWUWg72C33-3dm1DKdc8793sr--HJ6cY4xp15BPZDycjE8eMr8l96cU-prSWhgI1qgndLbLHYw1GyXKHViBq8LwMR9bNzzTAzmnQjSrum8Wgwa' },
  ];

  labResults = [
    { patient: 'David Miller', id: '882-192-00', test: 'Complete Blood Count (CBC)', statusKey: 'doctor-dashboard.lab_status.normal', statusSeverity: 'success', lab: "Saint Mary's Clinical", anomalous: false },
    { patient: 'Linda Garcia', id: '441-002-31', test: 'Thyroid Panel (TSH/T4)', statusKey: 'doctor-dashboard.lab_status.anomalous', statusSeverity: 'danger', lab: 'BioCore Labs', anomalous: true },
    { patient: 'Marcus Wu', id: '991-003-88', test: 'Lipid Profile', statusKey: 'doctor-dashboard.lab_status.normal', statusSeverity: 'success', lab: 'City Diagnostics', anomalous: false },
  ];
}
