
using KH.BuildingBlocks.Extentions.Entities;

namespace KH.Domain.Entities;

public class RolePermissions : TrackerEntity
{
  public long SystemActionsId { get; set; }
  public SystemActions SystemActions { get; set; }

  public long RoleId { get; set; }
  public Role Role { get; set; }
}
