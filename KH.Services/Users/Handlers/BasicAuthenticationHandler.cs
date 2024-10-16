using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using KH.Services.Users.Contracts;


namespace KH.Services.Users.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
  private readonly Contracts.IAuthenticationService _user;
  public BasicAuthenticationHandler(
    Contracts.IAuthenticationService user,
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

      var userClaims = await _user.GetUserClaimsAsync(new LoginRequest
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

  protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
  {
    Response.StatusCode = StatusCodes.Status401Unauthorized;
    Response.ContentType = "application/json";
    var result = JsonConvert.SerializeObject(new ApiResponse<object>(StatusCodes.Status401Unauthorized)
    {
      ErrorMessage = "Invalid Basic authentication credentials.",
      ErrorMessageAr = "بيانات اعتماد غير صالحة للمصادقة الأساسية."
    });
    await Response.WriteAsync(result);
  }
}

