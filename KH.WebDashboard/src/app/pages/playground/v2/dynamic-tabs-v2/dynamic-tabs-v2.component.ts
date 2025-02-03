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
    { id: '1', label: 'Emergency', isActive: true, content: '<h2>Emergency Content</h2>', count: 5, icon: 'alert-triangle-outline' },
    { id: '2', label: 'Closed', isActive: false, content: '<h2>Closed Content</h2>', count: 2, icon: 'folder-outline' },
    { id: '3', label: 'Closed from EAM', isActive: false, content: '<h2>EAM Closures</h2>', count: 7, icon: 'lock-outline' },
    { id: '4', label: 'Tasks unregistered meter', isActive: false, content: '<h2>Unregistered Meter Tasks</h2>', count: 3, icon: 'settings-2-outline' },
    { id: '5', label: 'Connection-registered', isActive: false, content: '<h2>Registered Connections</h2>', icon: 'power-outline' },
    { id: '6', label: 'Connection-allocated', isActive: false, content: '<h2>Allocated Connections</h2>', icon: 'link-outline' },
    { id: '7', label: 'Maintenance', isActive: false, content: '<h2>Maintenance</h2>', count: 1, icon: 'tool-outline' },
    { id: '8', label: 'Inspection', isActive: false, content: '<h2>Inspection</h2>', icon: 'search-outline' },
    { id: '9', label: 'Reports', isActive: false, content: '<h2>Reports</h2>', count: 10, icon: 'file-text-outline' },
    { id: '10', label: 'Settings', isActive: false, content: '<h2>Settings</h2>', icon: 'settings-outline' }
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

     updateTabCount(tabId: string, count: number): void {
      this.tabs = this.tabs.map(tab => 
        tab.id === tabId ? { ...tab, count } : tab
      );
    }
    
   }