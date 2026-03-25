using System.Net;
using System.Text.Json;

namespace QuantityMeasurementWebApi.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        context.Response.ContentType = "application/json";
        
        var response = context.Response;
        var errorResponse = new ErrorResponse
        {
            Timestamp = DateTime.UtcNow,
            Path = context.Request.Path,
            Error = "Internal Server Error"
        };

        switch (exception)
        {
            case ArgumentException argEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Error = "Bad Request";
                errorResponse.Message = argEx.Message;
                break;
                
            case InvalidOperationException opEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Error = "Bad Request";
                errorResponse.Message = opEx.Message;
                break;
                
            case KeyNotFoundException keyEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Error = "Not Found";
                errorResponse.Message = keyEx.Message;
                break;
                
            case UnauthorizedAccessException authEx:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Error = "Unauthorized";
                errorResponse.Message = authEx.Message;
                break;
                
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "An unexpected error occurred. Please try again later.";
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(jsonResponse);
    }
}

public class ErrorResponse
{
    public DateTime Timestamp { get; set; }
    public int StatusCode { get; set; }
    public string Error { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}
