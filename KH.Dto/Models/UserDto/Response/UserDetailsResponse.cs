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
    public int? GroupId { get; set; }
    public int? DepartmentId { get; set; }
    public int[]? RoleIds { get; set; }

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
  }

}
