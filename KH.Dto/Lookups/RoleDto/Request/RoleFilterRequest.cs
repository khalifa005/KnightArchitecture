namespace KH.Dto.Lookups.RoleDto.Request;
public class RoleFilterRequest : PagingRequestHelper
{
  public long? Id { get; set; }
  public bool? IsDeleted { get; set; } = false;

}
