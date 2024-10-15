namespace KH.BuildingBlocks.Apis.Middlewares;

public class CancellationTokenMiddleware
{
  private readonly RequestDelegate _next;

  public CancellationTokenMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext context)
  {
    var cancellationToken = context.RequestAborted;

    try
    {
      await _next(context); // Pass the token to the next middleware
    }
    catch (OperationCanceledException)
    {
      // Optionally, log or handle cancellation exception
      context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest; // 499: Client Closed Request
    }
  }
}
