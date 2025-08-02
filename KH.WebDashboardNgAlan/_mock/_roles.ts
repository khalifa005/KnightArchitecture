import { MockRequest } from '@delon/mock';

// Mock data for roles
const roles = [
   {
      id: 1,
      name_en: 'Administrator',
      name_ar: 'مدير النظام',
      created_by: 'John Doe',
      edited_by: 'Jane Smith',
      created_at: new Date('2024-01-15'),
      updated_at: new Date('2024-03-20')
   },
   {
      id: 2,
      name_en: 'Doctor',
      name_ar: 'طبيب',
      created_by: 'Admin User',
      edited_by: 'Admin User',
      created_at: new Date('2024-01-20'),
      updated_at: new Date('2024-02-15')
   },
   {
      id: 3,
      name_en: 'Nurse',
      name_ar: 'ممرض',
      created_by: 'System Admin',
      edited_by: 'John Doe',
      created_at: new Date('2024-02-01'),
      updated_at: new Date('2024-03-10')
   },
   {
      id: 4,
      name_en: 'Receptionist',
      name_ar: 'موظف استقبال',
      created_by: 'Jane Smith',
      edited_by: 'Jane Smith',
      created_at: new Date('2024-02-10'),
      updated_at: new Date('2024-03-05')
   },
   {
      id: 5,
      name_en: 'Pharmacist',
      name_ar: 'صيدلي',
      created_by: 'Admin User',
      edited_by: 'System Admin',
      created_at: new Date('2024-02-15'),
      updated_at: new Date('2024-03-18')
   },
   {
      id: 6,
      name_en: 'Lab Technician',
      name_ar: 'فني مختبر',
      created_by: 'John Doe',
      edited_by: 'John Doe',
      created_at: new Date('2024-02-20'),
      updated_at: new Date('2024-03-12')
   },
   {
      id: 7,
      name_en: 'Radiologist',
      name_ar: 'أخصائي أشعة',
      created_by: 'System Admin',
      edited_by: 'Jane Smith',
      created_at: new Date('2024-03-01'),
      updated_at: new Date('2024-03-25')
   },
   {
      id: 8,
      name_en: 'Patient',
      name_ar: 'مريض',
      created_by: 'Admin User',
      edited_by: 'Admin User',
      created_at: new Date('2024-03-05'),
      updated_at: new Date('2024-03-22')
   },
   {
      id: 9,
      name_en: 'Manager',
      name_ar: 'مدير',
      created_by: 'Jane Smith',
      edited_by: 'System Admin',
      created_at: new Date('2024-03-10'),
      updated_at: new Date('2024-03-28')
   },
   {
      id: 10,
      name_en: 'Supervisor',
      name_ar: 'مشرف',
      created_by: 'John Doe',
      edited_by: 'John Doe',
      created_at: new Date('2024-03-15'),
      updated_at: new Date('2024-03-30')
   }
];

// Generate more roles for pagination testing
function generateMoreRoles(count: number = 50): any[] {
   const additionalRoles = [];
   const roleTypes = [
      { en: 'Specialist', ar: 'أخصائي' },
      { en: 'Consultant', ar: 'استشاري' },
      { en: 'Resident', ar: 'مقيم' },
      { en: 'Intern', ar: 'طبيب متدرب' },
      { en: 'Technician', ar: 'فني' },
      { en: 'Coordinator', ar: 'منسق' },
      { en: 'Assistant', ar: 'مساعد' },
      { en: 'Clerk', ar: 'كاتب' },
      { en: 'Guard', ar: 'حارس' },
      { en: 'Cleaner', ar: 'منظف' }
   ];

   const creators = ['Admin User', 'John Doe', 'Jane Smith', 'System Admin', 'Manager User'];

   for (let i = 11; i <= count; i++) {
      const roleType = roleTypes[i % roleTypes.length];
      const creator = creators[i % creators.length];
      const editor = creators[(i + 1) % creators.length];

      additionalRoles.push({
         id: i,
         name_en: `${roleType.en} ${Math.floor(i / 10) + 1}`,
         name_ar: `${roleType.ar} ${Math.floor(i / 10) + 1}`,
         created_by: creator,
         edited_by: editor,
         created_at: new Date(2024, 0, 15 + (i % 30)),
         updated_at: new Date(2024, 2, 1 + (i % 30))
      });
   }

   return additionalRoles;
}

const allRoles = [...roles, ...generateMoreRoles(100)];

export const ROLES = {
   'GET /api/roles': (req: MockRequest) => {
      const { pi = 1, ps = 10, name_en, name_ar, created_by, edited_by, sort } = req.queryString;

      let filteredRoles = [...allRoles];

      // Apply filters
      if (name_en) {
         filteredRoles = filteredRoles.filter(role =>
            role.name_en.toLowerCase().includes(name_en.toLowerCase())
         );
      }

      if (name_ar) {
         filteredRoles = filteredRoles.filter(role =>
            role.name_ar.includes(name_ar)
         );
      }

      if (created_by) {
         filteredRoles = filteredRoles.filter(role =>
            role.created_by.toLowerCase().includes(created_by.toLowerCase())
         );
      }

      if (edited_by) {
         filteredRoles = filteredRoles.filter(role =>
            role.edited_by.toLowerCase().includes(edited_by.toLowerCase())
         );
      }

      // Apply sorting
      if (sort) {
         const [field, order] = sort.split(',');
         filteredRoles.sort((a, b) => {
            let aVal = a[field];
            let bVal = b[field];

            if (field === 'created_at' || field === 'updated_at') {
               aVal = new Date(aVal).getTime();
               bVal = new Date(bVal).getTime();
            } else if (typeof aVal === 'string') {
               aVal = aVal.toLowerCase();
               bVal = bVal.toLowerCase();
            }

            if (aVal < bVal) return order === 'ascend' ? -1 : 1;
            if (aVal > bVal) return order === 'ascend' ? 1 : -1;
            return 0;
         });
      }

      // Apply pagination
      const total = filteredRoles.length;
      const startIndex = (pi - 1) * ps;
      const endIndex = startIndex + ps;
      const paginatedRoles = filteredRoles.slice(startIndex, endIndex);

      return {
         list: paginatedRoles,
         total,
         pi: parseInt(pi),
         ps: parseInt(ps)
      };
   },

   'GET /api/roles/:id': (req: MockRequest) => {
      const { id } = req.params;
      const role = allRoles.find(r => r.id === parseInt(id));

      if (!role) {
         return { error: 'Role not found' };
      }

      return role;
   },

   'POST /api/roles': (req: MockRequest) => {
      const newRole = {
         id: allRoles.length + 1,
         ...req.body,
         created_at: new Date(),
         updated_at: new Date()
      };

      allRoles.push(newRole);
      return newRole;
   },

   'PUT /api/roles/:id': (req: MockRequest) => {
      const { id } = req.params;
      const roleIndex = allRoles.findIndex(r => r.id === parseInt(id));

      if (roleIndex === -1) {
         return { error: 'Role not found' };
      }

      allRoles[roleIndex] = {
         ...allRoles[roleIndex],
         ...req.body,
         updated_at: new Date()
      };

      return allRoles[roleIndex];
   },

   'DELETE /api/roles/:id': (req: MockRequest) => {
      const { id } = req.params;
      const roleIndex = allRoles.findIndex(r => r.id === parseInt(id));

      if (roleIndex === -1) {
         return { error: 'Role not found' };
      }

      const deletedRole = allRoles.splice(roleIndex, 1)[0];
      return { success: true, deletedRole };
   }
}; 