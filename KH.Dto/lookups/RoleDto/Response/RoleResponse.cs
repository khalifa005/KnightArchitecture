using KH.Domain.Entities;
using KH.Dto.Lookups.PermissionsDto.Response;

namespace KH.Dto.lookups.RoleDto.Response;

public class RoleResponse : LookupEntityWithTrackingDto
{
  //there is no cutom props because it's lookup and common dto has all needed props
  //we can use auto mapper to do mapping or doing our own using ctor
  public long? ReportToRoleId { get; set; }
  public RoleResponse? ReportToRole { get; set; }
  public ICollection<RoleResponse> SubRoles { get; set; } = new HashSet<RoleResponse>();
  public ICollection<PermissionResponse> Permissions { get; set; } = new HashSet<PermissionResponse>();
  public RoleResponse()
  {
  }

  public RoleResponse(Role e)
  {
    Id = e.Id;
    NameAr = e.NameAr;
    NameEn = e.NameEn;
    Description = e.Description;
    CreatedDate = e.CreatedDate;
    UpdatedDate = e.UpdatedDate;
    CreatedById = e.CreatedById;
    Permissions = e.RolePermissions.Select(x => new PermissionResponse(x.Permission)).ToList();
  }


}
