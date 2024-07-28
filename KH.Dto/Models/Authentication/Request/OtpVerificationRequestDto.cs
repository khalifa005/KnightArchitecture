namespace KH.Dto.Models.Authentication.Request
{
  public class OtpVerificationRequestDto : BasicTrackerEntityDto
  {
    public OtpVerificationRequestDto()
    {

    }

    #region Props

    public int? Id { get; set; }
    public string? OtpCode { get; set; }
    #endregion
  }
}
