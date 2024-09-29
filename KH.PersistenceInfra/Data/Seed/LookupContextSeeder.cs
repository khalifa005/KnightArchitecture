using KH.BuildingBlocks.Enums;
using Role = KH.Domain.Entities.Role;

namespace KH.PersistenceInfra.Data.Seed
{
  public class LookupContextSeeder
  {


    public static void SeedRoles(ModelBuilder builder, ILoggerFactory loggerFactory)
    {
      try
      {
        List<Role> entities = new List<Role>();

        entities.Add(new Role { Id = 1, NameEn = "Super admin", NameAr = "مدير النظام الكلي" });
        entities.Add(new Role { Id = 2, NameEn = "CEO", NameAr = "مدير المؤسسة" });
        entities.Add(new Role { Id = 3, NameEn = "Vice President", NameAr = "نائب مدير المؤسسة", ReportToRoleId = 2 });
        entities.Add(new Role { Id = 4, NameEn = "Dept. Manager", NameAr = "مدير القسم", ReportToRoleId = 3 });
        entities.Add(new Role { Id = 5, NameEn = "Unit Head", NameAr = "رئيس وحدة", ReportToRoleId = 4 });
        entities.Add(new Role { Id = 6, NameEn = "Supervisor", NameAr = "مشرف", ReportToRoleId = 5 });
        entities.Add(new Role { Id = 7, NameEn = "CPA", NameAr = "الموظف", ReportToRoleId = 6 });
        entities.Add(new Role { Id = 8, NameEn = "Customer Service Manager", NameAr = "مدير خدمة العملاء" });
        entities.Add(new Role { Id = 9, NameEn = "Customer Service Supervisor", NameAr = "مشرف خدمة العملاء", ReportToRoleId = 8 });
        entities.Add(new Role { Id = 10, NameEn = "Agent user", NameAr = "وكيل عملاء", ReportToRoleId = 9 });

        builder.Entity<Role>().HasData(entities);

      }
      catch (Exception ex)
      {
        var logger = loggerFactory.CreateLogger<LookupContextSeeder>();
        logger.LogError(ex.Message);
      }
    }

    public static void SeedDepartment(ModelBuilder builder, ILoggerFactory loggerFactory)
    {
      try
      {
        List<Department> entities = new List<Department>();

        entities.Add(new Department { Id = 1, NameEn = "Motor Claim / Providers & Salvage", NameAr = "Motor Claim / Providers & Salvage" });
        entities.Add(new Department { Id = 2, NameEn = "Call Center", NameAr = "خدمة عملاء" });
        entities.Add(new Department { Id = 3, NameEn = "Claim", NameAr = "مطالبات" });
        entities.Add(new Department { Id = 4, NameEn = "Medical", NameAr = "الطبي" });
        entities.Add(new Department { Id = 5, NameEn = "It", NameAr = "التقني" });
        entities.Add(new Department { Id = 6, NameEn = "Finance", NameAr = "المالية" });

        builder.Entity<Department>().HasData(entities);

      }
      catch (Exception ex)
      {
        var logger = loggerFactory.CreateLogger<LookupContextSeeder>();
        logger.LogError(ex.Message);
      }
    }

    public static void SeedGroups(ModelBuilder builder, ILoggerFactory loggerFactory)
    {
      try
      {
        List<Group> entities = new List<Group>();

        entities.Add(new Group { Id = 1, NameEn = " Request Group", NameAr = "الطلبات الخارجيه", TicketCategoryId = (int)TicketCategoryEnum.Request });
        entities.Add(new Group { Id = 2, NameEn = "Team Selling", NameAr = "المبيعات", TicketCategoryId = (int)TicketCategoryEnum.Request });

        builder.Entity<Group>().HasData(entities);

      }
      catch (Exception ex)
      {
        var logger = loggerFactory.CreateLogger<LookupContextSeeder>();
        logger.LogError(ex.Message);
      }
    }



