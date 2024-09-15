namespace KH.Dto.Lookups.RoleDto.Response
{
  public class RoleFunctionsResponse
  {
    public int RoleId { get; set; }
    public List<int> RoleFunctionIds { get; set; } = new List<int>();
  }

}
