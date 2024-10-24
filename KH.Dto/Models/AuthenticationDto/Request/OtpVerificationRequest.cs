namespace KH.Dto.Models.AuthenticationDto.Request;

public class OtpVerificationRequest : BasicTrackerEntityDto
{
  public string? OtpCode { get; set; }

  public OtpVerificationRequest()
  {

  }

}
