namespace KH.Dto.Models.CalendarDto.Request;

public class CalendarRequest : PagingRequestHelper
{
  public string? Id { get; set; }
  public bool? IsDeleted { get; set; } = false;
  public bool? FilterFutureDate { get; set; } = false;

}
