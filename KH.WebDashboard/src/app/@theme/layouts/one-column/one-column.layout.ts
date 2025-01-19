import { Component, OnDestroy, OnInit } from '@angular/core';
import { LayoutService } from '@app/@core/utils.ts';
import { NbMediaBreakpointsService, NbMenuService, NbSidebarService, NbThemeService } from '@nebular/theme';
import { Subject, Subscription, takeUntil } from 'rxjs';
@Component({
  selector: 'ngx-one-column-layout',
  styleUrls: ['./one-column.layout.scss'],
  template: `
    <nb-layout>
      <nb-layout-header 
      [ngClass]="isExpandedHeader ? headerClassExpanded : headerClassCompacted "
      class="custom-header-expanded"  fixed>
        <ngx-header></ngx-header>
      </nb-layout-header>

      <nb-sidebar class="menu-sidebar" tag="menu-sidebar" responsive start>
      <div class="logo-section">

      <img *ngIf="isExpandedHeader" src="https://cdn.builder.io/api/v1/image/assets/TEMP/a86eb78ce22023a335f5b608f9666955c1e4a0fcd44d2982b3eaa8921839e023?placeholderIfAbsent=true&apiKey=a0d33f5c0ebf4aa6a5461f186b30b21f" alt="Company logo" class="company-logo" />
      <img *ngIf="!isExpandedHeader" src="assets/icons/nwc-logo-col.svg" alt="Company logo" class="company-logo" />
    
      <img src="assets/icons/col.svg" alt="Menu toggle" class="menu-toggle" *ngIf="isExpandedHeader" 
      (click)="toggleSidebar()"/>
    

    </div>
    <div class="nav-divider"></div>

      <!-- <img src="https://cdn.builder.io/api/v1/image/assets/TEMP/660a5cc04a46775515f28b0da5f1a820598f6b2217cfad89aac7364ed3477e5c?placeholderIfAbsent=true&apiKey=a0d33f5c0ebf4aa6a5461f186b30b21f" alt="Menu toggle" class="menu-toggle" /> -->
        <ng-content select="nb-menu"></ng-content>
      </nb-sidebar>

      <nb-layout-column>
        <ng-content select="router-outlet"></ng-content>
      </nb-layout-column>

    </nb-layout>
  `,
})
export class OneColumnLayoutComponent implements OnInit, OnDestroy {
  $destroy = new Subject<void>()
  private sidebarToggleSubscription!: Subscription;
  private sidebarExpandSubscription!: Subscription;
  private sidebarCollapseSubscription!: Subscription;

  headerClassCompacted = "custom-header-compacted";
  headerClassExpanded = "custom-header-expanded";
  isExpandedHeader = true;

  constructor(
    private menuService: NbMenuService,
    private sidebarService: NbSidebarService,
        private layoutService: LayoutService,
    
    private breakpointService: NbMediaBreakpointsService) {

  }
  ngOnInit(): void {
    // Listen to sidebar toggle events
    this.sidebarToggleSubscription = this.sidebarService.onToggle().subscribe({
      next: (event) => {
        this.isExpandedHeader = !this.isExpandedHeader;
        console.log('Sidebar toggled:', event);
      },
    });


    // this.menuService.onItemClick().pipe(takeUntil(this.$destroy)).subscribe(
    //   {
    //     next: ()=>{
    //       setTimeout(()=>{
    //         const { sm , lg} = this.breakpointService.getBreakpointsMap(); 
    //         const width = document.documentElement.clientWidth
    //         if ( width < sm){
    //           this.sidebarService.collapse('menu-sidebar')
    //         } else if (width < lg){
    //           this.sidebarService.compact('menu-sidebar')
    //         }
    //       },30)
    //     }
    //   }
    // )
  }
  toggleSidebar(): boolean {
    this.sidebarService.toggle(true, 'menu-sidebar');
    this.layoutService.changeLayoutSize();

    return false;
  }


  ngOnDestroy(): void {

  }
}
