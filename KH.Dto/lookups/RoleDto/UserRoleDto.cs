using Microsoft.AspNetCore.Http;

namespace KH.Dto.lookups.RoleDto
{
  public class UserRoleDto
  {

    public int RoleId { get; set; }
    public string RoleNameEn { get; set; }
    public string RoleNameAr { get; set; }
    public List<string> RoleFunctions { get; set; }

  }


}
