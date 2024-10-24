using KH.BuildingBlocks.Apis.Entities;

namespace KH.Domain.Entities;

public class Calendar : TrackerEntity
{
  public bool IsHoliday { get; set; }
  public DateTime HolidayDate { get; set; }
  public string Description { get; set; }
}
