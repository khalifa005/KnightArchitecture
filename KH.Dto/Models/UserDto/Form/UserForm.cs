using KH.Domain.Entities;

namespace KH.Dto.Models.UserDto.Form
{
  public class UserForm
  {

    #region Props

    public long? Id { get; set; }
    public bool? IsUpdateMode { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? MobileNumber { get; set; }
    public long? GroupId { get; set; }
    public long? DepartmentId { get; set; }
    public long[]? RoleIds { get; set; }
    public UserForm()
    {
          
    }
    #endregion

    #region MappingMethods
    // Constructor that maps from User entity to UserForm
    public UserForm(User e)
    {
      Id = e.Id;
      FirstName = e.FirstName;
      MiddleName = e.MiddleName;
      LastName = e.LastName;
      Email = e.Email;
      Username = e.Username;
      BirthDate = e.BirthDate;
      MobileNumber = e.MobileNumber;
      GroupId = e.UserGroups.FirstOrDefault()?.GroupId; // Assuming a user can belong to only one group
      DepartmentId = e.UserDepartments.FirstOrDefault()?.DepartmentId; // Assuming a user can belong to only one department
      RoleIds = e.UserRoles.Select(ur => ur.RoleId).ToArray();
      IsUpdateMode = e.Id > 0; // Set IsUpdateMode based on the presence of an Id
    }

    // Method to map from UserForm to User entity
    public User ToEntity()
    {
      var user = new User
      {
        FirstName = this.FirstName,
        MiddleName = this.MiddleName,
        LastName = this.LastName,
        Email = this.Email,
        Username = this.Username,
        BirthDate = this.BirthDate,
        MobileNumber = this.MobileNumber
      };

      // If we're in update mode, set the Id
      if (Id.HasValue)
      {
        user.Id = Id.Value;
        IsUpdateMode = true;
      }

      // Map GroupId, DepartmentId, and RoleIds
      if (GroupId.HasValue)
      {
        user.UserGroups = new List<UserGroup> { new UserGroup { GroupId = GroupId.Value } };
      }

      if (DepartmentId.HasValue)
      {
        user.UserDepartments = new List<UserDepartment> { new UserDepartment { DepartmentId = DepartmentId.Value } };
      }

      if (RoleIds != null && RoleIds.Length > 0)
      {
        user.UserRoles = RoleIds.Select(roleId => new UserRole { RoleId = roleId }).ToList();
      }

      return user;
    }
    #endregion
  }

}
