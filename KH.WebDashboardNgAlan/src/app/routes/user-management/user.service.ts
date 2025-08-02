import { Injectable } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import { UserFilterRequest } from 'src/app/core/models/extentions/user-filter-request';

export interface User {
   id: number;
   username: string;
   email: string;
   phone_number: string;
   national_id: string;
   branch: string;
   name_ar: string;
   name_en: string;
   role: string;
   avatar?: string;
   is_active: boolean;
   created_by: string;
   created_at: string;
   expand?: boolean;
}

export interface UserResponse {
   data: User[];
   total: number;
   pageIndex: number;
   pageSize: number;
}

@Injectable({
   providedIn: 'root'
})
export class UserService {
   private users: User[] = this.generateMockUsers();

   constructor() { }

   getUsers(filter: UserFilterRequest): Observable<UserResponse> {
      let filteredUsers = [...this.users];

      // Apply search filter
      if (filter.search) {
         const searchTerm = filter.search.toLowerCase();
         filteredUsers = filteredUsers.filter(user =>
            user.username.toLowerCase().includes(searchTerm) ||
            user.email.toLowerCase().includes(searchTerm) ||
            user.name_ar.toLowerCase().includes(searchTerm) ||
            user.name_en.toLowerCase().includes(searchTerm) ||
            user.phone_number.includes(searchTerm) ||
            user.national_id.includes(searchTerm) ||
            user.branch.toLowerCase().includes(searchTerm) ||
            user.role.toLowerCase().includes(searchTerm)
         );
      }

      // Apply specific filters
      if (filter.username) {
         filteredUsers = filteredUsers.filter(user =>
            user.username.toLowerCase().includes(filter.username!.toLowerCase())
         );
      }

      if (filter.email) {
         filteredUsers = filteredUsers.filter(user =>
            user.email.toLowerCase().includes(filter.email!.toLowerCase())
         );
      }

      if (filter.role) {
         filteredUsers = filteredUsers.filter(user =>
            user.role.toLowerCase().includes(filter.role!.toLowerCase())
         );
      }

      if (filter.branch) {
         filteredUsers = filteredUsers.filter(user =>
            user.branch.toLowerCase().includes(filter.branch!.toLowerCase())
         );
      }

      if (filter.is_active !== undefined) {
         filteredUsers = filteredUsers.filter(user => user.is_active === filter.is_active);
      }

      // Apply sorting
      if (filter.sortKey && filter.sortOrder) {
         filteredUsers.sort((a, b) => {
            const aValue = this.getPropertyValue(a, filter.sortKey!);
            const bValue = this.getPropertyValue(b, filter.sortKey!);

            if (filter.sortOrder === 'Asc') {
               return aValue > bValue ? 1 : -1;
            } else {
               return aValue < bValue ? 1 : -1;
            }
         });
      }

      const total = filteredUsers.length;
      const pageIndex = filter.pageIndex || 1;
      const pageSize = filter.pageSize || 10;
      const startIndex = (pageIndex - 1) * pageSize;
      const endIndex = startIndex + pageSize;
      const paginatedUsers = filteredUsers.slice(startIndex, endIndex);



      return of({
         data: paginatedUsers,
         total,
         pageIndex,
         pageSize
      }).pipe(delay(500)); // Simulate API delay
   }

   private getPropertyValue(user: User, key: string): any {
      switch (key.toLowerCase()) {
         case 'id': return user.id;
         case 'username': return user.username;
         case 'email': return user.email;
         case 'phone_number': return user.phone_number;
         case 'national_id': return user.national_id;
         case 'branch': return user.branch;
         case 'name_ar': return user.name_ar;
         case 'name_en': return user.name_en;
         case 'role': return user.role;
         case 'is_active': return user.is_active;
         case 'created_by': return user.created_by;
         case 'created_at': return user.created_at;
         default: return user.id;
      }
   }

