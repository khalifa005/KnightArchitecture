namespace KH.Dto.Models.SMS.Request
{
  public class SMSFollowUpSearchRequestDto : PagingRequestHelper
  {
    public int? Id { get; set; }
    public string? Status { get; set; }

    public int? ModelId { get; set; }
  }
}
