using System.ComponentModel.DataAnnotations.Schema;

namespace KH.BuildingBlocks.Extentions.Entities;

public abstract partial class LookupEntity : TrackerEntity
{
  [Column(Order = 2)]
  public string NameAr { get; set; }
  [Column(Order = 3)]
  public string NameEn { get; set; }
  [Column(Order = 4)]
  public string? Description { get; set; }
}