   private generateMockUsers(): User[] {
      const arabicNames = [
         { ar: 'أحمد محمد علي', en: 'Ahmed Mohamed Ali' },
         { ar: 'فاطمة أحمد حسن', en: 'Fatima Ahmed Hassan' },
         { ar: 'محمد عبدالله محمود', en: 'Mohamed Abdullah Mahmoud' },
         { ar: 'عائشة محمد سعيد', en: 'Aisha Mohamed Saeed' },
         { ar: 'علي أحمد فتحي', en: 'Ali Ahmed Fathy' },
         { ar: 'مريم عبدالرحمن', en: 'Mariam Abdel Rahman' },
         { ar: 'حسن محمد إبراهيم', en: 'Hassan Mohamed Ibrahim' },
         { ar: 'زينب أحمد محمد', en: 'Zeinab Ahmed Mohamed' },
         { ar: 'يوسف علي حسن', en: 'Youssef Ali Hassan' },
         { ar: 'خديجة محمد عبدالله', en: 'Khadija Mohamed Abdullah' },
         { ar: 'عبدالله أحمد علي', en: 'Abdullah Ahmed Ali' },
         { ar: 'نور محمد حسن', en: 'Nour Mohamed Hassan' },
         { ar: 'محمود أحمد فتحي', en: 'Mahmoud Ahmed Fathy' },
         { ar: 'سارة محمد علي', en: 'Sarah Mohamed Ali' },
         { ar: 'عمر عبدالله محمود', en: 'Omar Abdullah Mahmoud' },
         { ar: 'ليلى أحمد حسن', en: 'Layla Ahmed Hassan' },
         { ar: 'كريم محمد سعيد', en: 'Karim Mohamed Saeed' },
         { ar: 'رنا عبدالرحمن', en: 'Rana Abdel Rahman' },
         { ar: 'طارق أحمد محمد', en: 'Tarek Ahmed Mohamed' },
         { ar: 'هند محمد علي', en: 'Hind Mohamed Ali' },
         { ar: 'سامر عبدالله حسن', en: 'Samir Abdullah Hassan' },
         { ar: 'نادية أحمد فتحي', en: 'Nadia Ahmed Fathy' },
         { ar: 'باسم محمد محمود', en: 'Bassem Mohamed Mahmoud' },
         { ar: 'دينا عبدالرحمن', en: 'Dina Abdel Rahman' },
         { ar: 'وائل أحمد علي', en: 'Wael Ahmed Ali' },
         { ar: 'رانيا محمد حسن', en: 'Rania Mohamed Hassan' },
         { ar: 'أيمن عبدالله سعيد', en: 'Ayman Abdullah Saeed' },
         { ar: 'سلمى أحمد محمد', en: 'Salma Ahmed Mohamed' },
         { ar: 'خالد محمد علي', en: 'Khaled Mohamed Ali' },
         { ar: 'نورا عبدالرحمن', en: 'Nora Abdel Rahman' },
         { ar: 'محمد أحمد حسن', en: 'Mohamed Ahmed Hassan' },
         { ar: 'هبة محمد فتحي', en: 'Heba Mohamed Fathy' },
         { ar: 'أحمد عبدالله محمود', en: 'Ahmed Abdullah Mahmoud' },
         { ar: 'مروة محمد علي', en: 'Marwa Mohamed Ali' },
         { ar: 'علي أحمد سعيد', en: 'Ali Ahmed Saeed' },
         { ar: 'فاطمة عبدالرحمن', en: 'Fatima Abdel Rahman' },
         { ar: 'حسن محمد أحمد', en: 'Hassan Mohamed Ahmed' },
         { ar: 'زينب علي محمد', en: 'Zeinab Ali Mohamed' },
         { ar: 'يوسف أحمد حسن', en: 'Youssef Ahmed Hassan' },
         { ar: 'خديجة محمد فتحي', en: 'Khadija Mohamed Fathy' },
         { ar: 'عبدالله علي محمود', en: 'Abdullah Ali Mahmoud' },
         { ar: 'نور أحمد محمد', en: 'Nour Ahmed Mohamed' },
         { ar: 'محمود محمد علي', en: 'Mahmoud Mohamed Ali' },
         { ar: 'سارة عبدالله حسن', en: 'Sarah Abdullah Hassan' },
         { ar: 'عمر أحمد فتحي', en: 'Omar Ahmed Fathy' },
         { ar: 'ليلى محمد سعيد', en: 'Layla Mohamed Saeed' },
         { ar: 'كريم عبدالرحمن', en: 'Karim Abdel Rahman' },
         { ar: 'رنا أحمد محمد', en: 'Rana Ahmed Mohamed' },
         { ar: 'طارق محمد علي', en: 'Tarek Mohamed Ali' },
         { ar: 'هند عبدالله حسن', en: 'Hind Abdullah Hassan' },
         { ar: 'سامر أحمد فتحي', en: 'Samir Ahmed Fathy' },
         { ar: 'نادية محمد محمود', en: 'Nadia Mohamed Mahmoud' },
         { ar: 'باسم عبدالرحمن', en: 'Bassem Abdel Rahman' },
         { ar: 'دينا أحمد علي', en: 'Dina Ahmed Ali' },
         { ar: 'وائل محمد حسن', en: 'Wael Mohamed Hassan' },
         { ar: 'رانيا عبدالله سعيد', en: 'Rania Abdullah Saeed' },
         { ar: 'أيمن أحمد محمد', en: 'Ayman Ahmed Mohamed' },
         { ar: 'سلمى محمد علي', en: 'Salma Mohamed Ali' },
         { ar: 'خالد عبدالرحمن', en: 'Khaled Abdel Rahman' },
         { ar: 'نورا أحمد فتحي', en: 'Nora Ahmed Fathy' },
         { ar: 'محمد محمد محمود', en: 'Mohamed Mohamed Mahmoud' },
         { ar: 'هبة عبدالله علي', en: 'Heba Abdullah Ali' },
         { ar: 'أحمد أحمد حسن', en: 'Ahmed Ahmed Hassan' },
         { ar: 'مروة محمد سعيد', en: 'Marwa Mohamed Saeed' },
         { ar: 'علي عبدالرحمن', en: 'Ali Abdel Rahman' },
         { ar: 'فاطمة أحمد محمد', en: 'Fatima Ahmed Mohamed' }
      ];

      const roles = [
         'Doctor',
         'Nurse',
         'Receptionist',
         'Pharmacist',
         'Lab Technician',
         'Radiologist',
         'Administrator',
         'Accountant',
         'Security Guard',
         'Cleaner'
      ];

      const branches = [
         'Cairo Main Branch',
         'Alexandria Branch',
         'Giza Branch',
         'Sharm El Sheikh Branch',
         'Hurghada Branch',
         'Luxor Branch',
         'Aswan Branch',
         'Port Said Branch',
         'Suez Branch',
         'Ismailia Branch'
      ];

      const users: User[] = [];

      for (let i = 1; i <= 50; i++) {
         const nameIndex = (i - 1) % arabicNames.length;
         const name = arabicNames[nameIndex];
         const role = roles[Math.floor(Math.random() * roles.length)];
         const branch = branches[Math.floor(Math.random() * branches.length)];

         // Generate Egyptian phone number
         const phonePrefixes = ['010', '011', '012', '015'];
         const phonePrefix = phonePrefixes[Math.floor(Math.random() * phonePrefixes.length)];
         const phoneSuffix = Math.floor(Math.random() * 90000000) + 10000000;
         const phoneNumber = phonePrefix + phoneSuffix.toString();

         // Generate Egyptian National ID (14 digits)
         const nationalId = Math.floor(Math.random() * 90000000000000) + 10000000000000;

         // Generate email
         const email = `${name.en.toLowerCase().replace(/\s+/g, '.')}${i}@clinic.com`;

         // Generate username
         const username = `${name.en.toLowerCase().replace(/\s+/g, '')}${i}`;

         users.push({
            id: i,
            username,
            email,
            phone_number: phoneNumber,
            national_id: nationalId.toString(),
            branch,
            name_ar: name.ar,
            name_en: name.en,
            role,
            avatar: `https://api.dicebear.com/7.x/avataaars/svg?seed=${username}`,
            is_active: Math.random() > 0.2, // 80% active
            created_by: 'System Admin',
            created_at: new Date(Date.now() - Math.random() * 365 * 24 * 60 * 60 * 1000).toISOString()
         });
      }

      return users;
   }

