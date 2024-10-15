using KH.BuildingBlocks.Apis.Extentions;
using KH.BuildingBlocks.Auth.Constant;
using KH.Dto.lookups.PermissionDto.Form;
using KH.Dto.Lookups.PermissionsDto.Response;

namespace KH.WebApi.Controllers;

public class PermissionsController : BaseApiController
{
  public readonly IPermissionService _lookupService;
  public PermissionsController(IPermissionService lookupService)
  {
    _lookupService = lookupService;
  }
  [PermissionAuthorize(PermissionKeysConstant.PermissionManagement.VIEW_PERMISSION)]

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<PermissionResponse>>> Get(int id)
  {
    var res = await _lookupService.GetAsync(id);
    return AsActionResult(res);
  }

  [HttpGet("List")]
  [PermissionAuthorize(PermissionKeysConstant.PermissionManagement.LIST_PERMISSIONS)]
  public async Task<ActionResult<ApiResponse<PagedResponse<PermissionResponse>>>> GetPermissions()
  {
    var res = await _lookupService.GetListAsync();
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.PermissionManagement.ADD_PERMISSION)]

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] PermissionForm request)
  {
    var res = await _lookupService.AddAsync(request);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.PermissionManagement.EDIT_PERMISSION)]

  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] PermissionForm request)
  {
    var res = await _lookupService.UpdateAsync(request);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.PermissionManagement.DELETE_PERMISSION)]

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
  {
    var res = await _lookupService.DeleteAsync(id);
    return AsActionResult(res);
  }

}
