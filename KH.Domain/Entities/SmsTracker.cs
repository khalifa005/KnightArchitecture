using KH.BuildingBlocks.Apis.Entities;

namespace KH.Domain.Entities;

public class SmsTracker : TrackerEntity
{
  public string MobileNumber { get; set; }
  public string Message { get; set; }
  public bool? IsSent { get; set; }
  public string? FailureReasons { get; set; }
  //related to SmsStatusEnum
  public string Status { get; set; }
  //related to ModelEnum
  public string Model { get; set; }
  public long ModelId { get; set; }
}
