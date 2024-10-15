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
  public async Task<ActionResult<ApiResponse<RoleResponse>>> Get(int id)
  {
    var res = await _lookupService.GetAsync(id);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Roles.LIST_ROLE)]

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<RoleListResponse>>>> GetList(RoleFilterRequest request)
  {
    var res = await _lookupService.GetListAsync(request);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Roles.ADD_ROLE)]

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] CreateRoleRequest request)
  {
    var res = await _lookupService.AddAsync(request);
    return AsActionResult(res);
  }
  [HttpPut]
  [PermissionAuthorize(PermissionKeysConstant.Roles.EDIT_ROLE)]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] CreateRoleRequest request)
  {
    var res = await _lookupService.UpdateAsync(request);
    return AsActionResult(res);
  }

  [HttpPut("UpdateBothRoleWithRelatedPermissions")]
  [PermissionAuthorize(PermissionKeysConstant.Roles.EDIT_ROLE)]
  public async Task<ActionResult<ApiResponse<string>>> UpdateBothRoleWithRelatedPermissions([FromBody] CreateRoleRequest request)
  {
    var res = await _lookupService.UpdateBothRoleWithRelatedPermissionsAsync(request);
    return AsActionResult(res);
  }
  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
  {
    var res = await _lookupService.DeleteAsync(id);
    return AsActionResult(res);
  }
}
