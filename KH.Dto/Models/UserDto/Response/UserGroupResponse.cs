using KH.Domain.Entities;
using KH.Dto.lookups.GroupDto.Response;
using KH.Dto.lookups.RoleDto.Response;

namespace KH.Dto.Models.UserDto.Response
{
  public class UserGroupResponse
  {
    public long UserId { get; set; }
    //public UserListResponse? User { get; set; }

    public long GroupId { get; set; }
    public GroupResponse? Group { get; set; }

    public UserGroupResponse()
    {
          
    }
    public UserGroupResponse(UserGroup userGroup)
    {
      GroupId = userGroup.GroupId;
      UserId = userGroup.UserId;
      Group = userGroup.Group != null ? new GroupResponse(userGroup.Group) : null;
    }

  }
}
