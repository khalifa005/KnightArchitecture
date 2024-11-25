using KH.BuildingBlocks.Apis.Enums;

namespace KH.PersistenceInfra.Data.Seed;

public class UsersContextSeeder
{
  public static void SeedUser(ModelBuilder builder, ILoggerFactory loggerFactory)
  {
    try
    {

      #region User
      List<User> entities = new List<User>();

      var superAdmin = new User
      {
        Id = 1,
        FirstName = "Super",
        MiddleName = "_",
        LastName = "Admin",
        MobileNumber = "0500000000",
        //password = KhalifaPassword
        Password = "AQAAAAIAAYagAAAAEIAW+6BlJBTvMDShf7A43Qti+mTql7IHvCJx4zLDkz1FONLS6rE9m85uLGfT1jd9Jg==",
        BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
        Username = "super_admin",
        IsDeleted = false,
        Email = "superadmin@gmail.com",
      };

      entities.Add(superAdmin);

      //var ceo = new User
      //{
      //  Id = 2,
      //  FirstName = "CEO",
      //  MiddleName = "_",
      //  LastName = "Khalifa",
      //  MobileNumber = "0500000001",
      //  //password = KhalifaPassword
      //  Password = "AQAAAAIAAYagAAAAEIAW+6BlJBTvMDShf7A43Qti+mTql7IHvCJx4zLDkz1FONLS6rE9m85uLGfT1jd9Jg==",
      //  BirthDate = DateTime.Parse("2023-06-18 12:37:20.9340518"),
      //  Username = "khalifa_CEO",
      //  IsDeleted = false,
      //  Email = "khalifa_CEO@example.com",
      //};

      //entities.Add(ceo);

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

      //UserRoles.Add(new UserRole
      //{
      //  Id = 2,
      //  RoleId = (int)RoleEnum.CEO,
      //  UserId = 2,
      //  IsDeleted = false,
      //});

      builder.Entity<UserRole>().HasData(UserRoles);

      #endregion

      List<UserGroup> UserGroups = new List<UserGroup>();

      UserGroups.Add(new UserGroup
      {
        Id = 1,
        GroupId = (int)GroupEnum.Managers,
        UserId = 1,
        IsDeleted = false,
      });

      builder.Entity<UserGroup>().HasData(UserGroups);
    }
    catch (Exception ex)
    {
      var logger = loggerFactory.CreateLogger<LookupContextSeeder>();
      logger.LogError(ex.Message);
    }
  }
}
