using KH.BuildingBlocks.Responses;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace KH.PersistenceInfra;

public static class IdentityServiceExtention
{
  public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
  {
    var key = Encoding.UTF8.GetBytes(configuration["TokenSettings:Key"]);

    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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

              ErrorMessage = "Token expired."
            });
            context.NoResult(); // Stop further processing
            return context.Response.WriteAsync(result);
          }

          context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
          context.Response.ContentType = "application/json";
          var errorResult = JsonConvert.SerializeObject(new ApiResponse<object>(StatusCodes.Status401Unauthorized)
          {
            ErrorMessage = "Authentication faild xx."
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
            ErrorMessage = "You are not authorized to access this resource."
          });
          context.NoResult(); // Stop further processing
          return context.Response.WriteAsync(result);
        },

        // Handle missing or invalid token
        OnChallenge = context =>
        {
          if (string.IsNullOrEmpty(context.Request.Headers["Authorization"]))
          {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new ApiResponse<object>(StatusCodes.Status401Unauthorized)
            {
              ErrorMessage = "No token provided."
            });
            return context.Response.WriteAsync(result);
          }

          return Task.CompletedTask;
        }
      };
    });

    return services;
  }

}
