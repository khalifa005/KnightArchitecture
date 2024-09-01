using KH.Dto.lookups.RoleDto;

namespace KH.Dto.Models.AuthenticationDto.Response
{
  public class AuthenticationResponse
  {
    public string? AccessToken { get; set; }
    public int? Id { get; set; }
    public bool? NeedVerification { get; set; }
    public string? NewOtpCode { get; set; }
    public List<UserRoleDto> UserRoles { get; set; }
  }
}
