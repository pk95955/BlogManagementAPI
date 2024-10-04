// Middleware/ExceptionHandlingMiddleware.cs
using BlogManagementAPI.CustomException;
using System.Net;
using System.Text.Json;

namespace BlogManagementAPI.CustomMiddleware
{
    public class CustomExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlingMiddleware> _logger;
        public CustomExceptionHandlingMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Proceed to the next middleware/component
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }

        }
        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //response details as per exception type
            HttpStatusCode status;
            string message;
            string? details = null;

            switch (ex)
            {
                case BadHttpRequestException _:
                    status = HttpStatusCode.BadRequest;
                    message = "Bad request";
                    // log ex,Message
                    break;
                case ArgumentNullException _:
                case ArgumentException _:
                    status = HttpStatusCode.BadRequest;
                    message = ex.Message;
                    break;
                case UnauthorizedAccessException _:
                    status = HttpStatusCode.Unauthorized;
                    message = "Authentication is required and has failed or has not yet been provided.";
                    break;
                case ForbiddenException _:
                    status = HttpStatusCode.Forbidden;
                    message = ex.Message;
                    break;
                case NotFoundException _:
                    status = HttpStatusCode.NotFound;
                    message = ex.Message;
                    break;
                case MethodNotAllowedException _:
                    status = HttpStatusCode.MethodNotAllowed;
                    message = ex.Message;
                    break;
                case UnsupportedMediaTypeException _:
                    status = HttpStatusCode.UnsupportedMediaType;
                    message = ex.Message;
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred.";
                    break;
            }

            // include stack trace or detailed info in development
            if (context.RequestServices.GetService(typeof(IHostEnvironment)) is IHostEnvironment env && env.IsDevelopment())
            {
                details = ex.StackTrace;
            }

            var expDetails = new
            {
                status = (int)status,
                title = message,
                detail = details
            };

            var result = JsonSerializer.Serialize(expDetails);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(result);
        }
    }
}
