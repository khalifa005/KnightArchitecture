
using KH.BuildingBlocks.Extentions.Entities;

namespace KH.Domain.Entities
{
  public class Role : LookupEntity
  {
    public long? ReportToRoleId { get; set; }
    public Role? ReportToRole { get; set; }

  }


}
