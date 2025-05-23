using KH.BuildingBlocks.Apis.Constant;
using KH.BuildingBlocks.Apis.Entities;
using KH.BuildingBlocks.Apis.Enums;
using KH.BuildingBlocks.Auth.Contracts;
using KH.BuildingBlocks.Auth.User;
using KH.BuildingBlocks.Common.Attributes;
using KH.PersistenceInfra.Data.Seed;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace KH.PersistenceInfra.Data;

public class AppDbContext : DbContext
{
  private readonly ILoggerFactory _loggerFactory;
  private readonly ILogger<AppDbContext> _logger;
  private readonly IServiceProvider _serviceProvider;
  private readonly ICurrentUserService _currentUserService;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public AppDbContext(
      DbContextOptions<AppDbContext> options,
      ICurrentUserService currentUserService,
      ILogger<AppDbContext> logger,
      IServiceProvider serviceProvider,
      ILoggerFactory loggerFactory,
      IHttpContextAccessor httpContextAccessor) : base(options)
  {
    _loggerFactory = loggerFactory;
    _currentUserService = currentUserService;
    _serviceProvider = serviceProvider;
    _logger = logger;
    _httpContextAccessor = httpContextAccessor; // Store IHttpContextAccessor

  }

  public DbSet<Audit> AuditTrails { get; set; }
  public DbSet<SmsTemplate> SmsTemplates { get; set; }
  public DbSet<SmsTracker> SmsTracker { get; set; }
  public DbSet<Media> Media { get; set; }
  public DbSet<Customer> Customers { get; set; }
  public DbSet<City> Cities { get; set; }
  public DbSet<Department> Departments { get; set; }
  public DbSet<Group> Groups { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<Role> Roles { get; set; }
  public DbSet<UserRole> UserRoles { get; set; }
  public DbSet<UserGroup> UserGroups { get; set; }
  public DbSet<Permission> Permissions { get; set; }
  public DbSet<RolePermissions> RolePermissions { get; set; }
  public DbSet<Calendar> Calendar { get; set; }
  public DbSet<EmailTracker> EmailTracker { get; set; }
  public DbSet<Event> Events { get; set; }
  public DbSet<Subscription> Subscriptions { get; set; }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    //foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
    //{
    //  fk.DeleteBehavior = DeleteBehavior.Restrict;
    //}

    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    LookupContextSeeder.SeedCities(modelBuilder, _loggerFactory);
    LookupContextSeeder.SeedDepartment(modelBuilder, _loggerFactory);
    LookupContextSeeder.SeedGroups(modelBuilder, _loggerFactory);
    LookupContextSeeder.SeedRoles(modelBuilder, _loggerFactory);
    PermissionsContextSeeder.SeedSystemPermissions(modelBuilder, _loggerFactory);
    PermissionsContextSeeder.SeedCEORolePermissions(modelBuilder, _loggerFactory);
    PermissionsContextSeeder.SeedSuperAdminPermissions(modelBuilder, _loggerFactory);
    SmsTemplateContextSeeder.SeedTemplates(modelBuilder, _loggerFactory);
    UsersContextSeeder.SeedUser(modelBuilder, _loggerFactory);

  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    var userId = _currentUserService.UserId;

    // Handle the state of TrackerEntity types
    foreach (var entry in ChangeTracker.Entries<TrackerEntity>())
    {
      switch (entry.State)
      {
        case EntityState.Detached:
          break;
        case EntityState.Unchanged:
          break;
        case EntityState.Deleted:
          if (entry.Entity.GetType() == typeof(RolePermissions) || entry.Entity.GetType() == typeof(UserRole) ||
              entry.Entity.GetType() == typeof(UserGroup) || entry.Entity.GetType() == typeof(UserDepartment))
            break;

          entry.State = EntityState.Modified;
          entry.Entity.DeletedDate = DateTime.UtcNow.AddHours(3); // KSA Time
          entry.Entity.DeletedById = _serviceProvider.GetUserId();
          entry.Entity.IsDeleted = true;
          break;

        case EntityState.Modified:
          entry.Entity.UpdatedDate = DateTime.UtcNow.AddHours(3); // KSA Time
          entry.Entity.UpdatedById = _serviceProvider.GetUserId();
          break;

        case EntityState.Added:
          entry.Entity.CreatedDate = DateTime.UtcNow.AddHours(3); // KSA Time
          entry.Entity.CreatedById = _serviceProvider.GetUserId();
          entry.Entity.IsDeleted = false;
          break;

        default:
          break;
      }
    }

    // Step 1: Call OnBeforeSaveChanges to gather audit entries
    var auditEntries = OnBeforeSaveChanges(_serviceProvider.GetUserId().ToString());

    // Step 2: Save changes to the database
    var result = await base.SaveChangesAsync(cancellationToken);

    // Step 3: Process the audit entries with temporary properties after save
    await OnAfterSaveChanges(auditEntries, cancellationToken);

    return result;
  }

