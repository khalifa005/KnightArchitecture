// user.interface.ts
export interface AppUser {
    name: string;
    avatar: string;
    email: string;
    // Extended properties
    department?: string;
    role?: string;
    permissions?: string[];
    lastLogin?: Date;
  }
  
  
  declare module '@delon/theme' {
    interface User extends AppUser {}
  }
  
