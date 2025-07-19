using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using FitQuest.Api.Exceptions;

namespace FitQuest.Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
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
        // Ensure response hasn't been started
        if (context.Response.HasStarted)
        {
            _logger.LogWarning("Cannot handle exception - response has already started");
            return;
        }

        context.Response.ContentType = "application/json";
        
        var response = new ErrorResponse();
        var correlationId = context.TraceIdentifier;

        // Log the exception with structured data
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId,
            ["RequestPath"] = context.Request.Path,
            ["RequestMethod"] = context.Request.Method,
            ["UserAgent"] = context.Request.Headers.UserAgent.ToString(),
            ["RemoteIpAddress"] = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown"
        }))
        {
            switch (exception)
            {
                case ArgumentNullException argNullEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Required parameter is missing";
                    response.Details = _environment.IsDevelopment() ? argNullEx.Message : null;
                    _logger.LogWarning(argNullEx, "Bad request: Required parameter is null");
                    break;

                case ArgumentException argEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Invalid request parameters";
                    response.Details = _environment.IsDevelopment() ? argEx.Message : null;
                    _logger.LogWarning(argEx, "Bad request: Invalid parameters provided");
                    break;

                case UnauthorizedAccessException unauthEx:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = "Unauthorized access";
                    response.Details = _environment.IsDevelopment() ? unauthEx.Message : null;
                    _logger.LogWarning(unauthEx, "Unauthorized access attempt");
                    break;

                case AuthenticationException authEx:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = "Authentication failed";
                    response.Details = _environment.IsDevelopment() ? authEx.Message : null;
                    _logger.LogWarning(authEx, "Authentication failure");
                    break;

                case KeyNotFoundException notFoundEx:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = "Resource not found";
                    response.Details = _environment.IsDevelopment() ? notFoundEx.Message : null;
                    _logger.LogInformation(notFoundEx, "Resource not found");
                    break;

                case DbUpdateConcurrencyException concurrencyEx:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Message = "The resource was modified by another user. Please refresh and try again.";
                    response.Details = _environment.IsDevelopment() ? concurrencyEx.Message : null;
                    _logger.LogWarning(concurrencyEx, "Database concurrency conflict occurred");
                    break;

                case DbUpdateException dbEx:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.Message = "Database operation failed";
                    
                    // Provide more specific error messages for common database issues
                    if (dbEx.InnerException?.Message.Contains("UNIQUE constraint failed") == true)
                    {
                        response.Message = "A record with this information already exists";
                    }
                    else if (dbEx.InnerException?.Message.Contains("FOREIGN KEY constraint failed") == true)
                    {
                        response.Message = "Cannot complete operation due to related data constraints";
                    }
                    
                    response.Details = _environment.IsDevelopment() ? dbEx.InnerException?.Message ?? dbEx.Message : null;
                    _logger.LogError(dbEx, "Database update exception occurred");
                    break;

                case InvalidOperationException invOpEx when invOpEx.Message.Contains("JWT"):
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "Authentication configuration error";
                    response.Details = _environment.IsDevelopment() ? invOpEx.Message : null;
                    _logger.LogCritical(invOpEx, "JWT configuration error - check appsettings");
                    break;

                case InvalidOperationException invOpEx when invOpEx.Message.Contains("database"):
                    response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    response.Message = "Database service is currently unavailable";
                    response.Details = _environment.IsDevelopment() ? invOpEx.Message : null;
                    _logger.LogError(invOpEx, "Database service unavailable");
                    break;

                case TimeoutException timeoutEx:
                    response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    response.Message = "Request timeout - the operation took too long to complete";
                    response.Details = _environment.IsDevelopment() ? timeoutEx.Message : null;
                    _logger.LogWarning(timeoutEx, "Request timeout occurred");
                    break;

                case TaskCanceledException cancelEx when cancelEx.InnerException is TimeoutException:
                    response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    response.Message = "Request was cancelled due to timeout";
                    response.Details = _environment.IsDevelopment() ? cancelEx.Message : null;
                    _logger.LogWarning(cancelEx, "Request cancelled due to timeout");
                    break;

                case NotSupportedException notSupportedEx:
                    response.StatusCode = (int)HttpStatusCode.NotImplemented;
                    response.Message = "Operation not supported";
                    response.Details = _environment.IsDevelopment() ? notSupportedEx.Message : null;
                    _logger.LogWarning(notSupportedEx, "Unsupported operation attempted");
                    break;

                case IdentityResultException identityEx:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = "Identity operation failed";
                    response.Details = _environment.IsDevelopment() ? string.Join(", ", identityEx.Errors) : null;
                    _logger.LogWarning(identityEx, "Identity operation failed with errors: {Errors}", string.Join(", ", identityEx.Errors));
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "An internal server error occurred";
                    response.Details = _environment.IsDevelopment() ? exception.Message : null;
                    _logger.LogError(exception, "Unhandled exception occurred");
                    break;
            }
        }

        context.Response.StatusCode = response.StatusCode;

        // Add correlation ID for tracking
        context.Response.Headers.Append("X-Correlation-ID", correlationId);
        response.CorrelationId = correlationId;

        // Add additional headers for debugging in development
        if (_environment.IsDevelopment())
        {
            context.Response.Headers.Append("X-Exception-Type", exception.GetType().Name);
        }

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _environment.IsDevelopment()
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public string? CorrelationId { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}