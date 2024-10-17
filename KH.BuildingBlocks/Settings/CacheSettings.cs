using System.ComponentModel.DataAnnotations;

namespace KH.BuildingBlocks.Settings;

public class CacheSettings
{
  [Required]
  public int AbsoluteExpirationInHours { get; set; }
  [Required]
  public int SlidingExpirationInMinutes { get; set; }
}
