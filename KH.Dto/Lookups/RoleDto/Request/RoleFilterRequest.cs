namespace KH.Dto.Lookups.RoleDto.Request;
public class RoleFilterRequest : CommonLookupFiltersRequest
{
  public long? ReportToRoleId { get; set; }
}
