
namespace KH.Dto.Models.UserDto.Request;

public class UserFilterRequest : PagingRequestHelper
{
  public long? Id { get; set; }
  public string? Email { get; set; }
  public string? UserName { get; set; }
  public string Language { get; set; } = "ar";
  public long? GroupId { get; set; }
  public long? DepartmentId { get; set; }
  public long? RoleId { get; set; }
  public bool? IsDeleted { get; set; } = false;
}
