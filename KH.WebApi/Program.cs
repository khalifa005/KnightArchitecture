using QuestPDF.Fluent;
using Serilog;

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
  services.AddSignalR();

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
  app.UseMiddleware<PermissionsMiddleware>();
  app.UseAuthorization();

  // Serve static files and map controllers
  app.UseStaticFiles();
  app.MapControllers();
  app.UseSerilogRequestLogging(); // Enable Serilog request logging
}
