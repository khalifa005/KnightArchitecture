namespace KH.Dto.Models.Calendar.Response
{
  public class CalendarDetailsResponseDto : BasicTrackerEntityDto
  {
    public bool IsHoliday { get; set; }
    public DateTime HolidayDate { get; set; }
    public string? Description { get; set; }
  }
}
