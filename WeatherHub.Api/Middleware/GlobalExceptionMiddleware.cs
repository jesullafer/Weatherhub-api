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
                var message = "Internal server error";

                if (ex is AppException appException)
                {
                    context.Response.StatusCode = appException.StatusCode;  
                    message = appException.Message;
                } else
                {
                    context.Response.StatusCode = 500;
                }

                await context.Response.WriteAsJsonAsync(new { message = message, statusCode = context.Response.StatusCode });
            }
        }
    }
}