using KH.BuildingBlocks.Constant;
using KH.BuildingBlocks.Contracts.Infrastructure;
using KH.BuildingBlocks.Extentions.Methods;
using KH.BuildingBlocks.Responses;
using Newtonsoft.Json;
using System.Security.Claims;

namespace KH.BuildingBlocks.Auth.V1;

/// <summary>
/// In CASE USER IsAuthenticated 
/// Collect his Permissions from (DB/Caching) and add to Identity Claims With Type [permissions]
/// </summary>
public class PermissionsMiddleware
{
  private readonly RequestDelegate _request;
  private readonly ILogger<PermissionsMiddleware> _logger;
  //private IResponseCacheService _cache;
  public PermissionsMiddleware(
    RequestDelegate request,
    ILogger<PermissionsMiddleware> logger)
  {
    _request = request;
    _logger = logger;
    //_cache = cache;
  }


  public async Task InvokeAsync(HttpContext context, IUserPermissionService permissionService)
  {
    if (context.User.Identity == null || !context.User.Identity.IsAuthenticated)
    {
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      context.Response.ContentType = "application/json";

      var apiResponse = new ApiResponse<object>(StatusCodes.Status401Unauthorized)
      {
        ErrorMessage = "Unauthorized access",
        ErrorMessageAr = "وصول غير مصرح به", // Example Arabic error message, customize as needed
        Errors = new List<string> { "User is not authenticated" }
      };

      var jsonResponse = JsonConvert.SerializeObject(apiResponse); // Assuming you're using Newtonsoft.Json or System.Text.Json
      await context.Response.WriteAsync(jsonResponse);//, cancellationToken
      return;
    }


    var cancellationToken = context.RequestAborted;
    var permissionsIdentity = new ClaimsIdentity(nameof(PermissionsMiddleware), "name", "role");

    if (context.User.HasSuperAdminRole())
    {
      //--Add Default Permissions In case User System admin
      permissionsIdentity.AddClaim(new Claim(PermissionRequirement.ClaimType, ApplicationConstant.SUPER_ADMIN_PERMISSION));
    }
    else
    {
      var userId = context.User.GetId();
      if (string.IsNullOrEmpty(userId))
      {
        await context.Response.WriteAsync("User 'Identifier' claim is required",
        cancellationToken);
        return;
      }

      var systemType = context.User.GetSystemType();
      var userRoles = context.User.GetUserRole();

      permissionsIdentity = await permissionService.GetUserPermissionsIdentity(int.Parse(userId), systemType, cancellationToken);
      if (permissionsIdentity == null)
      {
        _logger.LogWarning("User {Identifier} does not have permissions", userId);
        await context.Response.WriteAsync("Access denied",
        cancellationToken);
        return;
      }
    }
    // User has permissions, so we add the extra identity containing the "permissions" claims
    context.User.AddIdentity(permissionsIdentity);

    //-- TODO: Remove Comment To Allow Redis Caching
    /*
    var responseCached = await _cache.GetCachedResponseAsync($"Permissions_{userId}");
    var permissionsIdentity = new ClaimsIdentity();
    if (responseCached == null)
    {
      permissionsIdentity = await permissionService.GetUserPermissionsIdentity(int.Parse(userId), systemType, cancellationToken);
      if (permissionsIdentity == null)
      {
        _logger.LogWarning("User {Identifier} does not have permissions", userId);
        await context.Response.WriteAsync("Access denied",
        cancellationToken);
        return;
      }
      await _cache.CacheResponseAsync($"Permissions_{userId}", permissionsIdentity, TimeSpan.FromSeconds(500000));
    }
    else
    {
      permissionsIdentity = JsonConvert.DeserializeObject<ClaimsIdentity>(responseCached, new ClaimConverter());
    }
    if (permissionsIdentity != null)
    {
      // User has permissions, so we add the extra identity containing the "permissions" claims
      context.User.AddIdentity(permissionsIdentity);
    }
    */

    await _request(context);
  }
}
