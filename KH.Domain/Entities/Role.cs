using KH.BuildingBlocks.Apis.Entities;
using System.ComponentModel.DataAnnotations;


namespace KH.Domain.Entities;

public class Role : LookupEntity
{
  [Timestamp]
  public byte[]? RowVersion { get; set; }
  public long? ReportToRoleId { get; set; }
  public Role? ReportToRole { get; set; }
  public ICollection<Role> SubRoles { get; set; } = new HashSet<Role>();
  public ICollection<RolePermissions> RolePermissions { get; set; } = new HashSet<RolePermissions>();
}
