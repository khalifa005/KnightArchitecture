using KH.BuildingBlocks.Apis.Extentions;
using KH.BuildingBlocks.Auth.Constant;
using KH.Dto.lookups.DepartmentDto.Response;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.DepartmentDto.Request;
using KH.Dto.Lookups.RoleDto.Request;
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
  public async Task<ActionResult<ApiResponse<DepartmentResponse>>> Get(int id, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetAsync(id, cancellationToken);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Departments.LIST_DEPARTMENTS)]

  [HttpPost("ListAll")]
  public async Task<ActionResult<ApiResponse<List<DepartmentListResponse>>>> ListAll(DepartmentFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetListAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("PagedList")]
  [PermissionAuthorize(PermissionKeysConstant.Departments.LIST_DEPARTMENTS)]

  public async Task<ActionResult<ApiResponse<PagedResponse<DepartmentListResponse>>>> GetPagedList(DepartmentFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetPagedListAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [PermissionAuthorize(PermissionKeysConstant.Departments.ADD_DEPARTMENT)]

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] CreateDepartmentRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.AddAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [PermissionAuthorize(PermissionKeysConstant.Departments.EDIT_DEPARTMENT)]
  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] CreateDepartmentRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.UpdateAsync(request, cancellationToken);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Departments.DELETE_DEPARTMENT)]

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id, CancellationToken cancellationToken)
  {
    var res = await _lookupService.DeleteAsync(id, cancellationToken);
    return AsActionResult(res);
  }
}

