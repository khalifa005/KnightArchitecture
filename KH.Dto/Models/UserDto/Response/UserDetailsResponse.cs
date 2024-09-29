using KH.Domain.Entities;

namespace KH.Dto.Models.UserDto.Response
{
  public class UserDetailsResponse : BasicTrackerEntityDto
  {

    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Token { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? MobileNumber { get; set; }
    public long? GroupId { get; set; }
    public long? DepartmentId { get; set; }
    public long[]? RoleIds { get; set; }

    public DateTime? LastAssignDateAsSupervisor { get; set; }
    public DateTime? LastAssignDateAsCaseOwner { get; set; }
    public DateTime? LastAssignDateAsAssignTo { get; set; }

    public ICollection<UserRoleResponse> UserRoles { get; set; } = new HashSet<UserRoleResponse>();
    public ICollection<UserGroupResponse> UserGroups { get; set; } = new HashSet<UserGroupResponse>();
    public ICollection<UserDepartmentResponse> UserDepartments { get; set; } = new HashSet<UserDepartmentResponse>();


    public string FullName
    {
      get
      {
        return $"{FirstName} {LastName}";
      }
    }
    // Constructor to map from User entity to UserDetailsResponse
    public UserDetailsResponse(User user)
    {
      FirstName = user.FirstName;
      MiddleName = user.MiddleName;
      LastName = user.LastName;
      Email = user.Email;
      Username = user.Username;
      BirthDate = user.BirthDate;
      MobileNumber = user.MobileNumber;
      RoleIds = user.UserRoles.Select(ur => ur.RoleId).ToArray();

      // Map UserRoles, UserGroups, UserDepartments
      UserRoles = user.UserRoles.Select(ur => new UserRoleResponse(ur)).ToList();
      UserGroups = user.UserGroups.Select(ug => new UserGroupResponse(ug)).ToList();
      UserDepartments = user.UserDepartments.Select(ud => new UserDepartmentResponse(ud)).ToList();
    }

  }

}
