import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { TabScrollService } from './TabScrollService';

@Component({
  selector: 'app-dynamic-tabs-v2',
  templateUrl: './dynamic-tabs-v2.component.html',
  styleUrl: './dynamic-tabs-v2.component.scss'
})
export class DynamicTabsV2Component implements OnInit {
  @ViewChild('tabContainer') tabContainer!: ElementRef;

  tabs: TabItem[] = [
    { id: '1', label: 'Emergency', isActive: true },
    { id: '2', label: 'Closed', isActive: false },
    { id: '3', label: 'Closed from EAM', isActive: false },
    { id: '4', label: 'Tasks unregistered meter', isActive: false },
    { id: '5', label: 'Connection-registered', isActive: false },
    { id: '6', label: 'Connection-allocated', isActive: false }
  ];

  canScrollLeft$ = this.tabScrollService.canScrollLeft$;
  canScrollRight$ = this.tabScrollService.canScrollRight$;

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
}

export interface TabItem {
  id: string;
  label: string;
  isActive: boolean;
}


