using KH.BuildingBlocks.Apis.Extentions;
using KH.BuildingBlocks.Auth.Constant;
using KH.Dto.lookups.DepartmentDto.Response;
using KH.Dto.Lookups.DepartmentDto.Request;
using KH.Services.Lookups.Departments.Contracts;

namespace KH.WebApi.Controllers;

public class DepartmentsController : BaseApiController
{
  public readonly IDepartmentService _lookupService;
  public DepartmentsController(IDepartmentService lookupService)
  {
    _lookupService = lookupService;
  }
  [PermissionAuthorize(PermissionKeysConstant.Departments.VIEW_DEPARTMENT)]

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<DepartmentResponse>>> Get(int id)
  {
    var res = await _lookupService.GetAsync(id);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Departments.LIST_DEPARTMENTS)]

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<DepartmentListResponse>>>> GetList(DepartmentFilterRequest request)
  {
    var res = await _lookupService.GetListAsync(request);
    return AsActionResult(res);
  }

  [PermissionAuthorize(PermissionKeysConstant.Departments.ADD_DEPARTMENT)]

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] CreateDepartmentRequest request)
  {
    var res = await _lookupService.AddAsync(request);
    return AsActionResult(res);
  }

  [PermissionAuthorize(PermissionKeysConstant.Departments.EDIT_DEPARTMENT)]
  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] CreateDepartmentRequest request)
  {
    var res = await _lookupService.UpdateAsync(request);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Departments.DELETE_DEPARTMENT)]

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
  {
    var res = await _lookupService.DeleteAsync(id);
    return AsActionResult(res);
  }
}

