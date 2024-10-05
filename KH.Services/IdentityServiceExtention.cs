using AngleSharp.Css.Values;
using KH.BuildingBlocks.Auth.V1;
using KH.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PuppeteerSharp;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace KH.Services;

public static class IdentityServiceExtention
{
  public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IUserPermissionService, UserPermissionService>();
    services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();


    var key = Encoding.UTF8.GetBytes(configuration["TokenSettings:Key"]);

    services.AddAuthentication(options =>
    {
      //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultScheme = "BasicOrJwt";  // Set a policy scheme to handle both

      options.DefaultChallengeScheme = "BasicOrJwt";
    })
      .AddPolicyScheme("BasicOrJwt", "JWT or Basic", options =>
       {
         options.ForwardDefaultSelector = context =>
         {
           var authHeader = context.Request.Headers["Authorization"].ToString();

           var endpoint = context.GetEndpoint();
           var allowAnonymous = endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null;

           // Use a key to track if the authentication scheme has already been selected for this request
           const string schemeSelectedKey = "SchemeSelected";

           
           // Otherwise, select the appropriate scheme based on the Authorization header
           if (authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
           {
             // Mark the scheme as selected
             context.Items[schemeSelectedKey] = true;
             return "BasicAuthentication";
           }

           // Default to JWT Bearer
           context.Items[schemeSelectedKey] = true;
           return JwtBearerDefaults.AuthenticationScheme;
         };
       })
    .AddJwtBearer(bearer =>
    {
      bearer.RequireHttpsMetadata = false;
      bearer.SaveToken = true;
      bearer.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = configuration["TokenSettings:Issuer"],
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
      };

      bearer.Events = new JwtBearerEvents
      {
        // If token is passed through query string (for SignalR)
        OnMessageReceived = context =>
        {
          var accessToken = context.Request.Query["access_token"];
          var path = context.HttpContext.Request.Path;
          if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/signalrhub"))
          {
            context.Token = accessToken;
          }
          return Task.CompletedTask;
        },

        // Handle failed authentication
        OnAuthenticationFailed = context =>
        {
          if (context.Exception is SecurityTokenExpiredException)
          {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new ApiResponse<object>(StatusCodes.Status401Unauthorized)
            {

              ErrorMessage = "Token expired.",
              ErrorMessageAr = "Token expired."
            });
            context.NoResult(); // Stop further processing
            return context.Response.WriteAsync(result);
          }

          context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
          context.Response.ContentType = "application/json";
          var errorResult = JsonConvert.SerializeObject(new ApiResponse<object>(StatusCodes.Status401Unauthorized)
          {
            ErrorMessage = "Authentication faild",
            ErrorMessageAr = "Authentication faild"
          });
          context.NoResult(); // Stop further processing
          return context.Response.WriteAsync(errorResult);
        },

        // Handle forbidden access (when user is authenticated but lacks proper authorization)
        OnForbidden = context =>
        {
          context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
          context.Response.ContentType = "application/json";
          var result = JsonConvert.SerializeObject(new ApiResponse<object>(StatusCodes.Status403Forbidden)
          {
            ErrorMessage = "You are not authorized to access this resource.",
            ErrorMessageAr = "You are not authorized to access this resource."
          });
          context.NoResult(); // Stop further processing
          return context.Response.WriteAsync(result);
        },

        // Handle missing or invalid token
        OnChallenge = context =>
        {
          //DefaultChallengeScheme is used for prompting the user for credentials when authentication is required but not present.

          if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
              {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new ApiResponse<object>(StatusCodes.Status401Unauthorized)
            {
              ErrorMessage = "No token provided.",
              ErrorMessageAr = "No token provided."
            });
            return context.Response.WriteAsync(result);
          }

          return Task.CompletedTask;
        }
      };
    }).AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null); // Adding Basic Authentication

    return services;
  }

}

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
  public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                    ILoggerFactory logger,
                                    UrlEncoder encoder,
                                    ISystemClock clock)
      : base(options, logger, encoder, clock)
  {
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

      // Validate credentials (e.g., against a user store)
      if (IsValidUser(username, password))
      {
        var claims = new[] {
          new Claim(ClaimTypes.Name, username),
          new Claim(ClaimTypes.NameIdentifier, "26")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
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

