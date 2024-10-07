using KH.BuildingBlocks.Constant;

namespace KH.PersistenceInfra.Data.Seed;

public class PermissionsContextSeeder
{
  public const int DefaultSortKey = 1;

  public static void SeedSystemPermissions(ModelBuilder builder, ILoggerFactory loggerFactory)
  {
    var logger = loggerFactory.CreateLogger<LookupContextSeeder>();

    try
    {
      List<Permission> entities = new List<Permission>();

      AddLookupPermissions(entities);
      AddUserManagementPermissions(entities);
      AddServiceOnePermissions(entities);
      AddCustomerPermissions(entities);
      AddAuditPermissions(entities);
      AddDepartmentPermissions(entities);
      AddEmailPermissions(entities);
      AddMediaPermissions(entities);
      AddPdfPermissions(entities);
      AddPermissionManagementPermissions(entities);

      builder.Entity<Permission>().HasData(entities);
    }
    catch (Exception ex)
    {
      logger.LogError($"Error occurred while seeding permissions: {ex.Message}", ex);
    }
  }

  // Define a fixed date for the seed data to avoid changes in migration
  private static readonly DateTime SeedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

  private static void AddLookupPermissions(List<Permission> entities)
  {
    entities.AddRange(new[]
    {
        new Permission { Id = 1, Key = PermissionKeysConstant.Lookups.LOOKUPS, NameEn = "Basic Data", NameAr = "البيانات الاساسية", SortKey = DefaultSortKey, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 2, Key = PermissionKeysConstant.Lookups.CATEGORIES, NameEn = "Request Categories", NameAr = "فئات الطلبات", SortKey = DefaultSortKey, ParentId = 1, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 3, Key = PermissionKeysConstant.Lookups.ADD_CATEGORY, NameEn = "Add Request Category", NameAr = "إضافة فئة طلب", SortKey = DefaultSortKey, ParentId = 2, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 4, Key = PermissionKeysConstant.Lookups.EDIT_CATEGORY, NameEn = "Edit Request Category", NameAr = "تعديل فئة طلب", SortKey = 2, ParentId = 2, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 5, Key = PermissionKeysConstant.Lookups.DELETE_CATEGORY, NameEn = "Delete Request Category", NameAr = "حذف فئة طلب", SortKey = 3, ParentId = 2, CreatedDate = SeedDate, UpdatedDate = SeedDate },

        new Permission { Id = 6, Key = PermissionKeysConstant.Lookups.STATUS, NameEn = "Request Status", NameAr = "حالات الطلبات", SortKey = 2, ParentId = 1, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 8, Key = PermissionKeysConstant.Lookups.EDIT_STATUS, NameEn = "Edit Request Status", NameAr = "تعديل حالة طلب", SortKey = 2, ParentId = 6, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 9, Key = PermissionKeysConstant.Lookups.DELETE_STATUS, NameEn = "Delete Request Status", NameAr = "حذف حالة طلب", SortKey = 3, ParentId = 6, CreatedDate = SeedDate, UpdatedDate = SeedDate }
    });
  }

  private static void AddUserManagementPermissions(List<Permission> entities)
  {
    entities.AddRange(new[]
    {
        new Permission { Id = 69, Key = PermissionKeysConstant.UserManagement.USER_MANAGEMENT, NameEn = "User Management", NameAr = "الإدارة", SortKey = 2, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 70, Key = PermissionKeysConstant.UserManagement.ROLES_MANAGEMENT, NameEn = "Roles Management", NameAr = "إدارة أدوار المستخدمين", SortKey = DefaultSortKey, ParentId = 69, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 71, Key = PermissionKeysConstant.UserManagement.USERS, NameEn = "Users", NameAr = "المستخدمين", SortKey = 2, ParentId = 69, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 72, Key = PermissionKeysConstant.UserManagement.ADD_USER, NameEn = "Add User", NameAr = "إضافة مستخدم", SortKey = DefaultSortKey, ParentId = 71, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 73, Key = PermissionKeysConstant.UserManagement.EDIT_USER, NameEn = "Edit User", NameAr = "تعديل مستخدم", SortKey = 2, ParentId = 71, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 74, Key = PermissionKeysConstant.UserManagement.DELETE_USER, NameEn = "Delete User", NameAr = "حذف مستخدم", SortKey = 3, ParentId = 71, CreatedDate = SeedDate, UpdatedDate = SeedDate },

        new Permission { Id = 75, Key = PermissionKeysConstant.UserManagement.CALENDAR, NameEn = "Calendar", NameAr = "التورايخ", SortKey = 3, ParentId = 69, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 76, Key = PermissionKeysConstant.UserManagement.ADD_CALENDAR_HOLIDAY, NameEn = "Add Calendar Holiday", NameAr = "إضافة تاريخ اجازة", SortKey = DefaultSortKey, ParentId = 75, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 77, Key = PermissionKeysConstant.UserManagement.EDIT_CALENDAR_HOLIDAY, NameEn = "Edit Calendar Holiday", NameAr = "تعديل تاريخ اجازة", SortKey = 2, ParentId = 75, CreatedDate = SeedDate, UpdatedDate = SeedDate }
    });
  }

