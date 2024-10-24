
using KH.BuildingBlocks.Apis.Constant;

namespace KH.BuildingBlocks.Auth.User;

public static class UserExtensions
{
  public static int? GetDepartmentId(this ClaimsPrincipal? identity)
  {
    Claim? claim = identity?.FindFirst(ApplicationConstant.DEPARTMENT_ID_CLAIM);

    if (claim == null)
      return null;

    return int.TryParse(claim.Value, out var departmentId) ? departmentId : (int?)null;
  }

  public static bool IsManager(this ClaimsPrincipal? identity)
  {
    Claim? claim = identity?.FindFirst(ApplicationConstant.IS_MANAGER_CLAIM);

    if (claim == null)
      return false;

    return bool.TryParse(claim.Value, out var isManager) && isManager;
  }

  public static long? GetUserId(this IServiceProvider serviceProvider)
  {
    var context = serviceProvider.GetService<IHttpContextAccessor>();
    Claim? claim = context?.HttpContext?.User?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

    if (claim == null)
      return null;

    return long.TryParse(claim.Value, out var userId) ? userId : (long?)null;
  }

  public static string? GetId(this ClaimsPrincipal? identity)
  {
    Claim? claim = identity?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

    return claim?.Value;
  }

  public static string[] GetGroupIds(this ClaimsPrincipal? identity)
  {
    return identity?.Claims
        .Where(c => c.Type == ApplicationConstant.GROUP_IDS)
        .Select(c => c.Value)
        .ToArray() ?? Array.Empty<string>();
  }

  public static string? GetFullName(this ClaimsPrincipal? identity)
  {
    Claim? firstClaim = identity?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name);
    Claim? secondClaim = identity?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname);

    if (firstClaim == null || secondClaim == null)
      return null;

    return $"{firstClaim.Value} {secondClaim.Value}";
  }

  public static string? GetFirstName(this ClaimsPrincipal? identity)
  {
    Claim? claim = identity?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name);

    return claim?.Value;
  }

  public static string? GetFamily(this ClaimsPrincipal? identity)
  {
    Claim? claim = identity?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Surname);

    return claim?.Value;
  }

  public static string? GetDepartmentName(this ClaimsPrincipal? identity)
  {
    Claim? claim = identity?.Claims.SingleOrDefault(c => c.Type == ApplicationConstant.DEPARTMENT_NAME);

    return claim?.Value;
  }

  public static string? GetTitleName(this ClaimsPrincipal? identity)
  {
    Claim? claim = identity?.Claims.SingleOrDefault(c => c.Type == ApplicationConstant.TITLE);

    return claim?.Value;
  }

  public static string? GetClientId(this ClaimsPrincipal? identity)
  {
    Claim? claim = identity?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

    return claim?.Value;
  }

  /// <summary>
  /// GET System Type
  /// </summary>
  public static string? GetSystemType(this ClaimsPrincipal? identity)
  {
    Claim? claim = identity?.Claims.SingleOrDefault(c => c.Type == ClaimTypes.System);

    return claim?.Value;
  }

  /// <summary>
  /// Has Super Admin Role
  /// </summary>
  public static bool HasSuperAdminRole(this ClaimsPrincipal? identity)
  {
    if (identity?.Claims == null || !identity.Claims.Any(c => c.Type == ClaimTypes.Role))
      return false;

    foreach (var claim in identity.Claims.Where(c => c.Type == ClaimTypes.Role))
    {
      if (int.TryParse(claim.Value, out int roleId) && roleId == ApplicationConstant.SUPER_ADMIN_ROLE_ID)
      {
        return true;
      }
    }

    return false;
  }
}