    public static void SeedCities(ModelBuilder builder, ILoggerFactory loggerFactory)
    {
      //need to move the rest of data
      try
      {
        List<City> entities = new List<City>();


        entities.Add(new City { Id = 1, NameAr = "جدة", NameEn = "Jeddah" });
        entities.Add(new City { Id = 2, NameAr = "الرياض", NameEn = "Riyadh" });
        entities.Add(new City { Id = 3, NameAr = "الدمام", NameEn = "Damam" });

        //entities.Add(new TicketCategory { NameAr = "", NameEn = "" });

        builder.Entity<City>().HasData(entities);

      }
      catch (Exception ex)
      {
        var logger = loggerFactory.CreateLogger<LookupContextSeeder>();
        logger.LogError(ex.Message);
      }
    }


    public static void SeedCustomer(ModelBuilder builder, ILoggerFactory loggerFactory)
    {
      try
      {

        List<Customer> entities = new List<Customer>();
        //MotorTPL  Admin@123

        entities.Add(new Customer
        {
          Id = 1,
          FirstName = "General",
          MiddleName = "Customer",
          LastName = "for Admins",
          MobileNumber = "966535701842",
          IDNumber = "2543233566",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          IsOTPVerified = true,
          OTPCode = "",
          IsSelfRegistered = true,
          Username = "admin-customer",
          IsDeleted = false,
          Email = "admin123@gmail.com",
          Password = "AQAAAAIAAYagAAAAEDfjVdHaKiIjPGgo40R5sw5yz0euQkT0rPW0ClWdYJLxgXMou/Zw07GWc65b68IJOA=="
        });


        int counter = 10;
        while (counter < 2500)
        {
          entities.Add(new Customer
          {
            Id = counter,
            FirstName = "customer",
            MiddleName = "number",
            LastName = "-" + counter,
            MobileNumber = "05246554" + counter,
            IDNumber = "",
            BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
            IsOTPVerified = true,
            OTPCode = "",
            IsSelfRegistered = true,
            Username = "customer" + counter,
            IsDeleted = false,
            Email = "admin123@gmail.com",
            Password = "AQAAAAIAAYagAAAAEDfjVdHaKiIjPGgo40R5sw5yz0euQkT0rPW0ClWdYJLxgXMou/Zw07GWc65b68IJOA=="
          });

          counter++;
        }

        builder.Entity<Customer>().HasData(entities);

      }
      catch (Exception ex)
      {
        var logger = loggerFactory.CreateLogger<LookupContextSeeder>();
        logger.LogError(ex.Message);
      }
    }

