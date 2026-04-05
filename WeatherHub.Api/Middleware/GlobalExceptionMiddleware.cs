using WeatherHub.Application.Common.Exceptions;
using WeatherHub.Application.Common.Models;

namespace WeatherHub.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        public readonly RequestDelegate _next;
        public readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";                

                if (ex is AppException appException)
                {
                    _logger.LogWarning(ex, "Handled application error: {Message}", ex.Message);
                    context.Response.StatusCode = appException.StatusCode;

                    var response = ApiResponse<object>.Fail([ex.Message]);

                    await context.Response.WriteAsJsonAsync(response);
                } else
                {
                    _logger.LogError(ex, "Unhandled exception on {Path}", context.Request.Path);
                    context.Response.StatusCode = 500;

                    var response = ApiResponse<object>.Fail(["Internal server error"]);

                    await context.Response.WriteAsJsonAsync(response);
                }                
            }
        }
    }
}