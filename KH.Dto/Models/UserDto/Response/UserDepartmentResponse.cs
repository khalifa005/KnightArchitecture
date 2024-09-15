using KH.Dto.lookups.DepartmentDto.Response;

namespace KH.Dto.Models.UserDto.Response
{
  public class UserDepartmentResponse
  {
    public long UserId { get; set; }
    //public UserListResponse? User { get; set; }

    public long DepartmentId { get; set; }
    public DepartmentResponse? Department { get; set; }

  }
}
