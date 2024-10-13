
using KH.BuildingBlocks.Apis.Entities;
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
  public List<RolePermissions> AggregateRolePermissions(Role role)
  {
    var permissions = new List<RolePermissions>();
    GatherPermissions(role, permissions);
    return permissions;
  }

  private void GatherPermissions(Role role, List<RolePermissions> permissions)
  {
    // Add permissions from the current role
    if (role.RolePermissions != null && role.RolePermissions.Any())
    {
      permissions.AddRange(role.RolePermissions);
    }

    //// Recursively add permissions from sub-roles
    //foreach (var subRole in role.SubRoles)
    //{
    //  GatherPermissions(subRole, permissions);
    //}
  }
  //in RolePermissions we can list all user permissions instead of geeting it from multiple joins
  //and we will cache all roles permissions and user just the user roles to gets it;s related items

}
