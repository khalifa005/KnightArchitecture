using System.Security.Claims;

namespace KH.Services.Users.Contracts;
public interface IAuthenticationService
{
  Task<ApiResponse<AuthenticationResponse>> LoginAsync(LoginRequest request);
  Task<ApiResponse<AuthenticationResponse>> RefreshUserTokenAsync(RefreshTokenRequest refreshTokenRequest);
  Task<List<Claim>> GetUserClaims(LoginRequest request);
}
