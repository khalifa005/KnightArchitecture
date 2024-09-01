
using KH.Domain.Commons;
using KH.Helper.Extentions.Methods;
using KH.PersistenceInfra.Data.Seed;

namespace KH.PersistenceInfra.Data
{
  public class AppDbContext : DbContext
  {
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<AppDbContext> _logger;
    private readonly IServiceProvider _serviceProvider;

    public AppDbContext(
      DbContextOptions<AppDbContext> options,
      ILogger<AppDbContext> logger,
      IServiceProvider serviceProvider,
      ILoggerFactory loggerFactory) : base(options)
    {
      _loggerFactory = loggerFactory;
      _serviceProvider = serviceProvider;
      _logger = logger;
    }


    public DbSet<Media> Media { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<KH.Domain.Entities.Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<SystemFunction> SystemFunctions { get; set; }
    public DbSet<RoleFunction> RoleFunctions { get; set; }
    public DbSet<SMSFollowUp> SMSFollowUp { get; set; }
    public DbSet<Calendar> Calendar { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
      {
        fk.DeleteBehavior = DeleteBehavior.Restrict;
      }

      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

      //LookupContextSeeder.SeedRoles(modelBuilder, _loggerFactory);
      LookupContextSeeder.SeedCities(modelBuilder, _loggerFactory);
      LookupContextSeeder.SeedDepartment(modelBuilder, _loggerFactory);
      LookupContextSeeder.SeedGroups(modelBuilder, _loggerFactory);
      LookupContextSeeder.SeedCustomer(modelBuilder, _loggerFactory);
      LookupContextSeeder.SeedRoles(modelBuilder, _loggerFactory);
      LookupContextSeeder.SeedUser(modelBuilder, _loggerFactory);
      LookupContextSeeder.SeedSystemFunction(modelBuilder, _loggerFactory);

      //-- Add Type To Context Model but exclude from migration
      //modelBuilder.Entity<EscalationMatrixDto>().HasNoKey().ToTable((string?)null);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      foreach (var entry in ChangeTracker.Entries<TrackerEntity>())
      {
        switch (entry.State)
        {
          case EntityState.Detached:
            break;
          case EntityState.Unchanged:
            break;
          case EntityState.Deleted:

            if (entry.Entity.GetType() == typeof(RoleFunction) || entry.Entity.GetType() == typeof(UserRole) || entry.Entity.GetType() == typeof(UserGroup) || entry.Entity.GetType() == typeof(UserDepartment))
              break;

            entry.State = EntityState.Modified;
            entry.Entity.DeletedDate = DateTime.UtcNow.AddHours(3);//ksa
            entry.Entity.DeletedById = _serviceProvider.GetUserId();//test ??
            entry.Entity.IsDeleted = true;
            break;
          case EntityState.Modified:
            entry.Entity.UpdatedDate = DateTime.UtcNow.AddHours(3);//ksa;
            entry.Entity.UpdatedById = _serviceProvider.GetUserId();//test ??
            entry.Entity.IsDeleted = false;
            break;
          case EntityState.Added:
            entry.Entity.CreatedDate = DateTime.UtcNow.AddHours(3);//ksa
            entry.Entity.CreatedById = _serviceProvider.GetUserId();//test ??
            entry.Entity.IsDeleted = false;
            break;
          default:
            break;
        }
      }

      return base.SaveChangesAsync(cancellationToken);
    }
  }
}
