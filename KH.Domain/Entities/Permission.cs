
using KH.BuildingBlocks.Apis.Entities;

namespace KH.Domain.Entities;

public class Permission : LookupEntity
{
  public string Key { get; set; }
  public int SortKey { get; set; }
  public long? ParentId { get; set; }
  public virtual Permission Parent { get; set; }
  public long? DependOnId { get; set; }
  public ICollection<Permission> Children { get; set; } = new HashSet<Permission>();
  public ICollection<RolePermissions> RolePermissions { get; set; } = new HashSet<RolePermissions>();
}
