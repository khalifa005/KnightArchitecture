using KH.BuildingBlocks.Extentions.Methods;
using Microsoft.AspNetCore.Authorization;

namespace KH.BuildingBlocks.Auth.V1;

public class PermissionRequirement : AuthorizationHandler<PermissionRequirement>, IAuthorizationRequirement
{
  public string[] Permissions { get; }

  public PermissionOperator PermissionOperator { get; }

  public static string ClaimType => "permissions";

  public PermissionRequirement(PermissionOperator permissionOperator, string[] permissions)
  {
    if (permissions.Length == 0)
      throw new ArgumentException("At least one permission is required.", nameof(permissions));

    PermissionOperator = permissionOperator;
    Permissions = permissions;
  }

  /// <summary>
  /// Permisson Handler Check if User Has The required Permission in his Claim
  /// </summary>
  /// <param name="context"></param>
  /// <param name="requirement"></param>
  /// <returns></returns>
  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
  PermissionRequirement requirement)
  {

    //-- If User Has Role SuperAdmin Skip Check Permission 
    if (context.User.HasSuperAdminRole())
    {
      // identity has all required permissions
      context.Succeed(requirement);
      return Task.CompletedTask;
    }

    if (requirement.PermissionOperator == PermissionOperator.And)
    {
      foreach (var permission in requirement.Permissions)
      {
        if (!context.User.HasClaim(PermissionRequirement.ClaimType, permission))
        {
          context.Fail();
          return Task.CompletedTask;
        }
      }

      // identity has all required permissions
      context.Succeed(requirement);
      return Task.CompletedTask;
    }

    foreach (var permission in requirement.Permissions)
    {
      if (context.User.HasClaim(PermissionRequirement.ClaimType, permission))
      {
        context.Succeed(requirement);
        return Task.CompletedTask;
      }
    }

    // identity does not have any of the required permissions
    context.Fail();
    return Task.CompletedTask;
  }
}
