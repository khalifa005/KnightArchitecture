
namespace KH.BuildingBlocks.Apis.Middlewares;

public class ExceptionMiddleware
{
  private readonly RequestDelegate _request;
  private readonly ILogger<ExceptionMiddleware> _logger;
  private readonly IHostEnvironment _host;

  public ExceptionMiddleware(RequestDelegate request, ILogger<ExceptionMiddleware> logger, IHostEnvironment host)
  {
    _request = request;
    _logger = logger;
    _host = host;
  }

  public async Task InvokeAsync(HttpContext httpContext)
  {
    try
    {
      await _request(httpContext);

    }
    catch (Exception exception)
    {
      _logger.LogError(exception, exception.Message);

      httpContext.Response.ContentType = "application/json";
      httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

      // Prepare error details
      string errorDetails = null;
      if (_host.IsDevelopment())
      {
        errorDetails = exception.StackTrace;

        if (exception.InnerException != null)
        {
          errorDetails += $"\nInner Exception: {exception.InnerException.Message}";
        }
      }
      else
      {
        var logedErrorDetails = exception.StackTrace;

        if (exception.InnerException != null)
        {
          logedErrorDetails += $"\nInner Exception: {exception.InnerException.Message}";
        }
        _logger.LogError(exception, logedErrorDetails);
      }

      _logger.LogError(exception, errorDetails);

      var response = new ApiException((int)HttpStatusCode.InternalServerError, exception.Message, errorDetails);
      // Automatically assign error details like ErrorMessage and ErrorCode
      response.assignErrorDetails();

      var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
      var returnJson = System.Text.Json.JsonSerializer.Serialize(response, options);

      await httpContext.Response.WriteAsync(returnJson);

    }
  }
}

