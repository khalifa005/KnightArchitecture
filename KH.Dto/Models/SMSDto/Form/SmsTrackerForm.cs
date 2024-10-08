using KH.Domain.Entities;

namespace KH.Dto.Models.SMSDto.Form;

public class SmsTrackerForm
{
  public long? Id { get; set; }
  public string MobileNumber { get; set; }
  public string Message { get; set; }
  public bool? IsSent { get; set; }
  public bool IsPending { get; set; }
  public string? FailureReasons { get; set; }
  public string Status { get; set; }
  //related to ModelEnum.ToString()
  public string Model { get; set; }
  public long ModelId { get; set; }

  public SmsTrackerForm()
  {

  }

  public SmsTrackerForm(SmsTracker e)
  {
    Id = e.Id;
    ModelId = e.ModelId;
    Model = e.Model;
    Message = e.Message;
    MobileNumber = e.MobileNumber;
    Status = e.Status;
    IsSent = e.IsSent;
    FailureReasons = e.FailureReasons;
  }

  public SmsTracker ToEntity()
  {
    var entity = new SmsTracker();
    entity.IsSent = IsSent;
    entity.FailureReasons = FailureReasons;
    entity.MobileNumber = MobileNumber;
    entity.Model = Model;
    entity.ModelId = ModelId;
    entity.Message = Message;
    entity.Status = Status;

    if (Id.HasValue)
    {
      entity.Id = Id.Value;
    }

    return entity;

  }
}
