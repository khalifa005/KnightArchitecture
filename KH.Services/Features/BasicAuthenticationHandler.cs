using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace KH.Services.Features;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
  private readonly IUserService _user;
  public BasicAuthenticationHandler(
    IUserService user,
    IOptionsMonitor<AuthenticationSchemeOptions> options,
                                    ILoggerFactory logger,
                                    UrlEncoder encoder,
                                    ISystemClock clock)
      : base(options, logger, encoder, clock)
  {
    _user = user;
  }

  protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
  {
    // Check for Authorization header
    if (!Request.Headers.ContainsKey("Authorization"))
    {
      return AuthenticateResult.Fail("Missing Authorization Header");
    }

    try
    {
      var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
      var credentials = Encoding.UTF8
          .GetString(Convert.FromBase64String(authHeader.Parameter))
          .Split(':', 2);

      var username = credentials[0];
      var password = credentials[1];

      var userClaims = await _user.GetUserClaims(new LoginRequest
      {
        Username = username,
        Password = password
      });


      if (userClaims.Count > 0)
      {
        var identity = new ClaimsIdentity(userClaims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);

      }
      else
      {
        return AuthenticateResult.Fail("Invalid Username or Password");
      }
    }
    catch
    {
      return AuthenticateResult.Fail("Invalid Authorization Header");
    }
  }

  private bool IsValidUser(string username, string password)
  {
    // Implement your own user validation logic
    // This could be querying a database, checking a hardcoded list, etc.
    return username == "admin" && password == "password"; // Example hardcoded user
  }
}

