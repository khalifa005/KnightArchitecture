namespace KH.WebApi.Controllers;

public class AuthenticationController : BaseApiController
{
  public readonly IAuthenticationService _authenticationService;

  public AuthenticationController(IAuthenticationService authenticationService)
  {
    _authenticationService = authenticationService;
  }

  [AllowAnonymous]
  [HttpPost("Login")]
  public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> Login(LoginRequest request)
  {
    //var res = await _userService.LoginAsync(request);
    var res = await _authenticationService.LoginAsync(request);
    return AsActionResult(res);
  }

  [AllowAnonymous]
  [HttpPost("RefreshUserToken")]
  public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> RefreshUserToken(RefreshTokenRequest refreshTokenRequest)
  {
    //var res = await _userService.RefreshUserTokenAsync(refreshToken);
    var res = await _authenticationService.RefreshUserTokenAsync(refreshTokenRequest);
    return AsActionResult(res);
  }

 
}

