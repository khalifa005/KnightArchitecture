using KH.BuildingBlocks.Auth.V1;
using KH.BuildingBlocks.Constant;
using KH.Dto.lookups.RoleDto.Form;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.PermissionsDto.Response;
using KH.Dto.Lookups.RoleDto.Request;
using Microsoft.AspNetCore.Authorization;

namespace KH.WebApi.Controllers;

public class RolesController : BaseApiController
{
  public readonly IRoleService _lookupService;
  public RolesController(IRoleService lookupService)
  {
    _lookupService = lookupService;
  }
  [Authorize]
  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<RoleResponse>>> Get(int id)
  {
    var res = await _lookupService.GetAsync(id);
    return AsActionResult(res);
  }
  //[Authorize]
  [AllowAnonymous]
  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<RoleListResponse>>>> GetList(RoleFilterRequest request)
  {
    var res = await _lookupService.GetListAsync(request);
    return AsActionResult(res);
  }
  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] RoleForm request)
  {
    var res = await _lookupService.AddAsync(request);
    return AsActionResult(res);
  }
  [HttpPut]
  [PermissionAuthorize(PermissionKeysConstant.Roles.EDIT_ROLE)]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] RoleForm request)
  {
    var res = await _lookupService.UpdateAsync(request);
    return AsActionResult(res);
  }

  [HttpPut("UpdateBothRoleWithRelatedPermissions")]
  [PermissionAuthorize(PermissionKeysConstant.Roles.EDIT_ROLE)]
  public async Task<ActionResult<ApiResponse<string>>> UpdateBothRoleWithRelatedPermissions([FromBody] RoleForm request)
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
