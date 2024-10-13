using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace KH.BuildingBlocks.Auth.V2;

public enum PermissionOperator
{
  And = 1, Or = 2
}
public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
  internal const string PolicyPrefix = "PERMISSION_";
  private const string Separator = "_";

  public PermissionAuthorizeAttribute(
      PermissionOperator permissionOperator, params string[] permissions)
  {
    // E.g: PERMISSION_1_Create_Update..
    Policy = $"{PolicyPrefix}{(int)permissionOperator}{Separator}{string.Join(Separator, permissions)}";
  }

  public PermissionAuthorizeAttribute(string permission)
  {
    // E.g: PERMISSION_1_Create..
    Policy = $"{PolicyPrefix}{(int)PermissionOperator.And}{Separator}{permission}";
  }

  public static PermissionOperator GetOperatorFromPolicy(string policyName)
  {
    var @operator = int.Parse(policyName.AsSpan(PolicyPrefix.Length, 1));
    return (PermissionOperator)@operator;
  }

  public static string[] GetPermissionsFromPolicy(string policyName)
  {
    return policyName.Substring(PolicyPrefix.Length + 2)
        .Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
  }
}

//At startup
//services.AddAuthorization(options =>
//{
//    // One static policy - All users must be authenticated
//    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
//        .RequireAuthenticatedUser()
//        .Build();

//// A static policy from our previous post. This still works!
//options.AddPolicy("Over18YearsOld", policy => policy.RequireAssertion(context =>
//        context.User.HasClaim(c =>
//            (c.Type == "DateOfBirth" && DateTime.Now.Year - DateTime.Parse(c.Value).Year >= 18)
//        )));
//});

//// Register our custom Authorization handler
//services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

//// Overrides the DefaultAuthorizationPolicyProvider with our own
//services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
