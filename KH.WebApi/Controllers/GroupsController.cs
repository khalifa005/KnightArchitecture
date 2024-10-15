using KH.BuildingBlocks.Apis.Extentions;
using KH.Dto.lookups.GroupDto.Form;
using KH.Dto.lookups.GroupDto.Request;
using KH.Dto.lookups.GroupDto.Response;

namespace KH.WebApi.Controllers;

public class GroupsController : BaseApiController
{
  public readonly IGroupService _lookupService;
  public GroupsController(IGroupService lookupService)
  {
    _lookupService = lookupService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<GroupResponse>>> Get(int id)
  {
    var res = await _lookupService.GetAsync(id);
    return AsActionResult(res);
  }
  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<GroupListResponse>>>> GetList(GroupFilterRequest request)
  {
    var res = await _lookupService.GetListAsync(request);
    return AsActionResult(res);
  }
  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] GroupForm request)
  {
    var res = await _lookupService.AddAsync(request);
    return AsActionResult(res);
  }
  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] GroupForm request)
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

