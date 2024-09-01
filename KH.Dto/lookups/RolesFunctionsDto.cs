using KH.Dto.Models.lookups;
using Microsoft.AspNetCore.Http;

namespace KH.Dto.lookups
{
  public class RoleFunctionsDto
  {
    public int RoleId { get; set; }
    public List<int> RoleFunctionIds { get; set; } = new List<int>();
  }

}