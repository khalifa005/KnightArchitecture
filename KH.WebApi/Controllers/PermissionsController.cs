using KH.BuildingBlocks.Apis.Extentions;
using KH.BuildingBlocks.Auth.Constant;
using KH.Dto.Lookups.PermissionsDto.Request;
using KH.Dto.Lookups.PermissionsDto.Response;
using KH.Services.Lookups.Permissions.Contracts;

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
  public async Task<ActionResult<ApiResponse<PermissionResponse>>> Get(int id, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetAsync(id, cancellationToken);
    return AsActionResult(res);
  }

  [HttpGet("ListAll")]
  [PermissionAuthorize(PermissionKeysConstant.PermissionManagement.LIST_PERMISSIONS)]
  public async Task<ActionResult<ApiResponse<List<PermissionResponse>>>> ListAll(CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetListAsync(cancellationToken);
    return AsActionResult(res);
  }

  [PermissionAuthorize(PermissionKeysConstant.PermissionManagement.ADD_PERMISSION)]

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] CreatePermissionRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.AddAsync(request, cancellationToken);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.PermissionManagement.EDIT_PERMISSION)]

  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] CreatePermissionRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.UpdateAsync(request, cancellationToken);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.PermissionManagement.DELETE_PERMISSION)]

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id, CancellationToken cancellationToken)
  {
    var res = await _lookupService.DeleteAsync(id, cancellationToken);
    return AsActionResult(res);
  }

}
