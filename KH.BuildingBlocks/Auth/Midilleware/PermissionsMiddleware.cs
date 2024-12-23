using Serilog.Context;

namespace KH.BuildingBlocks.Auth.Midilleware;
public class PermissionsMiddleware
{
  private readonly RequestDelegate _request;
  private readonly ILogger<PermissionsMiddleware> _logger;
  public PermissionsMiddleware(
    RequestDelegate request,
    ILogger<PermissionsMiddleware> logger)
  {
    _request = request;
    _logger = logger;
  }


  public async Task InvokeAsync(HttpContext context, IUserPermissionService permissionService)
  {
    var cancellationToken = context.RequestAborted;

    // Check if the endpoint has AllowAnonymous applied
    var endpoint = context.GetEndpoint();
    if (endpoint != null && endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
    {
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
        ErrorMessageAr = "وصول غير مصرح به",
        Errors = new List<string> { "User is not authenticated" }
      };

      var jsonResponse = JsonConvert.SerializeObject(apiResponse);
      await context.Response.WriteAsync(jsonResponse, cancellationToken: cancellationToken);
      return;
    }

    var permissionsIdentity = new ClaimsIdentity(nameof(PermissionsMiddleware), "name", "role");

    if (context.User.HasSuperAdminRole())
    {
      //--Add Default Permissions In case User System admin
      permissionsIdentity.AddClaim(new Claim(PermissionRequirement.ClaimType, PermissionKeysConstant.SUPER_ADMIN_PERMISSION));
    }
    else
    {
      var userId = context.User.GetId();
      if (string.IsNullOrEmpty(userId))
      {
        var apiResponse = new ApiResponse<object>(StatusCodes.Status401Unauthorized)
        {
          ErrorMessage = "access-denied",
          ErrorMessageAr = "وصول غير مصرح به",
          Errors = new List<string> { "invalid-user-id" }
        };

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var jsonResponse = JsonConvert.SerializeObject(apiResponse);
        await context.Response.WriteAsync(jsonResponse, cancellationToken);
        return;
      }

      var systemType = context.User.GetSystemType();

      permissionsIdentity = await permissionService.GetUserPermissionsIdentity(long.Parse(userId), systemType, cancellationToken);
      if (permissionsIdentity == null)
      {
        var apiResponse = new ApiResponse<object>(StatusCodes.Status401Unauthorized)
        {
          ErrorMessage = "access-denied",
          ErrorMessageAr = "وصول غير مصرح به",
          Errors = new List<string> { "user-is-not-authorize" }
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

    // Log the UserId with Serilog's LogContext for this request
    using (LogContext.PushProperty("UserId", context.User.GetId()))
    {
      await _request(context);
    }
  }
}

