# Auditing Implementation in KnightArchitecture

## Overview

The auditing feature in the `KnightHedgeArchitecture` repository tracks changes made by users to critical data,
capturing detailed information about modifications. It stores information such as the table name, type of change (Create, Update, Delete), 
timestamp, and old/new values, requestId. This helps maintain a history of user actions for security, compliance, and analysis.

### Key Features
- **Detailed Change Tracking**: Captures both the old and new values of modified data.
- **User and Request Information**: Associates each audit record with a specific user and request.
- **Export and Import Support**: Allows exporting audit trails to Excel and importing external audits.
- **Granular Permission Control**: Enforces permissions for viewing, exporting, and importing audits.

---

## Files and Their Responsibilities

### 1. **Audit Entity Model**
The `Audit` entity represents the structure of audit records in the database.

- **File**: [`Audit.cs`](https://github.com/khalifa005/KnightHedgeArchitecture/blob/master/KH.BuildingBlocks/Common/Entities/Audit.cs)
- **Responsibilities**:
  - Defines the structure of an audit record.
  - Stores metadata such as user ID, type of operation, affected columns, and old/new values.
  - Converts dictionary properties into JSON for efficient storage and retrieval.

  ```csharp
  public class Audit : BaseEntity
  {
      public string? UserId { get; set; }
      public required string Type { get; set; }
      public required string TableName { get; set; }
      public DateTime DateTime { get; set; }
      public string? OldValues { get; set; }
      public string? NewValues { get; set; }
      public required string AffectedColumns { get; set; }
      public required string PrimaryKey { get; set; }
      public required string RequestId { get; set; }
  }
  ```

## 2. NoAudit Attribute
**File:** `NoAuditAttribute.cs`

**Responsibilities:**
- **Audit Exclusion:** The `NoAudit` attribute is used to mark classes that should not be audited.
- **Functionality:** Prevents the auditing of certain entities by excluding them from the auditing logic.
- **Application:** Applied to classes that do not require audit trail recording.

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class NoAuditAttribute : Attribute { }

``` 

## 3. Audit Service Interface and Implementation
**Files:**
- `IAuditService.cs`
- `AuditService.cs`

**Responsibilities:**
- **IAuditService:** Declares methods to retrieve user audit trails, export to Excel, and import external audits.
- **AuditService:** Implements the following methods:
  - **Retrieve and format audit logs** for a specific user.
  - **Export audit trails** to an Excel file.
  - **Import external audit records** from an Excel file.

```csharp
public interface IAuditService
{
    Task<ApiResponse<List<AuditResponse>>> GetCurrentUserTrailsAsync(string userId, CancellationToken cancellationToken);
    Task<ApiResponse<string>> ExportToExcelAsync(string userId, CancellationToken cancellationToken, string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false);
    Task<ApiResponse<string>> ImportExternalAudit(IFormFile file, CancellationToken cancellationToken);
}

```
## 4. Audit Configuration
**File:** `AuditConfiguration.cs`

**Responsibilities:**
- **Database Schema Configuration:** Configures the `Audit` entity’s database schema for proper storage.
- **Indexing:** Sets up indexing on fields, such as `UserId`, to enhance query performance.

```csharp
public class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.Property(p => p.Id).UseIdentityColumn();
        builder.HasIndex(e => e.UserId);
    }
}
```
## 5. Audits Controller
**File:** `AuditsController.cs`

**Responsibilities:**
- **Endpoint Exposure:** Exposes endpoints for retrieving, exporting, and importing audit data.
- **Security:** Applies permission requirements for each endpoint to ensure secure access.

```csharp
public class AuditsController : BaseApiController
{
    private readonly IAuditService _auditService;

    public AuditsController(IAuditService auditService)
    {
        _auditService = auditService;
    }

    [PermissionAuthorize(PermissionKeysConstant.Audits.VIEW_AUDITS)]
    [HttpGet("GetUserAudits/{userId}")]
    public async Task<ActionResult<ApiResponse<List<AuditResponse>>>> Get(string userId, CancellationToken cancellationToken)
    {
        var res = await _auditService.GetCurrentUserTrailsAsync(userId, cancellationToken);
        return AsActionResult(res);
    }
}
```
## Configuration and Usage

### 1. Database Configuration
- Ensure the `Audit` entity is configured in your `DbContext` with appropriate indexing to optimize performance.

### 2. Service Registration
- Register `IAuditService` and `AuditService` in `Startup.cs` or within your dependency injection configuration.

```csharp
  services.AddScoped<IAuditService, AuditService>();
```

```csharp
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
```
