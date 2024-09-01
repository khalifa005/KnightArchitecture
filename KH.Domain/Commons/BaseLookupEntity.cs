using System.ComponentModel.DataAnnotations.Schema;

namespace KH.Domain.Commons
{
  public abstract partial class BaseLookupEntity
  {
    [Column(Order = 1)]
    public int Id { get; set; }
  }


}
