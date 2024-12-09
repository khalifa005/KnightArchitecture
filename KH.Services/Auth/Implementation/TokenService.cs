using KH.Dto.Models.AuthenticationDto;
using KH.Services.Auth.Contracts;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace KH.Services.Auth.Implementation;

public class TokenService : ITokenService
{
  private readonly TokenSettings _tokenSettings;
  private readonly SymmetricSecurityKey _key;

  public TokenService(
      IOptions<TokenSettings> tokenSettings)
  {
    _tokenSettings = tokenSettings.Value;
    _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key));
  }

  public List<Claim> GetClaims(User user)
  {

    var userRoles = user.UserRoles?
        .Select(o => o.RoleId.ToString()) // Directly select RoleId as string
        .ToList();

    var claims = new List<Claim>
            {
               new Claim(ClaimTypes.System , SystemTypeEnum.InternalAdmin.ToString()),
               new Claim(ClaimTypes.NameIdentifier ,$"{user.Id}"),
               new Claim(ClaimTypes.Name,$"{user.FirstName}"),
               new Claim(ClaimTypes.Surname,$"{user.LastName}"),
               new Claim(ClaimTypes.Email,$"{user.Email}"),
               new Claim(ClaimTypes.MobilePhone,$"{user.MobileNumber}")
            };

    claims.AddRange(userRoles.Select(roleId => new Claim(ClaimTypes.Role, roleId)));

    return claims;
  }

  public string CreateToken(User user)
  {

    var claims = GetClaims(user);

    //var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
    var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      //claims: new List<Claim>(),
      Expires = DateTime.Now.AddDays(1),
      SigningCredentials = creds,
      Issuer = _tokenSettings.Issuer
      //audience: "https://localhost:5001",
    };

    var tokenHandler = new JwtSecurityTokenHandler();

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
  }

  public string CreateToken(Customer customer)
  {

    var claims = new List<Claim>
            {
               new Claim(ClaimTypes.Role , "ExternalUser"),
               new Claim(ClaimTypes.System , SystemTypeEnum.ExternalCustomer.ToString()),
               new Claim(ClaimTypes.NameIdentifier ,$"{customer.Id}"),
               new Claim(ClaimTypes.Name,$"{customer.FirstName}"),
               new Claim(ClaimTypes.Surname,$"{customer.LastName}"),
               new Claim(ClaimTypes.Email,$"{customer.Email}"),
               new Claim(ClaimTypes.GivenName,$"{customer.MobileNumber}")
            };

    var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.Now.AddDays(1),
      SigningCredentials = creds,
      Issuer = _tokenSettings.Issuer
    };

    var tokenHandler = new JwtSecurityTokenHandler();

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
  }


  public RefreshTokenResponse GenerateRefreshToken()
  {
    var randomNumber = new byte[32];
    using (var generator = RandomNumberGenerator.Create())
    {
      generator.GetBytes(randomNumber);
      return new RefreshTokenResponse
      {
        Token = Convert.ToBase64String(randomNumber),
        Expires = DateTime.UtcNow.AddDays(10),
        Created = DateTime.UtcNow
      };
    }
  }

  public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
  {
    var tokenValidationParameters = new TokenValidationParameters
    {
      ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
      ValidateIssuer = true,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = _key,
      ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    SecurityToken securityToken;
    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
    var jwtSecurityToken = securityToken as JwtSecurityToken;
    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
      throw new SecurityTokenException("Invalid token");
    return principal;
  }

}
