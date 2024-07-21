

namespace KH.Dto.Models
{
  public class AuthenticateDto : BasicTrackerEntityDto
  {
    public AuthenticateDto()
    {

    }

    #region Props

    public string? AccessToken { get; set; }
    public int? Id { get; set; }
    public bool? NeedVerification { get; set; }
    public string? NewOtpCode { get; set; }
    public List<UserRoleDto> UserRoles { get; set; }
    #endregion
  }
}
