using KH.BuildingBlocks.Apis.Extentions;
using KH.Dto.lookups.GroupDto.Request;
using KH.Dto.lookups.GroupDto.Response;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.GroupDto.Request;
using KH.Dto.Lookups.RoleDto.Request;
using KH.Services.Lookups.Groups.Contracts;

namespace KH.WebApi.Controllers;

public class GroupsController(IGroupService lookupService) : BaseApiController
{

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<GroupResponse>>> Get(long id, CancellationToken cancellationToken)
  {
    var res = await lookupService.GetAsync(id, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("ListAll")]
  public async Task<ActionResult<ApiResponse<List<GroupListResponse>>>> ListAll(GroupFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await lookupService.GetListAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("PagedList")]
  public async Task<ActionResult<ApiResponse<PagedResponse<GroupListResponse>>>> GetPagedList(GroupFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await lookupService.GetPagedListAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] CreateGroupRequest request, CancellationToken cancellationToken)
  {
    var res = await lookupService.AddAsync(request, cancellationToken);
    return AsActionResult(res);
  }
  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] CreateGroupRequest request, CancellationToken cancellationToken)
  {
    var res = await lookupService.UpdateAsync(request, cancellationToken);
    return AsActionResult(res);
  }
  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id, CancellationToken cancellationToken)
  {
    var res = await lookupService.DeleteAsync(id, cancellationToken);
    return AsActionResult(res);
  }
}

