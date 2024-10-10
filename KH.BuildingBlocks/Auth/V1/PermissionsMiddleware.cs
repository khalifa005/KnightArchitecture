using KH.BuildingBlocks.Constant;
using KH.BuildingBlocks.Extentions.Methods;
using KH.BuildingBlocks.Responses;
using Microsoft.AspNetCore.Authorization;
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
    // Check if the endpoint has AllowAnonymous applied
    var endpoint = context.GetEndpoint();
    if (endpoint != null && endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
    {
      // If AllowAnonymous is found, bypass the authentication and permission checks
      await _request(context);
      return;
    }


    if (context.User.Identity == null || !context.User.Identity.IsAuthenticated || context.Response.StatusCode == StatusCodes.Status403Forbidden)
    {
      if (context.Response.HasStarted)
      {
        // Log and exit early if the response has already started
        _logger.LogWarning("The response has already started.");
        return;
      }

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
        var apiResponse = new ApiResponse<object>(StatusCodes.Status401Unauthorized)
        {
          ErrorMessage = "access-denied",
          ErrorMessageAr = "وصول غير مصرح به", // Example Arabic error message, customize as needed
          Errors = new List<string> { "invalid-user-id" }
        };

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var jsonResponse = JsonConvert.SerializeObject(apiResponse); // Assuming you're using Newtonsoft.Json or System.Text.Json
        await context.Response.WriteAsync(jsonResponse, cancellationToken);
        return;
      }

      var systemType = context.User.GetSystemType();
      var userRole = context.User.GetUserRole();
      var userRoles = context.User.GetUserRoles();

      permissionsIdentity = await permissionService.GetUserPermissionsIdentity(int.Parse(userId), systemType, cancellationToken);
      if (permissionsIdentity == null)
      {
        var apiResponse = new ApiResponse<object>(StatusCodes.Status401Unauthorized)
        {
          ErrorMessage = "access-denied",
          ErrorMessageAr = "وصول غير مصرح به", // Example Arabic error message, customize as needed
          Errors = new List<string> { "User is not authenticated" }
        };

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
        var jsonResponse = JsonConvert.SerializeObject(apiResponse); // Assuming you're using Newtonsoft.Json or System.Text.Json
        await context.Response.WriteAsync(jsonResponse, cancellationToken);
        return;
      }
    }
    // User has permissions, so we add the extra identity containing the "permissions" claims
    context.User.AddIdentity(permissionsIdentity);

    await _request(context);
  }
}