  // OnBeforeSaveChanges - Gathers audit entries for entity changes
  private List<AuditEntry> OnBeforeSaveChanges(string userId)
  {
    ChangeTracker.DetectChanges();
    var auditEntries = new List<AuditEntry>();

    // Retrieve the TraceIdentifier from the current HttpContext this will keep the serilogs = auditing correlationId
    //to track the request life cycle starting  from request logs by serilog and auditing actions made by the user
    var requestId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? "internal-process";

    foreach (var entry in ChangeTracker.Entries())
    {
      // Skip audit for certain entities or unchanged/detached entities Skip entities with [NoAudit] attribute
      var entityType = entry.Entity.GetType();
      if (entityType.GetCustomAttribute<NoAuditAttribute>() != null
        || entry.Entity is Audit
        || entry.State == EntityState.Detached
        || entry.State == EntityState.Unchanged)
        continue;

      var auditEntry = new AuditEntry(entry)
      {
        TableName = entry.Entity.GetType().Name,
        UserId = userId,
        RequestId = requestId
      };

      auditEntries.Add(auditEntry);

      foreach (var property in entry.Properties)
      {
        if (property.IsTemporary)
        {
          auditEntry.TemporaryProperties.Add(property);
          continue;
        }

        string propertyName = property.Metadata.Name;

        if (property.Metadata.IsPrimaryKey())
        {
          auditEntry.KeyValues[propertyName] = property.CurrentValue;
          continue;
        }

        switch (entry.State)
        {
          case EntityState.Added:
            auditEntry.AuditType = AuditType.Create;
            auditEntry.NewValues[propertyName] = property.CurrentValue;
            break;

          case EntityState.Deleted:
            auditEntry.AuditType = AuditType.Delete;
            auditEntry.OldValues[propertyName] = property.OriginalValue;
            break;

          case EntityState.Modified:
            if (property.IsModified && !Equals(property.OriginalValue, property.CurrentValue))
            {
              auditEntry.ChangedColumns.Add(propertyName); // Track modified columns
              auditEntry.AuditType = AuditType.Update;
              auditEntry.OldValues[propertyName] = property.OriginalValue;
              auditEntry.NewValues[propertyName] = property.CurrentValue;
            }
            break;
        }
      }

      // Ensure that AffectedColumns are not empty for modified entities
      if (auditEntry.AuditType == AuditType.Update && !auditEntry.ChangedColumns.Any())
      {
        auditEntry.ChangedColumns.Add("No changes");
      }
    }

    foreach (var auditEntry in auditEntries.Where(ae => !ae.HasTemporaryProperties))
    {
      //var mappedAuditEntry = auditEntry.ToAudit();
      //mappedAuditEntry.CorrelationId = correlationId;

      AuditTrails.Add(auditEntry.ToAudit());
    }

    return auditEntries.Where(ae => ae.HasTemporaryProperties).ToList();
  }

  // OnAfterSaveChanges - Handles temporary properties (e.g., autogenerated primary keys)
  private Task OnAfterSaveChanges(List<AuditEntry> auditEntries, CancellationToken cancellationToken)
  {
    if (auditEntries == null || !auditEntries.Any())
      return Task.CompletedTask;

    foreach (var auditEntry in auditEntries)
    {
      foreach (var prop in auditEntry.TemporaryProperties)
      {
        if (prop.Metadata.IsPrimaryKey())
        {
          auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
        }
        else
        {
          auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
        }
      }

      // Add finalized audit entry to AuditTrails
      AuditTrails.Add(auditEntry.ToAudit());
    }

    // Save audit entries to the database
    return base.SaveChangesAsync(cancellationToken);
  }
}
