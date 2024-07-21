using KH.Helper.Responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;


namespace KH.Helper.Middlewares
{
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

                var errorDetails = exception.StackTrace.ToString();

                _logger.LogError(exception, errorDetails);

                var response = new ApiException((int)HttpStatusCode.InternalServerError, exception.Message, errorDetails);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var returnJson = JsonSerializer.Serialize(response, options);

                await httpContext.Response.WriteAsync(returnJson);

            }
        }
    }


  public class BaseException : Exception
  {
    public HttpStatusCode StatusCode { get; }
    public BaseException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
      StatusCode = statusCode;
    }
  }


}
