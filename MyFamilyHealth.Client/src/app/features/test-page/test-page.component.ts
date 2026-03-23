import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-test-page',
  standalone: true,
  imports: [CommonModule, ButtonModule],
  templateUrl: './test-page.component.html',
  styleUrl: './test-page.component.scss'
})
export class TestPageComponent {
  title = 'MyFamilyHealth Dashboard';
  
  kpiData = [
    { title: 'Orders', value: '152', icon: 'pi pi-shopping-cart', iconColor: '#3b82f6', iconBg: 'rgba(59, 130, 246, 0.12)' },
    { title: 'Revenue', value: '$3.200', icon: 'pi pi-map-marker', iconColor: '#f59e0b', iconBg: 'rgba(245, 158, 11, 0.12)' },
    { title: 'Customers', value: '28441', icon: 'pi pi-inbox', iconColor: '#10b981', iconBg: 'rgba(16, 185, 129, 0.12)' },
    { title: 'Comments', value: '152 Unread', icon: 'pi pi-comment', iconColor: '#6366f1', iconBg: 'rgba(99, 102, 241, 0.12)' }
  ];
}
