using KH.Dto.Models.lookups;
using Microsoft.AspNetCore.Http;

namespace KH.Dto.lookups
{
  public class UserRoleDto
  {

    public int RoleId { get; set; }
    public string RoleNameEn { get; set; }
    public string RoleNameAr { get; set; }
    public List<string> RoleFunctions { get; set; }

  }


}
