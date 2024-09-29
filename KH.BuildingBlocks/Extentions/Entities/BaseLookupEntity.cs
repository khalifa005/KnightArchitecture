using System.ComponentModel.DataAnnotations.Schema;

namespace KH.BuildingBlocks.Extentions.Entities
{
  public abstract partial class BaseLookupEntity
  {
    [Column(Order = 1)]
    public int Id { get; set; }
  }


}
