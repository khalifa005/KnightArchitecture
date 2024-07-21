namespace KH.Dto.Models
{
  public class CalendarDto : BasicTrackerEntityDto
  {
    public bool IsHoliday { get; set; }
    public DateTime HolidayDate { get; set; }
    public string? Description { get; set; }
  }
}
