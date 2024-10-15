using KH.BuildingBlocks.Auth;
using KH.BuildingBlocks.Auth.Constant;
using KH.BuildingBlocks.Auth.Contracts;
using KH.BuildingBlocks.Auth.Midilleware;
using System.Security.Claims;

namespace KH.Services.Users.Implementation;
public class UserPermissionService : IUserPermissionService
{
  private readonly IUnitOfWork _unitOfWork;
  public UserPermissionService(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  //??
  public IUnitOfWork UnitOfWork() => _unitOfWork;

  public async ValueTask<ClaimsIdentity?> GetUserPermissionsIdentity(
    int userId, string? systemType, CancellationToken cancellationToken)
  {
    var userPermissions = new List<Claim>();
    if (systemType == SystemTypeEnum.ExternalCustomer.ToString())
    {
      userPermissions = new List<Claim> { new Claim(PermissionRequirement.ClaimType, PermissionKeysConstant.CUSTOMER_SYSTEM_PERMISSION) };
    }
    else
    {
      //we can get user roles from token + we can get the system function from cache
      var userRoles = await _unitOfWork.Repository<UserRole>().FindByAsync(x => x.UserId == userId && x.IsDeleted == false);

      var dbRolesFunctions = await _unitOfWork
        .Repository<RolePermissions>()
        .FindByAsync(x => userRoles.Select(o => o.RoleId).Contains(x.RoleId),
        q => q.Include(u => u.Permission));

      userPermissions =
         (from perm in dbRolesFunctions
          select new Claim(PermissionRequirement.ClaimType, perm.Permission?.Key ?? "")).ToList();
    }
    return CreatePermissionsIdentity(userPermissions);
  }

  private static ClaimsIdentity? CreatePermissionsIdentity(IReadOnlyCollection<Claim> claimPermissions)
  {
    if (!claimPermissions.Any())
      return null;

    var permissionsIdentity = new ClaimsIdentity(nameof(PermissionsMiddleware), "name", "role");
    permissionsIdentity.AddClaims(claimPermissions);

    return permissionsIdentity;
  }
}

