using System.ComponentModel.DataAnnotations.Schema;

namespace KH.BuildingBlocks.Extentions.Entities;

public abstract partial class HistoryTrackerEntity : BaseEntity
{
  [Column(Order = 103)]
  public DateTime? UpdatedDate { get; set; }
  [Column(Order = 104)]
  public long? UpdatedById { get; set; }
}