   updateUserStatus(userId: number, isActive: boolean): Observable<boolean> {
      const user = this.users.find(u => u.id === userId);
      if (user) {
         user.is_active = isActive;
         return of(true).pipe(delay(300));
      }
      return of(false).pipe(delay(300));
   }

   getUserById(userId: number): Observable<User | null> {
      console.log('UserService.getUserById called with ID:', userId);
      const user = this.users.find(u => u.id === userId);
      console.log('Found user:', user);
      return of(user || null).pipe(delay(300));
   }

   createUser(formData: FormData): Observable<User> {
      // Simulate API call with FormData
      const username = formData.get('username') as string;
      const avatarFile = formData.get('avatar_file') as File;

      // Generate avatar URL based on whether a file was uploaded or not
      let avatarUrl: string;
      if (avatarFile) {
         // In a real application, you would upload the file to your server
         // and get back a URL. For now, we'll use a placeholder
         avatarUrl = `https://api.dicebear.com/7.x/avataaars/svg?seed=${username}&uploaded=true`;
      } else {
         avatarUrl = `https://api.dicebear.com/7.x/avataaars/svg?seed=${username}`;
      }

      return of({
         id: this.users.length + 1,
         username: username,
         email: formData.get('email') as string,
         phone_number: formData.get('phone_number') as string,
         national_id: formData.get('national_id') as string,
         branch: formData.get('branch') as string,
         name_ar: formData.get('name_ar') as string,
         name_en: formData.get('name_en') as string,
         role: formData.get('role') as string,
         avatar: avatarUrl,
         is_active: formData.get('is_active') === 'true',
         created_by: 'Current User',
         created_at: new Date().toISOString()
      }).pipe(delay(1000));
   }

