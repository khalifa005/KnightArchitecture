
using KH.BuildingBlocks.Extentions.Entities;

namespace KH.Domain.Entities
{
  public class User : TrackerEntity
  {

    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get { return FirstName + " " + LastName; } }
    public string? MobileNumber { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public DateTime? BirthDate { get; set; }

    public DateTime? LastAssignDateAsSupervisor { get; set; }
    public DateTime? LastAssignDateAsCaseOwner { get; set; }
    public DateTime? LastAssignDateAsAssignTo { get; set; }

    public ICollection<UserRole> UserRoles { get; set; }
    public ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();
    public ICollection<UserDepartment> UserDepartments { get; set; } = new HashSet<UserDepartment>();
  }
}
