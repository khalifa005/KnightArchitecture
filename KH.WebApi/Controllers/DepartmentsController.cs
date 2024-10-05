using KH.Dto.lookups.DepartmentDto.Form;
using KH.Dto.lookups.DepartmentDto.Response;
using KH.Dto.Lookups.DepartmentDto.Request;

namespace KH.WebApi.Controllers;

public class DepartmentsController : BaseApiController
{
  public readonly IDepartmentService _lookupService;
  public DepartmentsController(IDepartmentService lookupService)
  {
    _lookupService = lookupService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<DepartmentResponse>>> Get(int id)
  {
    var res = await _lookupService.GetAsync(id);
    return AsActionResult(res);
  }
  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<DepartmentListResponse>>>> GetList(DepartmentFilterRequest request)
  {
    var res = await _lookupService.GetListAsync(request);
    return AsActionResult(res);
  }
  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] DepartmentForm request)
  {
    var res = await _lookupService.AddAsync(request);
    return AsActionResult(res);
  }
  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] DepartmentForm request)
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

