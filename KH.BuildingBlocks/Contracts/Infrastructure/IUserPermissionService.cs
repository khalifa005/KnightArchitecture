using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
