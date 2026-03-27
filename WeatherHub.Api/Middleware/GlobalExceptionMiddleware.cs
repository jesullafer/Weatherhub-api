using WeatherHub.Application.Common.Exceptions;

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
                var responseMessage = "Internal server error";

                if (ex is AppException appException)
                {
                    _logger.LogWarning(ex, "Handled application error: {Message}", ex.Message);
                    context.Response.StatusCode = appException.StatusCode;  
                    responseMessage = appException.Message;
                } else
                {
                    _logger.LogError(ex, "Unhandled exception on {Path}", context.Request.Path);
                    context.Response.StatusCode = 500;
                }

                await context.Response.WriteAsJsonAsync(new { message = responseMessage, statusCode = context.Response.StatusCode });
            }
        }
    }
}