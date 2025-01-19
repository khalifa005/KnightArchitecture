import { Component, OnDestroy, OnInit } from '@angular/core';
import { NbMediaBreakpointsService, NbMenuService, NbSidebarService, NbThemeService } from '@nebular/theme';

import { LayoutService } from '../../../@core/utils.ts';
import { map, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { AuthService } from '../../../@auth/services/custom-auth-service';

@Component({
  selector: 'ngx-header',
  styleUrls: ['./header.component.scss'],
  templateUrl: './header.component.html',
})
export class HeaderComponent implements OnInit, OnDestroy {

  private destroy$: Subject<void> = new Subject<void>();
  userPictureOnly: boolean = false;
  user: any;
  userPicture: string = "./assets/images/logo.png";

  currentTime: string = '';
  currentPeriod: string = '';
  private timerInterval!: any;

  themes = [
    {
      value: 'default',
      name: 'Light',
    },
    {
      value: 'dark',
      name: 'Dark',
    },
    {
      value: 'cosmic',
      name: 'Cosmic',
    },
    {
      value: 'corporate',
      name: 'Corporate',
    },
  ];

  currentTheme = 'default';
  userFullName = "";
  // userMenu = [ { title: 'Profile' }, { title: 'Log out' } ];
  // userMenu = [ { title: 'Profile' }, { title: 'Log out'  } ];
  userMenu = [
    { title: 'Profile', action: 'profile' },
    { title: 'Log out', link: '/auth/logout', action: 'logout' }
  ];

  headerClassCompacted = "custom-header-compacted";
  headerClassExpanded = "custom-header-expanded";
  isExpandedHeader = true;

  constructor(private sidebarService: NbSidebarService,
    private menuService: NbMenuService,
    private authService: AuthService,
    private themeService: NbThemeService,
    private layoutService: LayoutService,
    private breakpointService: NbMediaBreakpointsService) {

    this.userFullName = this.authService.getUser().firstName;

  }

  ngOnInit() {
    this.updateTime(); // Initial time update
    this.timerInterval = setInterval(() => this.updateTime(), 1000); // Update every second

    this.currentTheme = this.themeService.currentTheme;

    const { xl } = this.breakpointService.getBreakpointsMap();
    this.themeService.onMediaQueryChange()
      .pipe(
        map(([, currentBreakpoint]) => currentBreakpoint.width < xl),
        takeUntil(this.destroy$),
      )
      .subscribe((isLessThanXl: boolean) => this.userPictureOnly = isLessThanXl);

    this.themeService.onThemeChange()
      .pipe(
        map(({ name }) => name),
        takeUntil(this.destroy$),
      )
      .subscribe(themeName => this.currentTheme = themeName);

          // Listen to sidebar toggle events
     this.sidebarService.onToggle().subscribe({
      next: (event) => {
        this.isExpandedHeader = !this.isExpandedHeader;
        console.log('Sidebar toggled:', event);
      },
    });

  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  changeTheme(themeName: string) {
    this.themeService.changeTheme(themeName);
  }

  toggleSidebar(): boolean {
    this.sidebarService.toggle(true, 'menu-sidebar');
    this.layoutService.changeLayoutSize();

    return false;
  }

  navigateHome() {
    this.menuService.navigateHome();
    return false;
  }

  onDirectionSwitch(): void {
    // location.reload();
  }

  updateTime(): void {
    const date = new Date();
    let hours = date.getHours();
    const minutes = date.getMinutes();
    this.currentPeriod = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12 || 12; // Convert to 12-hour format
    this.currentTime = `${this.formatNumber(hours)}:${this.formatNumber(minutes)}`;
  }

  formatNumber(num: number): string {
    return num < 10 ? `0${num}` : `${num}`;
  }
}
