using KH.BuildingBlocks.Apis.Extentions;
using KH.BuildingBlocks.Auth.Constant;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.RoleDto.Request;
using KH.Services.Lookups.Roles.Contracts;

namespace KH.WebApi.Controllers;

public class RolesController : BaseApiController
{
  public readonly IRoleService _lookupService;
  public RolesController(IRoleService lookupService)
  {
    _lookupService = lookupService;
  }
  [PermissionAuthorize(PermissionKeysConstant.Roles.VIEW_ROLE)]
  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<RoleResponse>>> Get(int id, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetAsync(id, cancellationToken);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Roles.LIST_ROLE)]

  [HttpPost("ListAll")]
  public async Task<ActionResult<ApiResponse<List<RoleListResponse>>>> ListAll(RoleFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetListAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("PagedList")]
  public async Task<ActionResult<ApiResponse<PagedResponse<RoleListResponse>>>> GetPagedList(RoleFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetPagedListAsync(request, cancellationToken);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Roles.ADD_ROLE)]

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.AddAsync(request, cancellationToken);
    return AsActionResult(res);
  }
  [HttpPut]
  [PermissionAuthorize(PermissionKeysConstant.Roles.EDIT_ROLE)]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.UpdateAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPut("UpdateBothRoleWithRelatedPermissions")]
  [PermissionAuthorize(PermissionKeysConstant.Roles.EDIT_ROLE)]
  public async Task<ActionResult<ApiResponse<string>>> UpdateBothRoleWithRelatedPermissions([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.UpdateBothRoleWithRelatedPermissionsAsync(request, cancellationToken);
    return AsActionResult(res);
  }
  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id, CancellationToken cancellationToken)
  {
    var res = await _lookupService.DeleteAsync(id, cancellationToken);
    return AsActionResult(res);
  }
}
