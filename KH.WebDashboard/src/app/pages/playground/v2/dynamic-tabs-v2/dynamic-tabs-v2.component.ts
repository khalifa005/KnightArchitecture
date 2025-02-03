import { DOCUMENT } from '@angular/common';
import { Component, ElementRef, Inject, OnInit, Renderer2, ViewChild } from '@angular/core';

@Component({
  selector: 'app-dynamic-tabs-v2',
  templateUrl: './dynamic-tabs-v2.component.html',
  styleUrls: ['./dynamic-tabs-v2.component.scss']
})
export class DynamicTabsV2Component implements OnInit {
  @ViewChild('tabContainer') tabContainer!: ElementRef;
  direction: string = 'ltr'; // Default

  tabs = [
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

  canScrollLeft = false;
  canScrollRight = false;
  fitToWidth = false;

  get activeTabContent(): string {
    return this.tabs.find(tab => tab.isActive)?.content || '';
  }

  constructor(private renderer: Renderer2, @Inject(DOCUMENT) private document: Document) {}

  ngOnInit(): void {
    this.direction = this.document.documentElement.dir || 'ltr';
    setTimeout(() => this.checkScroll(), 0);
  }

  onScroll(): void {
    this.checkScroll();
  }

  checkScroll(): void {
    const container = this.tabContainer.nativeElement;
    const isRtl = this.direction === 'rtl'; // Detect RTL mode
    const maxScrollLeft = container.scrollWidth - container.clientWidth;

    if (isRtl) {
      this.canScrollRight = container.scrollLeft < 0;
      this.canScrollLeft = container.scrollLeft > -maxScrollLeft;
    } else {
      this.canScrollLeft = container.scrollLeft > 0;
      this.canScrollRight = container.scrollLeft < maxScrollLeft;
    }
  }

  scrollLeft(): void {
    const container = this.tabContainer.nativeElement;
    const scrollAmount = this.direction === 'rtl' ? 200 : -200;
    container.scrollBy({ left: scrollAmount, behavior: 'smooth' });
  }

  scrollRight(): void {
    const container = this.tabContainer.nativeElement;
    const scrollAmount = this.direction === 'rtl' ? -200 : 200;
    container.scrollBy({ left: scrollAmount, behavior: 'smooth' });
  }

  selectTab(id: string): void {
    this.tabs = this.tabs.map(tab => ({
      ...tab,
      isActive: tab.id === id
    }));
  }

  toggleFitToWidth(value: boolean): void {
    this.fitToWidth = value;
  }

  updateTabCount(tabId: string, count: number): void {
    this.tabs = this.tabs.map(tab =>
      tab.id === tabId ? { ...tab, count } : tab
    );
  }
}
