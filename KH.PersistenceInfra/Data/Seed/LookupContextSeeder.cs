using KH.BuildingBlocks.Enums;
using Role = KH.Domain.Entities.Role;

namespace KH.PersistenceInfra.Data.Seed;

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
}
