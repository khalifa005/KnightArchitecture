import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { TabScrollService } from './TabScrollService';
import { Tab } from './Tab';

@Component({
  selector: 'app-dynamic-tabs-v2',
  templateUrl: './dynamic-tabs-v2.component.html',
  styleUrl: './dynamic-tabs-v2.component.scss'
})
export class DynamicTabsV2Component implements OnInit {
  @ViewChild('tabContainer') tabContainer!: ElementRef;

  tabs: Tab[] = [
    { id: '1', label: 'Emergency', isActive: true, content: '<h2>Emergency Content</h2><p>Emergency related information and actions.</p>' },
    { id: '2', label: 'Closed', isActive: false, content: '<h2>Closed Content</h2><p>List of closed items and their details.</p>' },
    { id: '3', label: 'Closed from EAM', isActive: false, content: '<h2>EAM Closures</h2><p>Items closed from the EAM system.</p>' },
    { id: '4', label: 'Tasks unregistered meter', isActive: false, content: '<h2>Unregistered Meter Tasks</h2><p>Tasks related to unregistered meters.</p>' },
    { id: '5', label: 'Connection-registered', isActive: false, content: '<h2>Registered Connections</h2><p>List of registered connections.</p>' },
    { id: '6', label: 'Connection-allocated', isActive: false, content: '<h2>Allocated Connections</h2><p>Details of allocated connections.</p>' },
    { id: '7', label: 'Maintenance', isActive: false, content: '<h2>Maintenance</h2><p>Scheduled maintenance and related tasks.</p>' },
    { id: '8', label: 'Inspection', isActive: false, content: '<h2>Inspection</h2><p>Inspection reports and schedules.</p>' },
    { id: '9', label: 'Reports', isActive: false, content: '<h2>Reports</h2><p>Generated reports and analytics.</p>' },
    { id: '10', label: 'Settings', isActive: false, content: '<h2>Settings</h2><p>System configuration and preferences.</p>' }
  ];

  canScrollLeft$ = this.tabScrollService.canScrollLeft$;
  canScrollRight$ = this.tabScrollService.canScrollRight$;
  fitToWidth$ = this.tabScrollService.fitToWidth$;

  get activeTabContent(): string {
    return this.tabs.find(tab => tab.isActive)?.content || '';
  }

  constructor(private tabScrollService: TabScrollService) {}

  ngOnInit(): void {
    setTimeout(() => this.checkScroll(), 0);
  }

  onScroll(): void {
    this.checkScroll();
  }

  checkScroll(): void {
    this.tabScrollService.updateScrollButtons(this.tabContainer.nativeElement);
  }

  scrollLeft(): void {
    this.tabScrollService.scrollLeft(this.tabContainer.nativeElement);
  }
  scrollRight(): void {
   this.tabScrollService.scrollRight(this.tabContainer.nativeElement);
     }
   
   
   
     selectTab(id: string): void {
    this.tabs = this.tabs.map(tab => ({
     ...tab,
     isActive: tab.id === id
     }));
     }
   
   
   
     toggleFitToWidth(value: boolean): void {
     this.tabScrollService.toggleFitToWidth(value);
     }
   }