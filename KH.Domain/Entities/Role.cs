
namespace KH.Domain.Entities
{
  public class Role : BasicEntity
  {
    public long? ReportToRoleId { get; set; }
    public Role? ReportToRole { get; set; }

  }


}