   updateUser(formData: FormData): Observable<User> {
      // Simulate API call with FormData for update
      const userId = parseInt(formData.get('id') as string);
      const username = formData.get('username') as string;
      const avatarFile = formData.get('avatar_file') as File;

      // Find existing user
      const existingUser = this.users.find(u => u.id === userId);
      if (!existingUser) {
         throw new Error('User not found');
      }

      // Generate avatar URL based on whether a file was uploaded or not
      let avatarUrl: string;
      if (avatarFile) {
         avatarUrl = `https://api.dicebear.com/7.x/avataaars/svg?seed=${username}&uploaded=true`;
      } else {
         avatarUrl = existingUser.avatar || `https://api.dicebear.com/7.x/avataaars/svg?seed=${username}`;
      }

      // Update the user data
      const updatedUser: User = {
         ...existingUser,
         username: username,
         email: formData.get('email') as string,
         phone_number: formData.get('phone_number') as string,
         national_id: formData.get('national_id') as string,
         branch: formData.get('branch') as string,
         name_ar: formData.get('name_ar') as string,
         name_en: formData.get('name_en') as string,
         role: formData.get('role') as string,
         avatar: avatarUrl,
         is_active: formData.get('is_active') === 'true'
      };

      // Update in the users array
      const userIndex = this.users.findIndex(u => u.id === userId);
      if (userIndex !== -1) {
         this.users[userIndex] = updatedUser;
      }

      return of(updatedUser).pipe(delay(1000));
   }
} 