
export interface Tab {
  id: string;
  label: string;
  isActive: boolean;
  content: string;
  count?: number; // Optional count for notifications
  icon: string; // Bootstrap icon class

}