  private static void AddServiceOnePermissions(List<Permission> entities)
  {
    entities.AddRange(new[]
    {
        new Permission { Id = 78, Key = PermissionKeysConstant.ServiceOne.SERVICE_ONE, NameEn = "Service One", NameAr = "الخدمة الاولى", SortKey = 3, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 80, Key = PermissionKeysConstant.ServiceOne.ADD_SERVICE_ONE, NameEn = "Add Complaint or Request", NameAr = "إضافة شكوى او طلب", SortKey = 2, ParentId = 78, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 81, Key = PermissionKeysConstant.ServiceOne.EDIT_SERVICE_ONE, NameEn = "Edit Complaint or Request", NameAr = "تعديل شكوى او طلب", SortKey = 3, ParentId = 78, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 82, Key = PermissionKeysConstant.ServiceOne.CLOSE_SERVICE_ONE, NameEn = "Close Complaint or Request", NameAr = "إغلاق شكوى او طلب", SortKey = 4, ParentId = 78, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 84, Key = PermissionKeysConstant.ServiceOne.CHANGE_CATEGORY_SERVICE_ONE, NameEn = "Change Complaint or Request Category", NameAr = "تغيير فئة شكوى او طلب", SortKey = 6, ParentId = 78, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 85, Key = PermissionKeysConstant.ServiceOne.ADD_COMMENT_SERVICE_ONE, NameEn = "Add Comment to Complaint or Request", NameAr = "إضافة تعليق على شكوى او طلب", SortKey = 7, ParentId = 78, CreatedDate = SeedDate, UpdatedDate = SeedDate }
    });
  }

  private static void AddCustomerPermissions(List<Permission> entities)
  {
    entities.AddRange(new[]
    {
        new Permission { Id = 86, Key = PermissionKeysConstant.Customers.CUSTOMERS, NameEn = "Customers", NameAr = "العملاء", SortKey = 4, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 87, Key = PermissionKeysConstant.Customers.ADD_CUSTOMER, NameEn = "Add Customer", NameAr = "إضافة عميل", SortKey = DefaultSortKey, ParentId = 86, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 88, Key = PermissionKeysConstant.Customers.EDIT_CUSTOMER, NameEn = "Edit Customer", NameAr = "تعديل عميل", SortKey = 2, ParentId = 86, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 89, Key = PermissionKeysConstant.Customers.DELETE_CUSTOMER, NameEn = "Delete Customer", NameAr = "حذف عميل", SortKey = 3, ParentId = 86, CreatedDate = SeedDate, UpdatedDate = SeedDate }
    });
  }

  private static void AddAuditPermissions(List<Permission> entities)
  {
    entities.AddRange(new[]
    {
        new Permission { Id = 100, Key = PermissionKeysConstant.Audits.AUDITS, NameEn = "Audit Trails", NameAr = "سجلات التدقيق", SortKey = 10, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 101, Key = PermissionKeysConstant.Audits.VIEW_AUDITS, NameEn = "View Audit Trails", NameAr = "عرض سجلات التدقيق", SortKey = 1, ParentId = 100, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 102, Key = PermissionKeysConstant.Audits.EXPORT_AUDITS, NameEn = "Export Audit Trails to Excel", NameAr = "تصدير سجلات التدقيق إلى Excel", SortKey = 2, ParentId = 100, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 103, Key = PermissionKeysConstant.Audits.IMPORT_AUDITS, NameEn = "Import External Audit Trails", NameAr = "استيراد سجلات التدقيق الخارجية", SortKey = 3, ParentId = 100, CreatedDate = SeedDate, UpdatedDate = SeedDate }
    });
  }

  private static void AddDepartmentPermissions(List<Permission> entities)
  {
    entities.AddRange(new[]
    {
        new Permission { Id = 110, Key = PermissionKeysConstant.Departments.DEPARTMENTS, NameEn = "Departments", NameAr = "الإدارات", SortKey = 11, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 111, Key = PermissionKeysConstant.Departments.VIEW_DEPARTMENT, NameEn = "View Department", NameAr = "عرض إدارة", SortKey = 1, ParentId = 110, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 112, Key = PermissionKeysConstant.Departments.LIST_DEPARTMENTS, NameEn = "List Departments", NameAr = "قائمة الإدارات", SortKey = 2, ParentId = 110, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 113, Key = PermissionKeysConstant.Departments.ADD_DEPARTMENT, NameEn = "Add Department", NameAr = "إضافة إدارة", SortKey = 3, ParentId = 110, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 114, Key = PermissionKeysConstant.Departments.EDIT_DEPARTMENT, NameEn = "Edit Department", NameAr = "تعديل إدارة", SortKey = 4, ParentId = 110, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 115, Key = PermissionKeysConstant.Departments.DELETE_DEPARTMENT, NameEn = "Delete Department", NameAr = "حذف إدارة", SortKey = 5, ParentId = 110, CreatedDate = SeedDate, UpdatedDate = SeedDate }
    });
  }

