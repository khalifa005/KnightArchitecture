using KH.Domain.Entities;

namespace KH.Dto.Models.UserDto.Response
{
  public class UserListResponse : BasicTrackerEntityDto
  {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? MobileNumber { get; set; }

    public List<string> DepartmentNames { get; set; }
    public ICollection<UserRoleResponse> UserRoles { get; set; } = new HashSet<UserRoleResponse>();
    public UserListResponse()
    {

    }
    public UserListResponse(User e)
    {
      Id = e.Id;
      FirstName = e.FirstName;
      LastName = e.LastName;
      Email = e.Email;
      Username = e.Username;
      BirthDate = e.BirthDate;
      MobileNumber = e.MobileNumber;
    }
  }
}
