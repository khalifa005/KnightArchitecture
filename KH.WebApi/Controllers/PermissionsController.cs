using KH.BuildingBlocks.Auth.V1;
using KH.BuildingBlocks.Constant;
using KH.Dto.lookups.PermissionDto.Form;
using KH.Dto.Lookups.PermissionsDto.Response;
using Microsoft.AspNetCore.Authorization;

namespace KH.WebApi.Controllers;

public class PermissionsController : BaseApiController
{
  public readonly IPermissionService _lookupService;
  public PermissionsController(IPermissionService lookupService)
  {
    _lookupService = lookupService;
  }
  [Authorize]
  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<PermissionResponse>>> Get(int id)
  {
    var res = await _lookupService.GetAsync(id);
    return AsActionResult(res);
  }

  [HttpGet("List")]
  [PermissionAuthorize(ApplicationConstant.SUPER_ADMIN_PERMISSION)]
  public async Task<ActionResult<ApiResponse<PagedResponse<PermissionResponse>>>> GetPermissions()
  {
    var res = await _lookupService.GetListAsync();
    return AsActionResult(res);
  }

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] PermissionForm request)
  {
    var res = await _lookupService.AddAsync(request);
    return AsActionResult(res);
  }

  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] PermissionForm request)
  {
    var res = await _lookupService.UpdateAsync(request);
    return AsActionResult(res);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
  {
    var res = await _lookupService.DeleteAsync(id);
    return AsActionResult(res);
  }

}
