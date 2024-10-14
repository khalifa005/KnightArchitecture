using KH.BuildingBlocks.Apis.Constant;

namespace KH.BuildingBlocks.Apis.Middlewares;

public class CorrelationIdMiddleware
{
  private const string CorrelationIdHeader = ApplicationConstant.X_Correlation_ID;
  private readonly RequestDelegate _next;

  public CorrelationIdMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    // Check if request has a correlation ID, otherwise generate one
    if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
    {
      correlationId = Guid.NewGuid().ToString();
      context.Request.Headers[CorrelationIdHeader] = correlationId;
    }

    // Add the correlation ID to the response headers
    context.Response.Headers.Add(CorrelationIdHeader, correlationId);

    // Attach correlation ID to the request so it can be used elsewhere in the pipeline
    context.Items[CorrelationIdHeader] = correlationId;

    // Call the next middleware in the pipeline
    await _next(context);
  }
}

