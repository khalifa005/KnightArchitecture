using KH.Dto.Models;

namespace KH.Dto.Models.User.Request
{
  public class UserSearchRequestDto : PagingRequestHelper
  {
    public int? Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public int? GroupId { get; set; }
    public int? DepartmentId { get; set; }
    public int? RoleId { get; set; }
    public bool? IsDeleted { get; set; } = false;
  }
}
