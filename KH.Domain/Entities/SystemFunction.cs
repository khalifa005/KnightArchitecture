
using KH.BuildingBlocks.Extentions.Entities;

namespace KH.Domain.Entities
{
  public class SystemFunction : LookupEntity
  {
    public int SortKey { get; set; }

    #region Parent
    public long? ParentID { get; set; }

    public virtual SystemFunction Parent { get; set; }
    #endregion

    public long? DependOnID { get; set; }

  }
}
