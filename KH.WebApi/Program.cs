using KH.Services.Lookups.Roles.RoleHub;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using QuestPDF.Fluent;
using Serilog;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogging(builder);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
ConfigureMiddlewares(app);

app.Run();

/// <summary>
/// Configures Serilog as the logging provider.
/// </summary>
void ConfigureLogging(WebApplicationBuilder builder)
{
  try
  {
    Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(builder.Configuration) // Reads settings from appsettings.json
      .Enrich.FromLogContext()
      .CreateLogger();
    builder.Host.UseSerilog();
  }
  catch
  {
    // Handle any issues with database connectivity or proceed without Serilog
  }
  
}

/// <summary>
/// Configures all the services required for the application.
/// </summary>
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{

  // Add HttpClient and Custom Http Request service
  services.AddHttpClient();
  services.AddTransient<CustomHttpRequestService>();

  // Add Controllers
  services.AddCustomControllers();
  // Add SignalR service
  services.AddSignalR();

  services.AddRateLimiter(options => {

    //f your application is running behind a reverse proxy, you need to make sure not to rate limit the proxy IP address.Reverse proxies usually
    //forward the original IP address with the X - Forwarded - For header.So you can use it as the partition key:
    //httpContext.Request.Headers["X-Forwarded-For"].ToString(),
    options.AddPolicy("fixed-by-ip", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
              PermitLimit = 10,
              Window = TimeSpan.FromMinutes(1)
            }));

    options.AddPolicy("LoginRateLimit", context =>
    {
      // Limit to 5 requests per minute per IP
      return RateLimitPartition.GetTokenBucketLimiter(
          partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
          factory: partition => new TokenBucketRateLimiterOptions
          {
            TokenLimit = 3, // Number of requests allowed
            ReplenishmentPeriod = TimeSpan.FromMinutes(1), // Time interval for replenishment
            TokensPerPeriod = 5, // Number of tokens replenished
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0 // No queuing of additional requests
          });
    });

    options.OnRejected = async (context, cancellationToken) =>
    {
      var customErrorResponse = new ApiResponse<object>(StatusCodes.Status429TooManyRequests)
      {
        ErrorMessage = $"Too many requests. {context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"}. Please try again after 1 minute.",
      };

      context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
      context.HttpContext.Response.ContentType = "application/json";

      var jsonResponse = System.Text.Json.JsonSerializer.Serialize(customErrorResponse);

      await context.HttpContext.Response.WriteAsync(jsonResponse, cancellationToken);
    };

    options.RejectionStatusCode = 429;

  });




  // Add Memory Cache
  services.AddMemoryCache();
  builder.Services.AddStackExchangeRedisCache(options =>
  {
    options.Configuration = "localhost";
    options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
    {
      AbortOnConnectFail = true,
      EndPoints = { options.Configuration }
    };
  });

  // Add CORS policy
  services.AddCustomCors(configuration);

  // Add application-specific services
  services.AddBuildingBlocksServices(configuration);
  services.AddDtoService(configuration);
  services.AddInfrastructureService(configuration);
  services.AddBusinessService(configuration);
}

/// <summary>
/// Configures all the middlewares required for the application.
/// </summary>
void ConfigureMiddlewares(WebApplication app)
{
  System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;

  var env = app.Environment;
  var configuration = app.Configuration;

  // Enable CORS and Authentication
  //app.UseCors(policy =>
  //    policy.WithOrigins("http://localhost:4200") // Replace with your Angular app's URL
  //          .AllowAnyHeader()
  //          .AllowAnyMethod());

  app.UseCors("CorsPolicy");
  app.UseHttpsRedirection();

  //app.UseMiddleware<ResponseTimeLoggingMiddleware>();


  // Status Code Pages
  app.UseStatusCodePagesWithReExecute("/errors/{0}");
  app.UseHangfireMiddleware(configuration);

  // Custom Middlewares
  app.UseInfrastructureMiddleware(configuration);
  app.UseMiddleware<ExceptionMiddleware>();

  // Swagger Documentation
  app.UseSwaggerDocumentationMiddleware(configuration);

  // HSTS for non-development environments
  if (!env.IsDevelopment())
  {
    app.UseHsts();
  }


  app.UseRouting();
  app.UseAuthentication();

  //app.UseRateLimiter(new RateLimiterOptions
  //{
  //  GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
  //  {
  //    var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

  //    return RateLimitPartition.GetFixedWindowLimiter(
  //        partitionKey: ipAddress, // Partition by IP address
  //        factory: _ => new FixedWindowRateLimiterOptions
  //        {
  //          PermitLimit = 3, // Max 3 requests
  //          Window = TimeSpan.FromSeconds(20), // Per 20 seconds
  //          QueueLimit = 2, // Allow up to 2 requests to queue
  //          QueueProcessingOrder = QueueProcessingOrder.OldestFirst
  //        });
  //  }),
  //  RejectionStatusCode = 429, // Status code for rate-limiting rejection
  //  OnRejected = async (context, cancellationToken) =>
  //  {
  //    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
  //    await context.HttpContext.Response.WriteAsync("Too many requests globally. Please try again later.", cancellationToken);
  //  }
  //});

  app.UseMiddleware<PermissionsMiddleware>();
  app.UseRateLimiter(); // This must be here

  app.UseAuthorization();
  app.MapHub<RolesHub>("/signalrhub");

  // Serve static files and map controllers
  app.UseStaticFiles();


  app.MapControllers();
  app.UseSerilogRequestLogging(); // Enable Serilog request logging
}
