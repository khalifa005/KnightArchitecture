using KH.BuildingBlocks.Extentions.Methods;
using Microsoft.AspNetCore.Authorization;

namespace KH.BuildingBlocks.Auth.V1;

//1- Requirements must implement the IAuthorizationRequirement marker interface. You can pass data to it, just like I did above. In this case, we need 1 - the operator and 2 - the list of permissions. We also have the ClaimType which is always permissions.
public class PermissionRequirement : AuthorizationHandler<PermissionRequirement>, IAuthorizationRequirement
{
  // 1 - The operator
  public PermissionOperator PermissionOperator { get; }
  // 2 - The list of permissions passed
  public string[] Permissions { get; }

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

          context.Fail(new AuthorizationFailureReason(this, "Unauthorized access"));

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
    context.Fail(new AuthorizationFailureReason(this, "Unauthorized access"));
    return Task.CompletedTask;
  }
}



//extra handeler
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
  protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context, PermissionRequirement requirement)
  {
    if (requirement.PermissionOperator == PermissionOperator.And)
    {
      foreach (var permission in requirement.Permissions)
      {
        if (!context.User.
            HasClaim(PermissionRequirement.ClaimType, permission))
        {
          // If the user lacks ANY of the required permissions
          // we mark it as failed.
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
        // In the OR case, as soon as we found a matching permission
        // we can already mark it as Succeed
        context.Succeed(requirement);
        return Task.CompletedTask;
      }
    }

    // identity does not have any of the required permissions
    context.Fail();
    return Task.CompletedTask;
  }
}
