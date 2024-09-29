namespace KH.Dto.Models.AuthenticationDto.Request;

public class OtpVerificationRequest : BasicTrackerEntityDto
{
  public int? Id { get; set; }
  public string? OtpCode { get; set; }

  public OtpVerificationRequest()
  {

  }

}
