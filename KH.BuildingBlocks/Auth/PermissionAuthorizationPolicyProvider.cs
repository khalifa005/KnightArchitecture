using KH.BuildingBlocks.Auth.Attributes;
using KH.BuildingBlocks.Auth.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace KH.BuildingBlocks.Auth;
//2
public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
  private readonly LockingConcurrentDictionary<string, AuthorizationPolicy> _policies =
    new LockingConcurrentDictionary<string, AuthorizationPolicy>();

  private const string PolicyPrefix = PermissionAuthorizeAttribute.PolicyPrefix;
  public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    : base(options)
  {
  }

  public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
  {
    if (!policyName.StartsWith(PolicyPrefix, StringComparison.OrdinalIgnoreCase))
    {
      // it's not one of our dynamic policies, so we fallback to the base behavior
      // this will load policies added in Startup.cs (AddPolicy..)
      return await base.GetPolicyAsync(policyName);
    }

    // create and return the policy for our requirement
    var policy = _policies.GetOrAdd(policyName, static name =>
    {
      PermissionOperatorEnum @operator = PermissionAuthorizeAttribute.GetOperatorFromPolicy(name);
      string[] permissions = PermissionAuthorizeAttribute.GetPermissionsFromPolicy(name);

      // extract the info from the policy name and create our requirement
      var requirement = new PermissionRequirement(@operator, permissions);

      return new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddRequirements(requirement)
        .Build();
    });

    return policy;
  }
}
