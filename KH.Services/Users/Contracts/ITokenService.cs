using KH.Dto.Models.AuthenticationDto;
using System.Security.Claims;

namespace KH.Services.Users.Contracts;

public interface ITokenService
{
  List<Claim> GetClaims(User user);
  string CreateToken(User request);
  string CreateToken(Customer customer);

  RefreshTokenResponse GenerateRefreshToken();
  ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

}
