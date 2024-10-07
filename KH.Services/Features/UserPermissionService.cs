using KH.BuildingBlocks.Auth.V1;
using KH.BuildingBlocks.Constant;
using KH.BuildingBlocks.Enums;
using System.Security.Claims;

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
      //--Add Default Permissions In case System Is CRMClient
      userPermissions = new List<Claim> { new Claim(PermissionRequirement.ClaimType, ApplicationConstant.CUSTOMER_SYSTEM_PERMISSION) };
    }
    else
    {
      //we can get user roles from token + we can get the system function from cache
      var userRoles = await _unitOfWork.Repository<UserRole>().FindByAsync(x => x.UserId == userId);

      var dbRolesFunctions = await _unitOfWork
        .Repository<RolePermissions>()
        .FindByAsync(x => userRoles.Select(o => o.RoleId).Contains(x.RoleId),
        q => q.Include(u => u.Permission));

      userPermissions =
         (from perm in dbRolesFunctions
          select new Claim(PermissionRequirement.ClaimType, perm.Permission?.NameEn ?? "")).ToList();
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
