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
        fixed>
        <ngx-header></ngx-header>
      </nb-layout-header>

      <nb-sidebar class="menu-sidebar" tag="menu-sidebar" responsive start>
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
