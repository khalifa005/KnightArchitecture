
using KH.BuildingBlocks.Extentions.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace KH.Domain.Entities
{
  public class UserRole : TrackerEntity
  {
    public long UserId { get; set; }
    public User? User { get; set; }

    public long RoleId { get; set; }
    public Role? Role { get; set; }

    [NotMapped]
    public List<RoleFunction> RoleFunctions { get; set; } = new List<RoleFunction>();

  }
}
