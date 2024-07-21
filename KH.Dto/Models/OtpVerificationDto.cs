namespace KH.Dto.Models
{
  public class OtpVerificationDto : BasicTrackerEntityDto
  {
    public OtpVerificationDto()
    {

    }

    #region Props

    public int? Id { get; set; }
    public string? OtpCode { get; set; }
    #endregion
  }
}
