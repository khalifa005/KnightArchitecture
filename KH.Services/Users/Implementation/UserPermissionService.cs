using KH.BuildingBlocks.Auth;
using KH.BuildingBlocks.Auth.Constant;
using KH.BuildingBlocks.Auth.Contracts;
using KH.BuildingBlocks.Auth.Midilleware;
using System.Security.Claims;

namespace KH.Services.Users.Implementation;
public class UserPermissionService : IUserPermissionService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly ICurrentUserService _currentUserService;
  public UserPermissionService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
  {
    _unitOfWork = unitOfWork;
    _currentUserService = currentUserService;
  }

  public IUnitOfWork UnitOfWork() => _unitOfWork;

  public async ValueTask<ClaimsIdentity?> GetUserPermissionsIdentity(
    long userId, string? systemType, CancellationToken cancellationToken)
  {
    var userPermissions = new List<Claim>();
    if (systemType == SystemTypeEnum.ExternalCustomer.ToString())
    {
      userPermissions = new List<Claim> { new Claim(PermissionRequirement.ClaimType, PermissionKeysConstant.CUSTOMER_SYSTEM_PERMISSION) };
    }
    else
    {
      //get user roles from token + permissions from cache
      var userRolesFromToken = _currentUserService.RolesIds;
      var userIdFromToken = _currentUserService.UserId;

      var repository = _unitOfWork.Repository<Role>();

      //form cache
      var allAppRolesWithPermissionsListResult = await repository.GetAllAsync(
      include: query =>
      query.Include(r => r.RolePermissions)
      .ThenInclude(p => p.Permission),
      tracking: false,
      useCache: true,
      cancellationToken: cancellationToken);

      var userRelatedRoles = allAppRolesWithPermissionsListResult
        .Where(r => userRolesFromToken.Select(roleId => Convert.ToInt64(roleId))
        .Contains(r.Id))
        .ToList();

      var userRelatedRolesPermissions = userRelatedRoles
        .SelectMany(x => x.RolePermissions)
        .ToList();

      userPermissions =
         (from perm in userRelatedRolesPermissions
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

