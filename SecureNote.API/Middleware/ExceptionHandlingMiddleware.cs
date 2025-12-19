using System.Net;
using System.Text.Json;
using SecureNote.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace SecureNote.API.Middleware
{
    /// <summary>
    /// Tüm istisnalarýný yakalayýp uygun HTTP yanýtý dönen middleware
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(ex, "Ýstisnalar yakalandý: {ExceptionMessage}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Message = exception.Message,
                Success = false
            };

            switch (exception)
            {
                // 404 Not Found
                case NotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response.ErrorCode = "NOT_FOUND";
                    break;

                // 403 Forbidden
                case UnauthorizedException:
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    response.ErrorCode = "UNAUTHORIZED";
                    break;

                // 400 Bad Request
                case ValidationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorCode = "VALIDATION_ERROR";
                    break;

                // 400 Bad Request (Genel Application Exception)
                case AppException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.ErrorCode = "APPLICATION_ERROR";
                    break;

                // 500 Internal Server Error (Bilinmeyen hata)
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.ErrorCode = "INTERNAL_SERVER_ERROR";
                    response.Message = "Bir hata oluþtu. Lütfen yöneticiye bildirin.";
                    break;
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    /// <summary>
    /// Standart hata yanýt formatý
    /// </summary>
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}