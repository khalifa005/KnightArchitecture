using KH.Domain.Entities;

namespace KH.Dto.Lookups.RoleDto.Request;

public class CreateRoleRequest : LookupEntityWithTrackingDto
{
  public bool IsUpdateMode { get; set; }
  public bool HasPermissionsUpdates { get; set; }
  public List<long> RolePermissionsIds { get; set; } = new();
  public CreateRoleRequest()
  {
  }
  public CreateRoleRequest(Role e)
  {
    NameAr = e.NameAr;
    NameEn = e.NameEn;
    Description = e.Description;
    CreatedDate = e.CreatedDate;
    UpdatedDate = e.UpdatedDate;
    CreatedById = e.CreatedById;
    RolePermissionsIds = e.RolePermissions.Select(x => x.PermissionId).ToList();
  }
  public Role ToEntity()
  {
    var e = new Role()
    {
      NameAr = NameAr,
      NameEn = NameEn,
      Description = Description,
      RolePermissions = RolePermissionsIds?.Select(x => new RolePermissions() { PermissionId = x }).ToList() ?? new List<RolePermissions>()
    };

    if (Id.HasValue)
    {
      //update mode
      IsUpdateMode = true;
      e.Id = Id.Value;
    }

    return e;
  }

}
