using KH.Domain.Entities;

namespace KH.Dto.Lookups.RoleDto.Request;

public class UpdatedRolePermissionsRequest 
{
  public long Id { get; set; }
  public List<long> RolePermissionsIds { get; set; } = new();
  public UpdatedRolePermissionsRequest()
  {
  }
  public Role ToEntity()
  {
    var e = new Role()
    {
      RolePermissions = RolePermissionsIds.Select(x => new RolePermissions() { PermissionId = x }).ToList()
    };

    return e;
  }

}
