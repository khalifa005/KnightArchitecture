using System.Security.Claims;

namespace KH.BuildingBlocks.Auth.User;

public interface ICurrentUserService
{
  string UserId { get; }
  public List<string> RolesIds { get; }
}
public class CurrentUserService : ICurrentUserService
{

  public CurrentUserService(IHttpContextAccessor httpContextAccessor)
  {
    UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    RolesIds = httpContextAccessor.HttpContext?.User?.Claims
    .Where(c => c.Type == ClaimTypes.Role)
    .Select(c => c.Value)
    .ToList();

    Claims = httpContextAccessor.HttpContext?.User?.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList();
  }

  public string UserId { get; }
  public List<string> RolesIds { get; }
  public List<KeyValuePair<string, string>> Claims { get; set; }


}
