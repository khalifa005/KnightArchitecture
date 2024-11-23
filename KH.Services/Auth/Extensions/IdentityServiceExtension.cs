using KH.BuildingBlocks.Auth;
using KH.BuildingBlocks.Auth.Contracts;
using KH.Services.Auth.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Text;

namespace KH.Services.Auth.Extensions;

public static class IdentityServiceExtension
{
  public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddScoped<IUserPermissionService, UserPermissionService>();
    services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

    //services.AddAuthorization(options =>
    //{
    //  // One static policy - All users must be authenticated
    //  options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
    //      .RequireAuthenticatedUser()
    //      .Build();
    //});
    services.AddAuthorization();

    services.AddAuthentication(options =>
    {
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenSettings:Key"])),

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
    })
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null); // Adding Basic Authentication

    return services;
  }

}