    public static void SeedUser(ModelBuilder builder, ILoggerFactory loggerFactory)
    {
      try
      {

        #region User
        List<User> entities = new List<User>();
        //MotorTPL 

        var CEOforMotor = new User
        {
          Id = 2,
          FirstName = "CEO",
          MiddleName = "For ",
          LastName = "Motors",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "CEO",
          IsDeleted = false,
          Email = "CEO@gmail.com",
        };

        var CEOforMedical = new User
        {
          Id = 3,
          FirstName = "CEO",
          MiddleName = "For ",
          LastName = "Medical",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "CEO",
          IsDeleted = false,
          Email = "CEOMedical@gmail.com",
        };

        var VicePresidentforMotor = new User
        {
          Id = 4,
          FirstName = "VicePresident",
          MiddleName = "For ",
          LastName = "Motors",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "VicePresident",
          IsDeleted = false,
          Email = "CEO@gmail.com",
        };

        var VicePresidentforMedical = new User
        {
          Id = 5,
          FirstName = "VicePresident",
          MiddleName = "For ",
          LastName = "Medical",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "MVicePresident",
          IsDeleted = false,
          Email = "CEOMedical@gmail.com",
        };

        var DeptManagerforMotor = new User
        {
          Id = 6,
          FirstName = "DeptManager",
          MiddleName = "For ",
          LastName = "Motors",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "DeptManager",
          IsDeleted = false,
          Email = "DeptManager@gmail.com",
        };

        var DeptManagerforMedical = new User
        {
          Id = 7,
          FirstName = "DeptManager",
          MiddleName = "For ",
          LastName = "Medical",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "DeptManagerM",
          IsDeleted = false,
          Email = "DeptManagerMedical@gmail.com",
        };

        var UnitHeadforMotor = new User
        {
          Id = 8,
          FirstName = "UnitHead",
          MiddleName = "For ",
          LastName = "Motors",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "UnitHead",
          IsDeleted = false,
          Email = "UnitHead@gmail.com",
        };

        var UnitHeadforMedical = new User
        {
          Id = 9,
          FirstName = "UnitHead",
          MiddleName = "For ",
          LastName = "Medical",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "UnitHeadM",
          IsDeleted = false,
          Email = "UnitHeadMedical@gmail.com",
        };

        var SupervisorforMotor = new User
        {
          Id = 10,
          FirstName = "Supervisor",
          MiddleName = "For ",
          LastName = "Motors",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "Supervisor",
          IsDeleted = false,
          Email = "Supervisor@gmail.com",
        };

        var SupervisorforMedical = new User
        {
          Id = 11,
          FirstName = "Supervisor",
          MiddleName = "For ",
          LastName = "Medical",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "SupervisorM",
          IsDeleted = false,
          Email = "SupervisorMedical@gmail.com",
        };

        var CPAforMotor = new User
        {
          Id = 12,
          FirstName = "CPA",
          MiddleName = "For ",
          LastName = "Motors",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "CPA",
          IsDeleted = false,
          Email = "CPA@gmail.com",
        };

        var CPAforMedical = new User
        {
          Id = 13,
          FirstName = "CPA",
          MiddleName = "For ",
          LastName = "Medical",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "CPAM",
          IsDeleted = false,
          Email = "CPAMedical@gmail.com",
        };

        entities.Add(new User
        {
          Id = 1,
          FirstName = "super",
          MiddleName = "admin",
          LastName = "for Admins",
          MobileNumber = "0535701842",
          BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
          Username = "admin",
          IsDeleted = false,
          Email = "superadmin@gmail.com",
        });

        entities.Add(CEOforMotor);
        entities.Add(CEOforMedical);
        entities.Add(VicePresidentforMotor);
        entities.Add(VicePresidentforMedical);
        entities.Add(DeptManagerforMotor);
        entities.Add(DeptManagerforMedical);
        entities.Add(UnitHeadforMotor);
        entities.Add(UnitHeadforMedical);
        entities.Add(SupervisorforMotor);
        entities.Add(SupervisorforMedical);
        entities.Add(CPAforMotor);
        entities.Add(CPAforMedical);

        builder.Entity<User>().HasData(entities);

        #endregion

        #region UserRoles
        List<UserRole> UserRoles = new List<UserRole>();

        UserRoles.Add(new UserRole
        {
          Id = 1,
          RoleId = (int)RoleEnum.SuperAdmin,
          UserId = 1,
          IsDeleted = false,
        });

        UserRoles.Add(new UserRole
        {
          Id = 2,
          RoleId = (int)RoleEnum.CEO,
          UserId = 2,
          IsDeleted = false,
        });

        UserRoles.Add(new UserRole
        {
          Id = 3,
          RoleId = (int)RoleEnum.CEO,
          UserId = 3,
          IsDeleted = false,
        });


        UserRoles.Add(new UserRole
        {
          Id = 4,
          RoleId = (int)RoleEnum.VicePresident,
          UserId = 4,
          IsDeleted = false,
        });

        UserRoles.Add(new UserRole
        {
          Id = 5,
          RoleId = (int)RoleEnum.VicePresident,
          UserId = 5,
          IsDeleted = false,
        });

        UserRoles.Add(new UserRole
        {
          Id = 6,
          RoleId = (int)RoleEnum.DeptManager,
          UserId = 6,
          IsDeleted = false,
        });

        UserRoles.Add(new UserRole
        {
          Id = 7,
          RoleId = (int)RoleEnum.DeptManager,
          UserId = 7,
          IsDeleted = false,
        });

        UserRoles.Add(new UserRole
        {
          Id = 8,
          RoleId = (int)RoleEnum.UnitHead,
          UserId = 8,
          IsDeleted = false,
        });

        UserRoles.Add(new UserRole
        {
          Id = 9,
          RoleId = (int)RoleEnum.UnitHead,
          UserId = 9,
          IsDeleted = false,
        });

        UserRoles.Add(new UserRole
        {
          Id = 10,
          RoleId = (int)RoleEnum.Supervisor,
          UserId = 10,
          IsDeleted = false,
        });
        UserRoles.Add(new UserRole
        {
          Id = 11,
          RoleId = (int)RoleEnum.Supervisor,
          UserId = 11,
          IsDeleted = false,
        });


        UserRoles.Add(new UserRole
        {
          Id = 12,
          RoleId = (int)RoleEnum.CPA,
          UserId = 12,
          IsDeleted = false,
        });
        UserRoles.Add(new UserRole
        {
          Id = 13,
          RoleId = (int)RoleEnum.CPA,
          UserId = 13,
          IsDeleted = false,
        });


        builder.Entity<UserRole>().HasData(UserRoles);

        #endregion

        List<UserGroup> UserGroups = new List<UserGroup>();

        UserGroups.Add(new UserGroup
        {
          Id = 1,
          GroupId = (int)GroupEnum.ExternalRequest,
          UserId = 1,
          IsDeleted = false,
        });

        builder.Entity<UserGroup>().HasData(UserGroups);


        #region UserDepartment
        List<UserDepartment> UserDepartments = new List<UserDepartment>();

        UserDepartments.Add(new UserDepartment
        {
          Id = 1,
          DepartmentId = (int)DepartmentEnum.MotorClaim,
          UserId = 2,
          IsDeleted = false,
        });

        UserDepartments.Add(new UserDepartment
        {
          Id = 2,
          DepartmentId = (int)DepartmentEnum.Medical,
          UserId = 3,
          IsDeleted = false,
        });



        UserDepartments.Add(new UserDepartment
        {
          Id = 3,
          DepartmentId = (int)DepartmentEnum.MotorClaim,
          UserId = 4,
          IsDeleted = false,
        });

        UserDepartments.Add(new UserDepartment
        {
          Id = 4,
          DepartmentId = (int)DepartmentEnum.Medical,
          UserId = 5,
          IsDeleted = false,
        });

        UserDepartments.Add(new UserDepartment
        {
          Id = 5,
          DepartmentId = (int)DepartmentEnum.MotorClaim,
          UserId = 6,
          IsDeleted = false,
        });

        UserDepartments.Add(new UserDepartment
        {
          Id = 6,
          DepartmentId = (int)DepartmentEnum.Medical,
          UserId = 7,
          IsDeleted = false,
        });

        UserDepartments.Add(new UserDepartment
        {
          Id = 7,
          DepartmentId = (int)DepartmentEnum.MotorClaim,
          UserId = 8,
          IsDeleted = false,
        });

        UserDepartments.Add(new UserDepartment
        {
          Id = 8,
          DepartmentId = (int)DepartmentEnum.Medical,
          UserId = 9,
          IsDeleted = false,
        });

        UserDepartments.Add(new UserDepartment
        {
          Id = 9,
          DepartmentId = (int)DepartmentEnum.MotorClaim,
          UserId = 10,
          IsDeleted = false,
        });

        UserDepartments.Add(new UserDepartment
        {
          Id = 10,
          DepartmentId = (int)DepartmentEnum.Medical,
          UserId = 11,
          IsDeleted = false,
        });


        UserDepartments.Add(new UserDepartment
        {
          Id = 11,
          DepartmentId = (int)DepartmentEnum.MotorClaim,
          UserId = 12,
          IsDeleted = false,
        });

        UserDepartments.Add(new UserDepartment
        {
          Id = 12,
          DepartmentId = (int)DepartmentEnum.Medical,
          UserId = 13,
          IsDeleted = false,
        });
        builder.Entity<UserDepartment>().HasData(UserDepartments);
        #endregion

      }
      catch (Exception ex)
      {
        var logger = loggerFactory.CreateLogger<LookupContextSeeder>();
        logger.LogError(ex.Message);
      }
    }

