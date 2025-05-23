using KH.BuildingBlocks.Apis.Helpers;
using KH.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace KH.Dto.Models.UserDto.Request;

public class CreateUserRequest
{

  #region Props

  public long? Id { get; set; }
  public bool? IsUpdateMode { get; set; }
  public string? SensitiveData { get; set; }
  public string? FirstName { get; set; }
  public string? MiddleName { get; set; }
  public string? LastName { get; set; }
  public string? Email { get; set; }
  public string? Username { get; set; }
  public DateTime? BirthDate { get; set; }
  public string? MobileNumber { get; set; }
  public string? OtpCode { get; set; }
  public bool IsOtpVerified { get; set; }
  public string? Password { get; set; }
  public long? GroupId { get; set; }
  public long? DepartmentId { get; set; }
  public long[]? RoleIds { get; set; }
  public CreateUserRequest()
  {

  }
  #endregion

  #region MappingMethods
  // Constructor that maps from User entity to UserForm
  public CreateUserRequest(User e)
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
      SensitiveData = SensitiveData,
      FirstName = FirstName,
      MiddleName = MiddleName,
      LastName = LastName,
      Email = Email,
      Username = Username,
      BirthDate = BirthDate,
      MobileNumber = MobileNumber
    };

    // If we're in update mode, set the Id
    if (Id.HasValue)
    {
      user.Id = Id.Value;
      IsUpdateMode = true;
    }
    else
    {
      user.Password = new PasswordHasher<object?>().HashPassword(null, Password);
      user.OtpCode = CryptoRandomGenerator.GenerateOTP();
      user.IsOtpVerified = false;
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
