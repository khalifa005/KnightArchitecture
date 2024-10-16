using KH.BuildingBlocks.Apis.Extentions;
using KH.Dto.lookups.GroupDto.Request;
using KH.Dto.lookups.GroupDto.Response;
using KH.Dto.Lookups.GroupDto.Request;
using KH.Services.Lookups.Groups.Contracts;

namespace KH.WebApi.Controllers;

public class GroupsController : BaseApiController
{
  public readonly IGroupService _lookupService;
  public GroupsController(IGroupService lookupService)
  {
    _lookupService = lookupService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<GroupResponse>>> Get(int id, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetAsync(id, cancellationToken);
    return AsActionResult(res);
  }
  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<GroupListResponse>>>> GetList(GroupFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetListAsync(request, cancellationToken);
    return AsActionResult(res);
  }
  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] CreateGroupRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.AddAsync(request, cancellationToken);
    return AsActionResult(res);
  }
  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] CreateGroupRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.UpdateAsync(request, cancellationToken);
    return AsActionResult(res);
  }
  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id, CancellationToken cancellationToken)
  {
    var res = await _lookupService.DeleteAsync(id, cancellationToken);
    return AsActionResult(res);
  }
}

