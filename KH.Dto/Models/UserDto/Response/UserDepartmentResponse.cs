using KH.Domain.Entities;
using KH.Dto.lookups.DepartmentDto.Response;

namespace KH.Dto.Models.UserDto.Response;

public class UserDepartmentResponse
{
  public long UserId { get; set; }
  //public UserListResponse? User { get; set; }

  public long DepartmentId { get; set; }
  public DepartmentResponse? Department { get; set; }

  // Constructor to map from UserDepartment entity to UserDepartmentResponse
  public UserDepartmentResponse(UserDepartment userDepartment)
  {
    DepartmentId = userDepartment.DepartmentId;
    UserId = userDepartment.UserId;
    Department = userDepartment.Department != null ? new DepartmentResponse(userDepartment.Department) : null;

  }
}
