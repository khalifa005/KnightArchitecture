
using KH.BuildingBlocks.Extentions.Entities;

namespace KH.Domain.Entities
{
  public class UserDepartment : TrackerEntity
  {
    public long UserId { get; set; }
    public User? User { get; set; }

    public long DepartmentId { get; set; }
    public Department? Department { get; set; }
  }
}
