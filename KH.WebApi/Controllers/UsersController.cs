namespace KH.WebApi.Controllers;
public class UsersController : BaseApiController
{
  public readonly IAuthenticationService _authenticationService;
  public readonly IUserManagementService _userManagementService;
  public readonly IUserQueryService _userQueryService;
  public readonly IUserValidationService _userValidationService;

  public UsersController(
  IAuthenticationService authenticationService,
  IUserManagementService userManagementService,
  IUserQueryService userQueryService,
  IUserValidationService userValidationService)
  {
    _authenticationService = authenticationService;
    _userManagementService = userManagementService;
    _userQueryService = userQueryService;
    _userValidationService = userValidationService;
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<UserDetailsResponse>>> Get(int id, CancellationToken cancellationToken)
  {
    //var res = await _userService.GetAsync(id);
    var res = await _userQueryService.GetAsync(id, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("GetUserByFilter")]
  public async Task<ActionResult<ApiResponse<UserDetailsResponse>>> GetUserByFilter(UserFilterRequest request, CancellationToken cancellationToken)
  {
    //var res = await _userService.GetAsync(request);
    var res = await _userQueryService.GetAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<UserListResponse>>>> GetList(UserFilterRequest request, CancellationToken cancellationToken)
  {
    //each willl have different implementation to atchive the same thing
    //var res = await _userService.GetListUsingIQueryableAsync(request);
    //var res = await _userService.GetListAsync(request);

    //var res = await _userService.GetListUsingProjectionAsync(request);
    var res = await _userQueryService.GetListUsingProjectionAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
  {
    //var res = await _userService.AddAsync(request);
    var res = await _userManagementService.AddAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPost("AddRange")]
  public async Task<ActionResult<ApiResponse<string>>> PostRange([FromBody] List<CreateUserRequest> request, CancellationToken cancellationToken)
  {
    //var res = await _userService.AddListAsync(request);
    var res = await _userManagementService.AddListAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
  {
    //var res = await _userService.UpdateAsync(request);
    var res = await _userManagementService.UpdateAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id, CancellationToken cancellationToken)
  {
    //var res = await _userService.DeleteAsync(id);
    var res = await _userManagementService.DeleteAsync(id, cancellationToken);
    return AsActionResult(res);
  }

  [HttpPut("ResetDepartment/{id}")]
  public async Task<ActionResult<ApiResponse<string>>> ResetDepartment(int id, CancellationToken cancellationToken)
  {
    //var res = await _userService.ResetDepartmentsAsync(id);
    var res = await _userManagementService.ResetDepartmentsAsync(id, cancellationToken);
    return AsActionResult(res);
  }
}

