
namespace KH.Domain.Entities
{
  public class RoleFunction : TrackerEntity
  {
    public long SystemFunctionId { get; set; }
    public SystemFunction SystemFunction { get; set; }

    public long RoleId { get; set; }
    public Role Role { get; set; }
  }
}
