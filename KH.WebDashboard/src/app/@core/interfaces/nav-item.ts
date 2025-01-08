export interface NavItem {
  displayName: string;
  iconName: string;
  route?: string;
  windowSize?: string;
  children?: NavItem[];
}
