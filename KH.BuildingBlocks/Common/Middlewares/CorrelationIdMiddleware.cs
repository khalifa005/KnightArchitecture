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
    // Retrieve the TraceIdentifier as the fallback correlation ID
    var requestId = context.TraceIdentifier;

    // Check if the request already has a correlation ID in the headers
    string? correlationId = context.Request.Headers.TryGetValue(CorrelationIdHeader, out var headerValue)
                            && !string.IsNullOrEmpty(headerValue)
                            ? headerValue.ToString()
                            : requestId;

    // Add the correlation ID to the response headers
    context.Response.Headers[CorrelationIdHeader] = correlationId;

    // Attach correlation ID to the request so it can be used elsewhere in the pipeline
    context.Items[CorrelationIdHeader] = correlationId;

    // Call the next middleware in the pipeline
    await _next(context);
  }
}
