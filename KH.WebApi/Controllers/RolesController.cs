using KH.BuildingBlocks.Apis.Extentions;
using KH.BuildingBlocks.Auth.Constant;
using KH.Dto.lookups.RoleDto.Response;
using KH.Dto.Lookups.RoleDto.Request;
using KH.Services.Lookups.Roles.Contracts;
using KH.Services.Lookups.Roles.RoleHub;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;

namespace KH.WebApi.Controllers;

public class RolesController : BaseApiController
{
  private readonly IHubContext<RolesHub> _hubContext;
  public readonly IRoleService _lookupService;
  private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

  public RolesController(IRoleService lookupService, IHubContext<RolesHub> hubContext)
  {
    _lookupService = lookupService;
    _hubContext = hubContext;
  }
  [PermissionAuthorize(PermissionKeysConstant.Roles.VIEW_ROLE)]
  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<RoleResponse>>> Get(int id, CancellationToken cancellationToken)
  {
    // Use the GetCurrentVersion method to retrieve the version
    var version = AssemblyExtensions.GetCurrentVersion();

    var res = await _lookupService.GetAsync(id, cancellationToken);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Roles.LIST_ROLE)]

  [HttpPost("ListAll")]
  public async Task<ActionResult<ApiResponse<List<RoleResponse>>>> ListAll(RoleFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetListAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("PagedList")]
  public async Task<ActionResult<ApiResponse<PagedList<RoleListResponse>>>> GetPagedList(RoleFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetPagedListAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("PagedListx")]
  public async Task<ActionResult<ApiResponse<PagedList<RoleListResponse>>>> GetPagedListx(RoleFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.GetPagedListAsyncx(request, cancellationToken);
    return AsActionResult(res);
  }
  [PermissionAuthorize(PermissionKeysConstant.Roles.ADD_ROLE)]

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
  {
    //await _semaphore.WaitAsync(); // Wait until the semaphore is available

    try
    {
      var res = await _lookupService.AddAsync(request, cancellationToken);

      // Notify clients about the new role
      await _hubContext.Clients.All.SendAsync("RoleAdded", res.Data);

      return AsActionResult(res);
    }
    finally
    {
      //_semaphore.Release(); // Release the semaphore for the next request
    }
  }

  [HttpPut]
  [PermissionAuthorize(PermissionKeysConstant.Roles.EDIT_ROLE)]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.UpdateAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPut("ReActivate/{id}")]

  [PermissionAuthorize(PermissionKeysConstant.Roles.EDIT_ROLE)]
  public async Task<ActionResult<ApiResponse<string>>> ReActivate([FromRoute] long id, CancellationToken cancellationToken)
  {
    var res = await _lookupService.ReActivateAsync(id, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPut("UpdateBothRoleWithRelatedPermissions")]
  [PermissionAuthorize(PermissionKeysConstant.Roles.EDIT_ROLE)]
  public async Task<ActionResult<ApiResponse<string>>> UpdateBothRoleWithRelatedPermissions([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.UpdateBothRoleWithRelatedPermissionsAsync(request, cancellationToken);
    return AsActionResult(res);
  }


  [HttpPut("UpdateRolePermissions", Name = "UpdateRolePermissions")]
  [PermissionAuthorize(PermissionKeysConstant.Roles.EDIT_ROLE)]
  public async Task<ActionResult<ApiResponse<string>>> UpdateRolePermissionsAsync([FromBody] UpdatedRolePermissionsRequest request, CancellationToken cancellationToken)
  {
    var res = await _lookupService.UpdateRolePermissionsAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  /// <summary>
  /// Delete a role by its ID.
  /// </summary>
  /// <param name="id">The ID of the role.</param>
  /// <returns>The role id.</returns>

  [HttpDelete("{id}", Name = "DeleteRoleById")]
  [Consumes("application/json")]
  [Produces("application/json")]
  //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<RoleResponse>))]

  public async Task<ActionResult<ApiResponse<string>>> Delete(int id, CancellationToken cancellationToken)
  {
    var res = await _lookupService.DeleteAsync(id, cancellationToken);
    return AsActionResult(res);
  }
}
