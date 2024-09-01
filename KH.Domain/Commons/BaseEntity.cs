using System.ComponentModel.DataAnnotations.Schema;

namespace KH.Domain.Commons
{
  public abstract partial class BaseEntity
  {
    [Column(Order = 1)]
    public long Id { get; set; }
  }


}
