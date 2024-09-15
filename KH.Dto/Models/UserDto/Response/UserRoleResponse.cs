using KH.Domain.Entities;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.RoleDto.Response;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace KH.Dto.Models.UserDto.Response
{
  public class UserRoleResponse
  {
    public long UserId { get; set; }
    //public UserListResponse? User { get; set; }

    public long RoleId { get; set; }
    public RoleResponse? Role { get; set; }

    public List<RoleFunctionsResponse> RoleFunctions { get; set; } = new List<RoleFunctionsResponse>();
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
}
