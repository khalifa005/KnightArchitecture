using System.Security.Claims;
using System.Threading;

namespace KH.Services.Auth.Contracts;
public interface IAuthenticationService
{
  Task<ApiResponse<AuthenticationResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
  Task<ApiResponse<AuthenticationResponse>> RefreshUserTokenAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken);
  Task<List<Claim>> GetUserClaimsAsync(LoginRequest request);
}
