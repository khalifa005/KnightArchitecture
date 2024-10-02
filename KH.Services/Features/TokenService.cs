using KH.BuildingBlocks.Enums;
using KH.BuildingBlocks.Settings;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Linq;
using Newtonsoft.Json;
namespace KH.Services.Features;

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

  public string CreateToken(User user)
  {

    var userRoles = user.UserRoles?
        .Where(o =>
            o.RoleId == UserExtensions.SUPER_ADMIN_ROLE_ID ||
            (o.RolePermissions != null && o.RolePermissions.Count > 0))
        .Select(o => o.RoleId.ToString()) // Directly select RoleId as string
        .ToList();

    var claims = new List<Claim>
            {
               new Claim(ClaimTypes.System , SystemTypeEnum.InternalAdmin.ToString()),
               new Claim(ClaimTypes.NameIdentifier ,$"{user.Id}"),
               new Claim(ClaimTypes.Name,$"{user.FirstName}"),
               new Claim(ClaimTypes.Surname,$"{user.LastName}"),
               new Claim(ClaimTypes.Email,$"{user.Email}"),
               new Claim(ClaimTypes.GivenName,$"{user.MobileNumber}")
            };

    claims.AddRange(userRoles.Select(roleId => new Claim(ClaimTypes.Role, roleId)));

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



}
