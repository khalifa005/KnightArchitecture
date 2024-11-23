using KH.Domain.Entities;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.RoleDto.Response;

namespace KH.Dto.Models.UserDto.Response;

public partial class UserRoleResponse
{
  public long UserId { get; set; }
  //public UserListResponse? User { get; set; }

  public long RoleId { get; set; }
  public RoleResponse? Role { get; set; }

  public List<RolesPermissionResponse> RoleFunctions { get; set; } = new List<RolesPermissionResponse>();
  public UserRoleResponse()
  {

  }

  // Constructor to map from UserRole entity to UserRoleResponse
  public UserRoleResponse(UserRole userRole)
  {
    RoleId = userRole.RoleId;
    UserId = userRole.UserId;
    Role = userRole.Role != null ? new RoleResponse(userRole.Role) : null;
  }

}
