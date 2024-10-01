using KH.Dto.Models.AuthenticationDto.Request;
using KH.Dto.Models.AuthenticationDto.Response;

namespace KH.WebApi.Controllers;
public class UsersController : BaseApiController
{
  public readonly IUserService _userService;
  public UsersController(IUserService userService)
  {
    _userService = userService;
  }

  [HttpPost("Login")]
  public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> Login(LoginRequest request)
  {
    var res = await _userService.LoginAsync(request);
    return AsActionResult(res);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<ApiResponse<UserDetailsResponse>>> Get(int id)
  {
    var res = await _userService.GetAsync(id);
    return AsActionResult(res);
  }

  [HttpPost("GetUserByFilter")]
  public async Task<ActionResult<ApiResponse<UserDetailsResponse>>> GetUserByFilter(UserFilterRequest request)
  {
    var res = await _userService.GetAsync(request);
    return AsActionResult(res);
  }

  [HttpPost("list")]
  public async Task<ActionResult<ApiResponse<PagedResponse<UserListResponse>>>> GetList(UserFilterRequest request)
  {
    //each willl have different implementation to atchive the same thing
    //var res = await _userService.GetListUsingIQueryableAsync(request);
    //var res = await _userService.GetListAsync(request);
    var res = await _userService.GetListUsingProjectionAsync(request);
    return AsActionResult(res);
  }

  [HttpPost]
  public async Task<ActionResult<ApiResponse<string>>> Post([FromBody] UserForm request)
  {
    var res = await _userService.AddAsync(request);
    return AsActionResult(res);
  }

  [HttpPost("AddRange")]
  public async Task<ActionResult<ApiResponse<string>>> PostRange([FromBody] List<UserForm> request)
  {
    var res = await _userService.AddListAsync(request);
    return AsActionResult(res);
  }

  [HttpPut]
  public async Task<ActionResult<ApiResponse<string>>> Put([FromBody] UserForm request)
  {
    var res = await _userService.UpdateAsync(request);
    return AsActionResult(res);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
  {
    var res = await _userService.DeleteAsync(id);
    return AsActionResult(res);
  }

  [HttpPut("ResetDepartment/{id}")]
  public async Task<ActionResult<ApiResponse<string>>> ResetDepartment(int id)
  {
    var res = await _userService.ResetDepartmentsAsync(id);
    return AsActionResult(res);
  }
}

