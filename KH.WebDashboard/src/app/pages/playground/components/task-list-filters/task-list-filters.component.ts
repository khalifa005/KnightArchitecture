import { Component } from '@angular/core';
export interface FilterTab {
  id: string;
  label: string;
  active: boolean;
}

export interface FilterState {
  selectedTab: string;
  searchQuery: string;
  exportEnabled: boolean;
  multiAllocationEnabled: boolean;
}
@Component({
  selector: 'task-list-filters',
  templateUrl: './task-list-filters.component.html',
  styleUrl: './task-list-filters.component.scss'
})
export class TaskListFiltersComponent {
  filterTabs: FilterTab[] = [
    { id: 'emergency', label: 'Emergency', active: true },
    { id: 'closed', label: 'Closed', active: false },
    { id: 'closed-eam', label: 'Closed from EAM', active: false },
    { id: 'unregistered', label: 'Tasks unregistered meter', active: false },
    { id: 'connection-registered', label: 'Connection-registered', active: false },
    { id: 'connection-allocated', label: 'Connection-allocated', active: false }
  ];

  selectTab(tabId: string): void {
    this.filterTabs = this.filterTabs.map(tab => ({
      ...tab,
      active: tab.id === tabId
    }));
  }
}
