using KH.Services.Auth.Contracts;
using Microsoft.AspNetCore.RateLimiting;

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
  [EnableRateLimiting("LoginRateLimit")]

  public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> Login(LoginRequest request, CancellationToken cancellationToken)
  {
    //var res = await _userService.LoginAsync(request);
    var res = await _authenticationService.LoginAsync(request, cancellationToken);
    return AsActionResult(res);
  }

  [AllowAnonymous]
  [HttpPost("RefreshUserToken")]
  public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> RefreshUserToken(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
  {
    //var res = await _userService.RefreshUserTokenAsync(refreshToken);
    var res = await _authenticationService.RefreshUserTokenAsync(refreshTokenRequest,cancellationToken);
    return AsActionResult(res);
  }

 
}

