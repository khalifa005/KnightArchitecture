import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS: NbMenuItem[] = [
  {
    title: 'sideMenu.home',
    icon: 'home-outline',
    link: '/pages/home',
    home: true,
    data: {
      translationKey: 'sideMenu.home',
      permissionKey: 'view-home',
    },
  },
  {
    title: 'sideMenu.userManagement',
    icon: 'keypad-outline',
    data: {
      translationKey: 'sideMenu.userManagement',
      permissionKey: 'manage-user-management',
    },
    children: [

      {
        title: 'sideMenu.permissions',
        icon: 'eye-outline',
        link: '/pages/manage/permissions',
        data: {
          translationKey: 'sideMenu.permissions',
          permissionKey: 'manage-permissions',
        },
      },
      {
        title: 'sideMenu.roles',
        icon: 'briefcase-outline', // Updated icon for roles
        link: '/pages/manage/roles',
        data: {
          translationKey: 'sideMenu.roles',
          permissionKey: 'manage-permissions',//updated in backend
        },
      },
      {
        title: 'sideMenu.departments',
        icon: 'layers-outline', // Updated icon for departments
        link: '/pages/manage/departments',
        data: {
          translationKey: 'sideMenu.departments',
          permissionKey: '',//updated in backend
        },
      },
      {
        title: 'sideMenu.users',
        icon: 'people-outline',
        link: '/pages/manage/users',
        data: {
          translationKey: 'sideMenu.users',
          permissionKey: 'manage-users',
        },
      },
      {
        title: 'sideMenu.audit',
        icon: 'file-text-outline', // Selected icon for audit
        link: '/pages/manage/audit',
        data: {
          translationKey: 'sideMenu.audit',
          permissionKey: '',
        },
      },
    ],
  },
  {
    title: 'sideMenu.permissions-test',
    icon: 'eye-outline',
    link: '/pages/permissions',
    data: {
      translationKey: 'sideMenu.permissions-test',
      permissionKey: 'view-permissions',
    },
  },
  {
    title: 'sideMenu.chat',
    icon: 'message-square-outline',
    link: '/pages/chat',
    data: {
      translationKey: 'sideMenu.cchat',
      permissionKey: '',
    },
  },
  {
    title: 'sideMenu.cchat',
    icon: 'message-circle-outline',
    link: '/pages/cchat',
    data: {
      translationKey: 'sideMenu.chat',
      permissionKey: '',
    },
  },

  {
    title: 'sideMenu.dynamicTable',
    icon: 'grid-outline',
    link: '/pages/dynamic-table',
    data: {
      translationKey: 'sideMenu.dynamicTable',
      permissionKey: 'view-dynamic-table',
    },
  },
  {
    title: 'sideMenu.cardsExamples',
    icon: 'browser-outline',
    link: '/pages/cards-examples',
    data: {
      translationKey: 'sideMenu.cardsExamples',
      permissionKey: 'view-cards-examples',
    },
  },
  {
    title: 'sideMenu.bootstrapExamples',
    icon: 'layers-outline',
    link: '/pages/bootstrap-examples',
    data: {
      translationKey: 'sideMenu.bootstrapExamples',
      permissionKey: 'view-bootstrap-examples',
    },
  },
  {
    title: 'sideMenu.iconsExamples',
    icon: 'star-outline',
    link: '/pages/icons-examples',
    data: {
      translationKey: 'sideMenu.iconsExamples',
      permissionKey: 'view-icons-examples',
    },
  },
  {
    title: 'sideMenu.inputsExamples',
    icon: 'edit-outline',
    link: '/pages/inputs-examples',
    data: {
      translationKey: 'sideMenu.inputsExamples',
      permissionKey: 'manage-inputs-examples',
    },
  },
  {
    title: 'sideMenu.formExamples',
    icon: 'file-text-outline',
    link: '/pages/form-examples',
    data: {
      translationKey: 'sideMenu.formExamples',
      permissionKey: 'view-form-examples',
    },
  },
  {
    title: 'sideMenu.dynamicForm',
    icon: 'edit-2-outline',
    link: '/pages/dynamic-form',
    data: {
      translationKey: 'sideMenu.dynamicForm',
      permissionKey: 'manage-dynamic-form',
    },
  },
  {
    title: 'sideMenu.tabs',
    icon: 'options-outline', // Select an appropriate icon
    link: '/pages/tabs',
    data: {
      translationKey: 'sideMenu.tabs',
      permissionKey: '', // Add permission if needed
    },
  },
  {
    title: 'sideMenu.buttons',
    icon: 'radio-button-on-outline', // Select an appropriate icon
    link: '/pages/buttons',
    data: {
      translationKey: 'sideMenu.buttons',
      permissionKey: '', // Add permission if needed
    },
  },
  {
    title: 'sideMenu.pipes',
    icon: 'funnel-outline', // Select an appropriate icon
    link: '/pages/pipes',
    data: {
      translationKey: 'sideMenu.pipes',
      permissionKey: '', // Add permission if needed
    },
  },
  {
    title: 'sideMenu.spinner',
    icon: 'loader-outline', // Select an appropriate icon
    link: '/pages/spinner',
    data: {
      translationKey: 'sideMenu.spinner',
      permissionKey: '', // Add permission if needed
    },
  },
  {
    title: 'sideMenu.auth',
    icon: 'lock-outline',
    link: '/auth',
    data: {
      translationKey: 'sideMenu.auth',
      permissionKey: 'access-auth',
    },
  },
  {
    title: 'sideMenu.notFound',
    icon: 'close-circle-outline',
    link: '/pages/whatever',
    data: {
      translationKey: 'sideMenu.notFound',
      permissionKey: 'view-not-found',
    },
  },

];
