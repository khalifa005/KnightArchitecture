namespace KH.Dto.Lookups.PermissionsDto.Request;

public class PermissionFilterRequest : PagingRequestHelper
{
  public List<long> RoleIds { get; set; } = new List<long>();
  public bool? IsDeleted { get; set; } // Nullable to allow fetching both deleted and non-deleted if not specified
}
