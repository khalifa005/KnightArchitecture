namespace KH.Dto.Models.Calendar.Request
{
  public class CalendarRequestDto : PagingRequestHelper
  {
    public string? Id { get; set; }
    public bool? IsDeleted { get; set; } = false;
    public bool? FilterFutureDate { get; set; } = false;

  }


}
