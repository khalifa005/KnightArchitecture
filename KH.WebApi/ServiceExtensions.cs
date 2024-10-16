namespace KH.Services;

public static class ServiceExtensions
{
  /// <summary>
  /// Adds CORS policy to the service collection.
  /// </summary>
  public static IServiceCollection AddCustomCors(this IServiceCollection services)
  {
    services.AddCors(opt =>
    {
      opt.AddPolicy("CorsPolicy", policy =>
      {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
      });
    });

    return services;
  }

  /// <summary>
  /// Adds controller services with custom configuration for FluentValidation and API behavior.
  /// </summary>
  public static IServiceCollection AddCustomControllers(this IServiceCollection services)
  {
    services.AddControllers(options =>
    {
      // Disable ASP.NET Core's default model validation
      // options.ModelValidatorProviders.Clear();
    })
    .ConfigureApiBehaviorOptions(options =>
    {
      options.InvalidModelStateResponseFactory = context =>
      {
        var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

        var detailedErrors = context.ModelState.SelectMany(x => x.Value.Errors.Select(e => e.Exception?.Message ?? e.ErrorMessage)).ToList();
        Console.WriteLine("ModelState Errors: " + string.Join(", ", detailedErrors));

        var customErrorResponse = new ApiResponse<object>(400)
        {
          ErrorMessage = "validation-failed.",
          Errors = errors.SelectMany(x => x.Value).ToList()
        };

        return new BadRequestObjectResult(customErrorResponse);
      };
    })
    .AddJsonOptions(options =>
    {
      options.JsonSerializerOptions.PropertyNamingPolicy = null;
      options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    })
    .AddNewtonsoftJson(options =>
    {
      options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

    return services;
  }
}
