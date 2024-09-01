namespace KH.Dto.Models.SMSDto.Request
{
  public class SMSFollowUpFilterRequest : PagingRequestHelper
  {
    public int? Id { get; set; }
    public string? Status { get; set; }

    public int? ModelId { get; set; }
  }
}
