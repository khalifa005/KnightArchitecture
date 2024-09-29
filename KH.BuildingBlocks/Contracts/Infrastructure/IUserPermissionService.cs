using System.Security.Claims;

namespace KH.BuildingBlocks.Contracts.Infrastructure
{
  public interface IUserPermissionService
  {
    /// <summary>
    /// Returns a new identity containing the user permissions as Claims
    /// </summary>
    /// <param name="userId">The user external id (sub claim)</param>
    /// <param name="cancellationToken"></param>
    ValueTask<ClaimsIdentity?> GetUserPermissionsIdentity(int userId, string? systemType, CancellationToken cancellationToken);
  }
}
