

using KH.BuildingBlocks.Extentions.Entities;

namespace KH.Domain.Entities;

public class UserGroup : TrackerEntity
{
  public long UserId { get; set; }
  public User? User { get; set; }

  public long GroupId { get; set; }
  public Group? Group { get; set; }


}
