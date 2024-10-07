namespace KH.Dto.Lookups.RoleDto.Response;

public class RolesPermissionResponse
{
  public int RoleId { get; set; }
  public List<int> RoleFunctionIds { get; set; } = new List<int>();
}
