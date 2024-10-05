using System.Security.Claims;

namespace KH.Services.Contracts;

public interface ITokenService
{
  List<Claim> GetClaims(User user);
  string CreateToken(User request);
  string CreateToken(Customer customer);
}
