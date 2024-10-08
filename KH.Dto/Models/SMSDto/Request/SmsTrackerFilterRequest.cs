namespace KH.Dto.Models.SMSDto.Request;

public class SmsTrackerFilterRequest : PagingRequestHelper
{
  public int? Id { get; set; }
  public string? Status { get; set; }
  public bool IsSent { get; set; } = false;
  public bool IsDeleted { get; set; } = false;
  public int? ModelId { get; set; }
  public string Model { get; set; }
}
