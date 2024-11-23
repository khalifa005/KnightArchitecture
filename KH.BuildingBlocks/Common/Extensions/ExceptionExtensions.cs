using KH.BuildingBlocks.Apis.Responses;
using System.Runtime.CompilerServices;

namespace KH.BuildingBlocks.Apis.Extentions;

public static class ExceptionExtensions
{
  public static ApiResponse<T> HandleException<T>(
    this Exception ex,
    ApiResponse<T> res,
    IHostEnvironment env,
    ILogger logger,
    [CallerMemberName] string context = "") where T : class
  {
    // Log the exception details for both environments with additional context
    logger.LogError(ex, $"Exception occurred in {context}: {ex.Message}");
    logger.LogError($"StackTrace in {context}: {ex.StackTrace}");
    if (ex.InnerException != null)
    {
        
    }

    // Set the default status code and error messages
    res.StatusCode = (int)HttpStatusCode.InternalServerError;
    res.ErrorMessage = ex.Message;
    res.ErrorCode = "ERO_005"; // Internal Server Error Code

    // Include detailed error information in development environment only
    if (env.IsDevelopment())
    {
      // Optionally include the stack trace for debugging purposes
      res.Errors.Add($"StackTrace: {ex.StackTrace}");

      if (ex.InnerException != null)
      {
        res.Errors.Add($"Inner Exception: {ex.InnerException.Message}");
      }
    }
    else
    {
      // Call assignErrorDetails to automatically set the appropriate error messages and codes
      res.assignErrorDetails();
    }

    return res;
  }
}
