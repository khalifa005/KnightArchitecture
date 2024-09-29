namespace KH.Dto.Models.CalendarDto.Response;

public class CalendarResponse : BasicTrackerEntityDto
{
  public bool IsHoliday { get; set; }
  public DateTime HolidayDate { get; set; }
  public string? Description { get; set; }
}
