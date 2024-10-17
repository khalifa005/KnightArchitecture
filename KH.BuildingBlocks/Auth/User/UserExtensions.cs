
namespace KH.BuildingBlocks.Auth.User;

public static class UserExtensions
{
  public const int SUPER_ADMIN_ROLE_ID = (int)RoleEnum.SuperAdmin;
  public const string departmentIdClaim = "departmentId";
  public const string isManagerClaim = "is_manager";
  public const string engineeringOfficeID = "engineeringOfficeID";
  public const string groupids = "groupsPaths";
  public const string preferred_username = "preferred_username";
  public const string clientId = "azp";
  public const string departmentName = "departmentName";
  public const string title = "title";

  /*public static List<string> GetPropsValues()
  {

      return null;
  }*/

  public static int? GetDepartmentId(this ClaimsPrincipal identity)
  {
    Claim claim = identity?.FindFirst(departmentIdClaim);

    if (claim == null)
      return null;

    return int.Parse(claim.Value);
  }

  public static bool IsManager(this ClaimsPrincipal identity)
  {
    Claim claim = identity?.FindFirst(isManagerClaim);

    if (claim == null)
      return false;

    return bool.Parse(claim.Value);
  }

  public static long? GetUserId(this IServiceProvider serviceProvider)
  {
    var context = serviceProvider.GetService<IHttpContextAccessor>();
    Claim claim = context.HttpContext?.User?.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).SingleOrDefault();

    if (claim == null)
      return null;

    return int.Parse(claim.Value);
  }

  public static string GetId(this ClaimsPrincipal identity)
  {
    Claim claim = identity?.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).SingleOrDefault();

    if (claim == null)
      return null;

    return claim.Value;
  }

  public static string[] GetGroupIds(this ClaimsPrincipal identity)
  {
    return identity?.Claims.Where(c => c.Type == groupids).Select(c => c.Value).ToArray();
  }

  public static int? GetEngineeringOfficeID(this ClaimsPrincipal identity)
  {
    Claim claim = identity?.FindFirst(engineeringOfficeID);

    if (claim == null)
      return null;

    return int.Parse(claim.Value);
  }

  public static string GetFullName(this ClaimsPrincipal identity)
  {
    Claim firstClaim = identity?.Claims.Where(c => c.Type == ClaimTypes.Name).SingleOrDefault();
    if (firstClaim == null)
      return null;

    Claim secondClaim = identity?.Claims.Where(c => c.Type == ClaimTypes.Surname).SingleOrDefault();
    if (secondClaim == null)
      return null;


    return firstClaim.Value + " " + secondClaim.Value;
  }

  public static string GetFristName(this ClaimsPrincipal identity)
  {
    Claim claim = identity?.Claims.Where(c => c.Type == ClaimTypes.Name).SingleOrDefault();

    if (claim == null)
      return null;

    return claim.Value;
  }

  public static string GetFamily(this ClaimsPrincipal identity)
  {
    Claim claim = identity?.Claims.Where(c => c.Type == ClaimTypes.Surname).SingleOrDefault();

    if (claim == null)
      return null;

    return claim.Value;
  }

  public static string GetDepartmentName(this ClaimsPrincipal identity)
  {
    Claim claim = identity?.Claims.Where(c => c.Type == departmentName).SingleOrDefault();

    if (claim == null)
      return null;

    return claim.Value;
  }

  public static string GetTitleName(this ClaimsPrincipal identity)
  {
    Claim claim = identity?.Claims.Where(c => c.Type == title).SingleOrDefault();

    if (claim == null)
      return null;

    return claim.Value;
  }

  public static string GetClientId(this ClaimsPrincipal identity)
  {
    Claim claim = identity?.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).SingleOrDefault();

    if (claim == null)
      return null;

    return claim.Value;
  }


  /// <summary>
  /// GET System Type
  /// </summary>
  /// <param name="identity"></param>
  /// <returns></returns>
  public static string GetSystemType(this ClaimsPrincipal identity)
  {
    Claim claim = identity?.Claims.Where(c => c.Type == ClaimTypes.System).SingleOrDefault();

    if (claim == null)
      return null;

    return claim.Value;
  }


  /// <summary>
  /// Has Super Admin Role
  /// </summary>
  /// <param name="identity"></param>
  /// <returns></returns>
  public static bool HasSuperAdminRole(this ClaimsPrincipal identity)
  {
    // Check if the identity has any role claims
    if (identity?.Claims == null || !identity.Claims.Any(c => c.Type == ClaimTypes.Role))
      return false;

    // Iterate over all role claims and check if any match the SUPER_ADMIN_ROLE_ID
    foreach (var claim in identity.Claims.Where(c => c.Type == ClaimTypes.Role))
    {
      if (int.TryParse(claim.Value, out int roleId) && roleId == SUPER_ADMIN_ROLE_ID)
      {
        return true; // Found the super admin role
      }
    }

    return false; // No matching role found
  }

}

