namespace KH.Dto.Models
{
  public class ResetCustomerPasswordDto : BasicTrackerEntityDto
  {
    public ResetCustomerPasswordDto()
    {

    }

    #region Props

    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    #endregion
  }
}
