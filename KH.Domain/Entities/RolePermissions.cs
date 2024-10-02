
using KH.BuildingBlocks.Extentions.Entities;

namespace KH.Domain.Entities;

public class RolePermissions : TrackerEntity
{
  public long PermissionId { get; set; }
  public Permission Permission { get; set; }

  public long RoleId { get; set; }
  public Role Role { get; set; }
}
