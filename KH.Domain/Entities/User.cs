
using KH.BuildingBlocks.Apis.Entities;
using KH.BuildingBlocks.Common.Attributes;

namespace KH.Domain.Entities;
//[NoAudit]
public class User : TrackerEntity
{
  //public byte[] RowVersion { get; set; } // Concurrency token
  public string? FirstName { get; set; }
  public string? MiddleName { get; set; }
  public string? LastName { get; set; }
  public string? FullName { get { return FirstName + " " + LastName; } }
  public string? MobileNumber { get; set; }
  public string? Email { get; set; }
  public string? Username { get; set; }
  public DateTime? BirthDate { get; set; }

  public string? OtpCode { get; set; }
  public bool IsOtpVerified { get; set; }
  public string? Password { get; set; }
  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiryTime { get; set; }
  public DateTime RefreshTokenCreatedDate { get; set; }
  public DateTime? RefreshTokenRevokedDate { get; set; }

  public DateTime? LastAssignDateAsSupervisor { get; set; }
  public DateTime? LastAssignDateAsCaseOwner { get; set; }
  public DateTime? LastAssignDateAsAssignTo { get; set; }

  public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
  public ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();
  public ICollection<UserDepartment> UserDepartments { get; set; } = new HashSet<UserDepartment>();
}

