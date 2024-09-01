using KH.Domain.Commons;

namespace KH.Dto.common
{
  public class BasicEntityWithTrackingDto : BasicTrackerEntityDto
  {
    public string NameAr { get; set; }
    public string NameEn { get; set; }
    public string? Description { get; set; }

  }
}
