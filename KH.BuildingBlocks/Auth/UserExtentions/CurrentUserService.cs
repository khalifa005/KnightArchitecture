namespace KH.BuildingBlocks.Auth.User;

public class CurrentUserService : ICurrentUserService
{
  public CurrentUserService(IHttpContextAccessor httpContextAccessor)
  {
    var user = httpContextAccessor?.HttpContext?.User;

    // Filling user claims with null-coalescing operators to avoid null values
    UserId = user?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    FirstName = user?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
    LastName = user?.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;
    Email = user?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
    MobileNumber = user?.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;

    // Filling role claims safely
    RolesIds = user?.Claims
        .Where(c => c.Type == ClaimTypes.Role)
        .Select(c => c.Value)
        .ToList() ?? new List<string>();

    // Storing all claims safely
    Claims = user?.Claims
        .Select(item => new KeyValuePair<string, string>(item.Type, item.Value))
        .ToList() ?? new List<KeyValuePair<string, string>>();
  }

  public string UserId { get; }
  public string FirstName { get; }
  public string LastName { get; }
  public string Email { get; }
  public string MobileNumber { get; }
  public List<string> RolesIds { get; }
  public List<KeyValuePair<string, string>> Claims { get; set; }
}
