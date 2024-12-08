
using System.Diagnostics;

namespace KH.BuildingBlocks.Apis.Middlewares;

public class ResponseTimeLoggingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ResponseTimeLoggingMiddleware> _logger;
  private readonly int _thresholdMilliseconds;

  public ResponseTimeLoggingMiddleware(RequestDelegate next, ILogger<ResponseTimeLoggingMiddleware> logger, IConfiguration configuration)
  {
    _next = next;
    _logger = logger;

    // Allow the threshold to be configurable through app settings or environment variables
    _thresholdMilliseconds = configuration.GetValue<int>("ResponseTimeThreshold", 5000); // Default: 5 seconds
  }

  public async Task InvokeAsync(HttpContext context)
  {
    // Start measuring time
    var stopwatch = Stopwatch.StartNew();

    // Proceed to the next middleware in the pipeline
    await _next(context);

    // Stop measuring time
    stopwatch.Stop();

    // Log requests exceeding the threshold
    if (stopwatch.ElapsedMilliseconds > _thresholdMilliseconds)
    {
      //send emails to it 

      _logger.LogWarning("Request [{Method}] at [{Path}] took [{ElapsedMilliseconds}] ms (Threshold: {Threshold} ms). Response Status: {StatusCode}",
          context.Request.Method,
          context.Request.Path,
          stopwatch.ElapsedMilliseconds,
          _thresholdMilliseconds,
          context.Response?.StatusCode);
    }
  }
}
