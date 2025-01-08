import { PermissionResponse } from "../models/responses/permission-response";

export const STATIC_PERMISSIONS: PermissionResponse[] = [
  new PermissionResponse({
    id: 1,
    key: 'dashboard_view',
    nameEn: 'View Dashboard',
    nameAr: 'عرض لوحة التحكم',
    disabled: false,
    checked: false,
    childrens: [],
    sortKey: 1,
    parentId: null
  }),
  new PermissionResponse({
    id: 2,
    key: 'users_management',
    nameEn: 'Manage Users',
    nameAr: 'إدارة المستخدمين',
    disabled: false,
    checked: false,
    childrens: [
      new PermissionResponse({
        id: 3,
        key: 'users_add',
        nameEn: 'Add Users',
        nameAr: 'إضافة المستخدمين',
        disabled: false,
        checked: false,
        childrens: [],
        sortKey: 1,
        parentId: 2
      }),
      new PermissionResponse({
        id: 4,
        key: 'users_edit',
        nameEn: 'Edit Users',
        nameAr: 'تعديل المستخدمين',
        disabled: false,
        checked: false,
        childrens: [],
        sortKey: 2,
        parentId: 2
      }),
    ],
    sortKey: 2,
    parentId: null
  }),
  new PermissionResponse({
    id: 5,
    key: 'settings',
    nameEn: 'Settings',
    nameAr: 'الإعدادات',
    disabled: true,
    checked: false,
    childrens: [
      new PermissionResponse({
        id: 6,
        key: 'general_settings',
        nameEn: 'General Settings',
        nameAr: 'الإعدادات العامة',
        disabled: false,
        checked: false,
        childrens: [],
        sortKey: 1,
        parentId: 5
      }),
      new PermissionResponse({
        id: 7,
        key: 'advanced_settings',
        nameEn: 'Advanced Settings',
        nameAr: 'الإعدادات المتقدمة',
        disabled: true,
        checked: false,
        childrens: [],
        sortKey: 2,
        parentId: 5
      }),
    ],
    sortKey: 3,
    parentId: null
  })
];
