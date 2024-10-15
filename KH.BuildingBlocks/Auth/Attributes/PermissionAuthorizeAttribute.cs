

namespace KH.BuildingBlocks.Auth.Attributes;

//3
public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
  public const string PolicyPrefix = "PERMISSION_";
  private const string Separator = "_";

  /// <summary>
  /// Initializes the attribute with multiple permissions
  /// </summary>
  /// <param name="permissionOperator">The operator to use when verifying the permissions provided</param>
  /// <param name="permissions">The list of permissions</param>
  public PermissionAuthorizeAttribute(PermissionOperatorEnum permissionOperator, params string[] permissions)
  {
    // E.g: PERMISSION_1_Create_Update..
    Policy = $"{PolicyPrefix}{(int)permissionOperator}{Separator}{string.Join(Separator, permissions)}";
  }

  /// <summary>
  /// Initializes the attribute with a single permission
  /// </summary>
  /// <param name="permission">The permission</param>
  public PermissionAuthorizeAttribute(string permission)
  {
    // E.g: PERMISSION_1_Create..
    Policy = $"{PolicyPrefix}{(int)PermissionOperatorEnum.And}{Separator}{permission}";
  }

  /// <summary>
  /// GET Permissions Operator [AND / OR] From policy Name
  /// </summary>
  /// <param name="policyName"></param>
  /// <returns></returns>
  public static PermissionOperatorEnum GetOperatorFromPolicy(string policyName)
  {
    var @operator = int.Parse(policyName.AsSpan(PolicyPrefix.Length, 1));
    return (PermissionOperatorEnum)@operator;
  }

  /// <summary>
  /// GET Permissions From Policy Name
  /// </summary>
  /// <param name="policyName"></param>
  /// <returns></returns>
  public static string[] GetPermissionsFromPolicy(string policyName)
  {
    return policyName.Substring(PolicyPrefix.Length + 2)
      .Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
  }

}
