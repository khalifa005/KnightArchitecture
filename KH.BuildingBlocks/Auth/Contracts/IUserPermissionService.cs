
namespace KH.BuildingBlocks.Auth.Contracts;

public interface IUserPermissionService
{
  /// <summary>
  /// Returns a new identity containing the user permissions as Claims
  /// </summary>
  /// <param name="userId">The user external id (sub claim)</param>
  /// <param name="cancellationToken"></param>
  ValueTask<ClaimsIdentity?> GetUserPermissionsIdentity(long userId, string? systemType, CancellationToken cancellationToken);
}