    public static void SeedSystemFunction(ModelBuilder builder, ILoggerFactory loggerFactory)
    {
      try
      {
        List<SystemFunction> entities = new List<SystemFunction>();

        #region lookups

        entities.Add(new SystemFunction
        {
          Id = 1,
          NameEn = "lookups",
          NameAr = "البيانات الاساسية",
          SortKey = 1
        });


        #region categories
        entities.Add(new SystemFunction
        {
          Id = 2,
          NameEn = "categories",
          NameAr = "فئات الطلبات",
          SortKey = 1,
          ParentID = 1
        });

        entities.Add(new SystemFunction
        {
          Id = 3,
          NameEn = "add-category",
          NameAr = "إضافة فئة طلب",
          SortKey = 1,
          ParentID = 2
        });

        entities.Add(new SystemFunction
        {
          Id = 4,
          NameEn = "edit-category",
          NameAr = "تعديل فئة طلب",
          SortKey = 2,
          ParentID = 2
        });

        entities.Add(new SystemFunction
        {
          Id = 5,
          NameEn = "delete-category",
          NameAr = "حذف فئة طلب",
          SortKey = 3,
          ParentID = 2
        });
        #endregion

        #region status
        entities.Add(new SystemFunction
        {
          Id = 6,
          NameEn = "status",
          NameAr = "حالات الطلبات",
          SortKey = 2,
          ParentID = 1
        });

        entities.Add(new SystemFunction
        {
          Id = 8,
          NameEn = "edit-status",
          NameAr = "تعديل حالة طلب",
          SortKey = 2,
          ParentID = 6
        });

        entities.Add(new SystemFunction
        {
          Id = 9,
          NameEn = "delete-status",
          NameAr = "حذف حالة طلب",
          SortKey = 3,
          ParentID = 6
        });

        #endregion

        #region cities
        entities.Add(new SystemFunction
        {
          Id = 18,
          NameEn = "cities",
          NameAr = "المدن",
          SortKey = 5,
          ParentID = 1
        });

        entities.Add(new SystemFunction
        {
          Id = 19,
          NameEn = "add-city",
          NameAr = "إضافة مدينه",
          SortKey = 1,
          ParentID = 18
        });

        entities.Add(new SystemFunction
        {
          Id = 20,
          NameEn = "edit-city",
          NameAr = "تعديل مدينه",
          SortKey = 2,
          ParentID = 18
        });

        entities.Add(new SystemFunction
        {
          Id = 21,
          NameEn = "delete-city",
          NameAr = "حذف مدينه",
          SortKey = 3,
          ParentID = 18
        });

        #endregion

        #region groups
        entities.Add(new SystemFunction
        {
          Id = 22,
          NameEn = "groups",
          NameAr = "المجموعات",
          SortKey = 6,
          ParentID = 1
        });

        entities.Add(new SystemFunction
        {
          Id = 23,
          NameEn = "add-group",
          NameAr = "إضافة مجموعه",
          SortKey = 1,
          ParentID = 22
        });

        entities.Add(new SystemFunction
        {
          Id = 24,
          NameEn = "edit-group",
          NameAr = "تعديل مجموعه",
          SortKey = 2,
          ParentID = 22
        });

        entities.Add(new SystemFunction
        {
          Id = 25,
          NameEn = "delete-group",
          NameAr = "حذف مجموعه",
          SortKey = 3,
          ParentID = 22
        });
        #endregion

        #region roles
        entities.Add(item: new SystemFunction
        {
          Id = 30,
          NameEn = "roles",
          NameAr = "الأدوار",
          SortKey = 8,
          ParentID = 1
        });


        entities.Add(new SystemFunction
        {
          Id = 31,
          NameEn = "edit-role",
          NameAr = "تعديل دور مستخدم",
          SortKey = 1,
          ParentID = 30
        });

        entities.Add(new SystemFunction
        {
          Id = 32,
          NameEn = "delete-role",
          NameAr = "حذف دور مستخدم",
          SortKey = 2,
          ParentID = 30
        });
        #endregion

        #region departments
        entities.Add(new SystemFunction
        {
          Id = 33,
          NameEn = "departments",
          NameAr = "الإدارات والأقسام",
          SortKey = 9,
          ParentID = 1
        });

        entities.Add(new SystemFunction
        {
          Id = 34,
          NameEn = "add-department",
          NameAr = "إضافة قسم",
          SortKey = 1,
          ParentID = 33
        });

        entities.Add(new SystemFunction
        {
          Id = 35,
          NameEn = "edit-department",
          NameAr = "تعديل قسم",
          SortKey = 2,
          ParentID = 33
        });

        entities.Add(new SystemFunction
        {
          Id = 36,
          NameEn = "delete-department",
          NameAr = "حذف قسم",
          SortKey = 3,
          ParentID = 33
        });

        #endregion

        #endregion

        #region user-management

        entities.Add(new SystemFunction
        {
          Id = 69,
          NameEn = "user-management",
          NameAr = "الإدارة",
          SortKey = 2
        });

        #region Role management
        entities.Add(new SystemFunction
        {
          Id = 70,
          NameEn = "roles-management",
          NameAr = "إدارة أدوار المستخدمين",
          SortKey = 1,
          ParentID = 69
        });
        #endregion

        #region Users
        entities.Add(new SystemFunction
        {
          Id = 71,
          NameEn = "users",
          NameAr = "المستخدمين",
          SortKey = 2,
          ParentID = 69
        });

        entities.Add(new SystemFunction
        {
          Id = 72,
          NameEn = "add-user",
          NameAr = " إضافة مستخدم",
          SortKey = 1,
          ParentID = 71
        });

        entities.Add(new SystemFunction
        {
          Id = 73,
          NameEn = "edit-user",
          NameAr = "تعديل مستخدم ",
          SortKey = 2,
          ParentID = 71
        });

        entities.Add(new SystemFunction
        {
          Id = 74,
          NameEn = "delete-user",
          NameAr = "حذف مستخدم ",
          SortKey = 3,
          ParentID = 71
        });

        #endregion

        #region calender
        entities.Add(new SystemFunction
        {
          Id = 75,
          NameEn = "calender",
          NameAr = "التورايخ",
          SortKey = 3,
          ParentID = 69
        });


        entities.Add(new SystemFunction
        {
          Id = 76,
          NameEn = "add-calender-holiday",
          NameAr = " إضافة تاريخ اجازة",
          SortKey = 1,
          ParentID = 75
        });

        entities.Add(new SystemFunction
        {
          Id = 77,
          NameEn = "edit-calender-holiday",
          NameAr = "تعديل تاريخ اجازة ",
          SortKey = 2,
          ParentID = 75
        });

        #endregion

        #endregion

        #region ServiceOne
        entities.Add(new SystemFunction
        {
          Id = 78,
          NameEn = "servic-one",
          NameAr = "الخدمة الاولى",
          SortKey = 3,
        });



        entities.Add(new SystemFunction
        {
          Id = 80,
          NameEn = "add-service-one",
          NameAr = "إضافة شكوى او طلب",
          SortKey = 2,
          ParentID = 78
        });

        entities.Add(new SystemFunction
        {
          Id = 81,
          NameEn = "edit-service-one",
          NameAr = "تعديل شكوى او طلب",
          SortKey = 3,
          ParentID = 78
        });



        entities.Add(new SystemFunction
        {
          Id = 82,
          NameEn = "close-service-one",
          NameAr = "تعديل شكوى او طلب",
          SortKey = 4,
          ParentID = 78
        });




        entities.Add(new SystemFunction
        {
          Id = 84,
          NameEn = "change-category-service-one",
          NameAr = "تغيير فئة شكوى او طلب",
          SortKey = 6,
          ParentID = 78
        });

        entities.Add(new SystemFunction
        {
          Id = 85,
          NameEn = "add-comment-service-one",
          NameAr = "إضافة تعليق على شكوى او طلب",
          SortKey = 7,
          ParentID = 78
        });


        #endregion

        #region Customers
        entities.Add(new SystemFunction
        {
          Id = 86,
          NameEn = "Customers",
          NameAr = "العملاء",
          SortKey = 4,
        });


        entities.Add(new SystemFunction
        {
          Id = 87,
          NameEn = "add-Customer",
          NameAr = "إضافة عميل",
          SortKey = 1,
          ParentID = 86
        });

        entities.Add(new SystemFunction
        {
          Id = 88,
          NameEn = "edit-Customer",
          NameAr = "تعديل عميل",
          SortKey = 2,
          ParentID = 86
        });

        entities.Add(new SystemFunction
        {
          Id = 89,
          NameEn = "delete-Customer",
          NameAr = "حذف عميل",
          SortKey = 3,
          ParentID = 86
        });
        #endregion

        builder.Entity<SystemFunction>().HasData(entities);

      }
      catch (Exception ex)
      {
        var logger = loggerFactory.CreateLogger<LookupContextSeeder>();
        logger.LogError(ex.Message);
      }
    }


  }
}
