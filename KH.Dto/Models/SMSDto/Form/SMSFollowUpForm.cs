namespace KH.Dto.Models.SMSDto.Form
{
  public class SMSFollowUpForm
  {
    public string MobileNumber { get; set; }
    public string Message { get; set; }
    public bool? IsSent { get; set; }
    public string? FailReason { get; set; }

    /// <summary>
    /// SET Status Of SMS 
    /// IN CASE IsPending --> [TRUE]  ELSE BASED ON HTTPResponse [FAIL/SUCCESS]
    /// </summary>
    //related to SmsStatusEnum
    public string Status { get; set; }

    //related to ModelEnum.ToString()
    public string Model { get; set; }
    //as fake FK of the related item
    public int ModelId { get; set; }

    /// <summary>
    /// SET When Need SMS To Be Saved And Not SENT Directly
    /// </summary>
    public bool IsPending { get; set; }
  }

}
