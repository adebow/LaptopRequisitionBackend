using System.Net;
using System.Text.Json;

namespace LaptopRequisition.WebAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred. TraceId: {TraceId}", context.TraceIdentifier);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
                throw exception;

            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),

                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized access."),

                KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),

                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),

                _ => (HttpStatusCode.InternalServerError,
                      "An unexpected error occurred. Please try again later.")
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message,
                traceId = context.TraceIdentifier,
                
                details = _env.IsDevelopment() ? exception.StackTrace : null
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}