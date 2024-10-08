using KH.Domain.Entities;

namespace KH.Dto.Models.SMSDto.Response;

public class SmsTrackerResponse : BasicTrackerEntityDto
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

  public SmsTrackerResponse()
  {

  }

  public SmsTrackerResponse(SmsTracker e)
  {
    Id = e.Id;
    Model = e.Model;
    ModelId = e.ModelId;
    Message = e.Message;
    MobileNumber = e.MobileNumber;
    Status = e.Status;
    IsSent = e.IsSent;
    FailureReasons = e.FailureReasons;
    CreatedDate = e.CreatedDate;
    CreatedById = e.CreatedById;
    UpdatedDate = e.UpdatedDate;
    UpdatedById = e.UpdatedById;
    IsDeleted = e.IsDeleted;
  }
}
