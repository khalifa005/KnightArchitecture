using KH.BuildingBlocks.Apis.Entities;


namespace KH.Domain.Entities;

public class Role : LookupEntity
{
  public long? ReportToRoleId { get; set; }
  public Role? ReportToRole { get; set; }
  public ICollection<Role> SubRoles { get; set; } = new HashSet<Role>();
  public ICollection<RolePermissions> RolePermissions { get; set; } = new HashSet<RolePermissions>();
}
