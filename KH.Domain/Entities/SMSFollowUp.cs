using KH.BuildingBlocks.Extentions.Entities;

namespace KH.Domain.Entities;

public class SMSFollowUp : TrackerEntity
{
  public string MobileNumber { get; set; }
  public string Message { get; set; }
  public bool? IsSent { get; set; }
  public string? FailReason { get; set; }

  //related to SmsStatusEnum
  public string Status { get; set; }

  //related to ModelEnum
  public string Model { get; set; }
  //as fake FK of the related item
  public int ModelId { get; set; }
}
