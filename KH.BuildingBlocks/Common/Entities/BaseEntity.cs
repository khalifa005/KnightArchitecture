using System.ComponentModel.DataAnnotations.Schema;

namespace KH.BuildingBlocks.Apis.Entities;

public abstract partial class BaseEntity
{
  [Column(Order = 1)]
  public long Id { get; set; }
}
