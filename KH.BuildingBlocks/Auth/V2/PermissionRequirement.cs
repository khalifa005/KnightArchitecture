using KH.BuildingBlocks.Auth.V1;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KH.BuildingBlocks.Auth.V2;
public class PermissionRequirement : IAuthorizationRequirement
{
  // 1 - The operator
  public PermissionOperator PermissionOperator { get; }

  // 2 - The list of permissions passed
  public string[] Permissions { get; }
  public static string ClaimType => "permissions";

  public PermissionRequirement(
      PermissionOperator permissionOperator, string[] permissions)
  {
    if (permissions.Length == 0)
      throw new ArgumentException("At least one permission is required.", nameof(permissions));

    PermissionOperator = permissionOperator;
    Permissions = permissions;
  }
}


