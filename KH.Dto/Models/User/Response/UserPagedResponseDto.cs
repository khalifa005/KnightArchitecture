using KH.Dto.Models;

namespace KH.Dto.Models.User.Response
{
  public class UserPagedResponseDto : BasicTrackerEntityDto
  {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? MobileNumber { get; set; }
  }
}
