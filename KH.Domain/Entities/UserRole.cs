
using KH.BuildingBlocks.Extentions.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace KH.Domain.Entities;

public class UserRole : TrackerEntity
{
  public long UserId { get; set; }
  public User? User { get; set; }

  public long RoleId { get; set; }
  public Role? Role { get; set; }

  [NotMapped]
  public List<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();
  //in RolePermissions we can list all user permissions instead of geeting it from multiple joins
  //and we will cache all roles permissions and user just the user roles to gets it;s related items

}
