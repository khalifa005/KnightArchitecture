using KH.BuildingBlocks.Apis.Enums;

namespace KH.PersistenceInfra.Data.Seed;

public class UsersContextSeeder
{
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
        Password = "AQAAAAIAAYagAAAAEIAW+6BlJBTvMDShf7A43Qti+mTql7IHvCJx4zLDkz1FONLS6rE9m85uLGfT1jd9Jg==",
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

      var superAdmin = new User
      {
        Id = 1,
        FirstName = "Super",
        MiddleName = "_",
        LastName = "Admin",
        MobileNumber = "0566255570",
        Password = "AQAAAAIAAYagAAAAEIAW+6BlJBTvMDShf7A43Qti+mTql7IHvCJx4zLDkz1FONLS6rE9m85uLGfT1jd9Jg==",
        BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
        Username = "super-admin",
        IsDeleted = false,
        Email = "superadmin@gmail.com",
      };


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
      entities.Add(superAdmin);

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
}