  private static void AddEmailPermissions(List<Permission> entities)
  {
    entities.AddRange(new[]
    {
        new Permission { Id = 120, Key = PermissionKeysConstant.Emails.EMAILS, NameEn = "Emails", NameAr = "البريد الإلكتروني", SortKey = 12, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 121, Key = PermissionKeysConstant.Emails.VIEW_EMAIL, NameEn = "View Email", NameAr = "عرض البريد الإلكتروني", SortKey = 1, ParentId = 120, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 122, Key = PermissionKeysConstant.Emails.LIST_EMAILS, NameEn = "List Emails", NameAr = "قائمة البريد الإلكتروني", SortKey = 2, ParentId = 120, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 123, Key = PermissionKeysConstant.Emails.SEND_EMAIL, NameEn = "Send Email", NameAr = "إرسال البريد الإلكتروني", SortKey = 3, ParentId = 120, CreatedDate = SeedDate, UpdatedDate = SeedDate }
    });
  }

  private static void AddMediaPermissions(List<Permission> entities)
  {
    entities.AddRange(new[]
    {
        new Permission { Id = 130, Key = PermissionKeysConstant.Media.MEDIA, NameEn = "Media", NameAr = "الوسائط", SortKey = 13, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 131, Key = PermissionKeysConstant.Media.VIEW_MEDIA, NameEn = "View Media", NameAr = "عرض الوسائط", SortKey = 1, ParentId = 130, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 132, Key = PermissionKeysConstant.Media.LIST_MEDIA, NameEn = "List Media", NameAr = "قائمة الوسائط", SortKey = 2, ParentId = 130, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 133, Key = PermissionKeysConstant.Media.ADD_MEDIA, NameEn = "Add Media", NameAr = "إضافة الوسائط", SortKey = 3, ParentId = 130, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 134, Key = PermissionKeysConstant.Media.ADD_MEDIA_RANGE, NameEn = "Add Media Range", NameAr = "إضافة مجموعة وسائط", SortKey = 4, ParentId = 130, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 135, Key = PermissionKeysConstant.Media.DELETE_MEDIA, NameEn = "Delete Media", NameAr = "حذف الوسائط", SortKey = 5, ParentId = 130, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 136, Key = PermissionKeysConstant.Media.DOWNLOAD_MEDIA, NameEn = "Download Media", NameAr = "تحميل الوسائط", SortKey = 6, ParentId = 130, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 137, Key = PermissionKeysConstant.Media.SUBMIT_FORM_MEDIA, NameEn = "Submit Form with Media", NameAr = "إرسال نموذج مع وسائط", SortKey = 7, ParentId = 130, CreatedDate = SeedDate, UpdatedDate = SeedDate }
    });
  }

  private static void AddPdfPermissions(List<Permission> entities)
  {
    entities.AddRange(new[]
    {
        new Permission { Id = 140, Key = PermissionKeysConstant.Pdf.PDF, NameEn = "PDF Files", NameAr = "ملفات PDF", SortKey = 14, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 141, Key = PermissionKeysConstant.Pdf.EXPORT_PDF, NameEn = "Export User Details to PDF", NameAr = "تصدير تفاصيل المستخدم إلى PDF", SortKey = 1, ParentId = 140, CreatedDate = SeedDate, UpdatedDate = SeedDate }
    });
  }

  private static void AddPermissionManagementPermissions(List<Permission> entities)
  {
    entities.AddRange(new[]
    {
        new Permission { Id = 150, Key = PermissionKeysConstant.PermissionManagement.PERMISSION_MANAGEMENT, NameEn = "Permission Management", NameAr = "إدارة الأذونات", SortKey = 15, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 151, Key = PermissionKeysConstant.PermissionManagement.VIEW_PERMISSION, NameEn = "View Permission", NameAr = "عرض إذن", SortKey = 1, ParentId = 150, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 152, Key = PermissionKeysConstant.PermissionManagement.LIST_PERMISSIONS, NameEn = "List Permissions", NameAr = "قائمة الأذونات", SortKey = 2, ParentId = 150, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 153, Key = PermissionKeysConstant.PermissionManagement.ADD_PERMISSION, NameEn = "Add Permission", NameAr = "إضافة إذن", SortKey = 3, ParentId = 150, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 154, Key = PermissionKeysConstant.PermissionManagement.EDIT_PERMISSION, NameEn = "Edit Permission", NameAr = "تعديل إذن", SortKey = 4, ParentId = 150, CreatedDate = SeedDate, UpdatedDate = SeedDate },
        new Permission { Id = 155, Key = PermissionKeysConstant.PermissionManagement.DELETE_PERMISSION, NameEn = "Delete Permission", NameAr = "حذف إذن", SortKey = 5, ParentId = 150, CreatedDate = SeedDate, UpdatedDate = SeedDate }
    });
  }

}
