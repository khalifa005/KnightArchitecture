using KH.Dto.lookups.RoleDto;

namespace KH.Dto.Models.User.Response
{
  public class UserDetailsResponseDto : BasicTrackerEntityDto
  {

    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? Token { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? MobileNumber { get; set; }
    public int? GroupId { get; set; }
    public int? DepartmentId { get; set; }
    public int[]? RoleIds { get; set; }

    public DateTime? LastAssignDateAsSupervisor { get; set; }
    public DateTime? LastAssignDateAsCaseOwner { get; set; }
    public DateTime? LastAssignDateAsAssignTo { get; set; }

    public ICollection<UserRoleDto> UserRoles { get; set; } = new HashSet<UserRoleDto>();
    //public ICollection<UserGroupDto> UserGroups { get; set; } = new HashSet<UserGroup>();
    //public ICollection<UserDepartmentDto> UserDepartments { get; set; } = new HashSet<UserDepartment>();


    public string FullName
    {
      get
      {
        return $"{FirstName} {LastName}";
      }
    }
  }
}
