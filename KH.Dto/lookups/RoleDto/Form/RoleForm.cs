
using KH.Domain.Entities;

namespace KH.Dto.lookups.RoleDto.Form;

public class RoleForm : LookupEntityWithTrackingDto
{
  public bool IsUpdateMode { get; set; }
  public bool HasPermissionsUpdates { get; set; }
  public List<long> RolePermissionsIds { get; set; } = new();
  public RoleForm()
  {
  }
  public RoleForm(Role e)
  {
    NameAr = e.NameAr;
    NameEn = e.NameEn;
    Description = e.Description;
    CreatedDate = e.CreatedDate;
    UpdatedDate = e.UpdatedDate;
    CreatedById = e.CreatedById;
    RolePermissionsIds = e.RolePermissions.Select(x=> x.PermissionId).ToList();
  }
  public Role ToEntity()
  {
    var e = new Role()
    {
      NameAr = NameAr,
      NameEn = NameEn,
      Description = Description,
      RolePermissions = RolePermissionsIds.Select(x => new RolePermissions() { PermissionId = x }).ToList()
    };

    if (Id.HasValue)
    {
      //update mode
      IsUpdateMode = true;
      e.Id = Id.Value;
    }
    else
    {
      e.Id = Id.Value;
    }

    return e;
  }

}
