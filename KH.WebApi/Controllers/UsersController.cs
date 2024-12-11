using KH.Dto.Lookups.PermissionsDto.Response;
using KH.Services.Auth.Contracts;

namespace KH.WebApi.Controllers;

public class UsersController(
    IAuthenticationService authenticationService,
    IUserManagementService userManagementService,
    IUserQueryService userQueryService,
    IUserValidationService userValidationService) : BaseApiController
{
  // Action methods

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<UserDetailsResponse>>> Get(int id, CancellationToken cancellationToken)
  {
    var res = await userQueryService.GetAsync(id, cancellationToken);
    return AsActionResult(res);
  }



  [HttpPost("GetUserByFilter")]
  public async Task<ActionResult<ApiResponse<UserDetailsResponse>>> GetUserByFilter(UserFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await userQueryService.GetAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<UserListResponse>>>> GetList(UserFilterRequest request, CancellationToken cancellationToken)
  {
    var res = await userQueryService.GetListUsingProjectionAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
  {
    var res = await userManagementService.AddAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("AddRange")]
  public async Task<ActionResult<ApiResponse<string>>> PostRange([FromBody] List<CreateUserRequest> request, CancellationToken cancellationToken)
  {
    var res = await userManagementService.AddListAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
  {
    var res = await userManagementService.UpdateAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id, CancellationToken cancellationToken)
  {
    var res = await userManagementService.DeleteAsync(id, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPut("ResetDepartment/{id}")]
  public async Task<ActionResult<ApiResponse<string>>> ResetDepartment(int id, CancellationToken cancellationToken)
  {
    var res = await userManagementService.ResetDepartmentsAsync(id, cancellationToken);
    return AsActionResult(res);
  }



  [HttpGet("GetUserPermissions")]
  public async Task<ActionResult<ApiResponse<List<PermissionResponse>>>> GetUserPermissions(CancellationToken cancellationToken)
  {
    var res = await userQueryService.GetUserPermissionsListAsync(cancellationToken);
    return AsActionResult(res);
  }

}
