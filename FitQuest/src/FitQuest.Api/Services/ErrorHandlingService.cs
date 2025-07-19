using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FitQuest.Api.Middleware;
using FitQuest.Api.Exceptions;
using System.Security.Authentication;

namespace FitQuest.Api.Services;

/// <summary>
/// Comprehensive error handling service for the FitQuest API
/// </summary>
public class ErrorHandlingService : IErrorHandlingService
{
    private readonly IWebHostEnvironment _environment;

    public ErrorHandlingService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public string LogAndGetUserFriendlyMessage(Exception exception, ILogger logger, string? context = null)
    {
        var errorContext = GetErrorContext(exception);
        if (!string.IsNullOrEmpty(context))
        {
            errorContext["Context"] = context;
        }

        using (logger.BeginScope(errorContext))
        {
            if (IsCriticalError(exception))
            {
                logger.LogCritical(exception, "Critical error occurred: {ErrorType}", exception.GetType().Name);
            }
            else
            {
                logger.LogError(exception, "Error occurred: {ErrorType}", exception.GetType().Name);
            }
        }

        return GetUserFriendlyMessage(exception);
    }

    public void HandleIdentityResult(IdentityResult result, string operation)
    {
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            throw new IdentityResultException($"Identity operation '{operation}' failed", errors);
        }
    }

    public bool IsCriticalError(Exception exception)
    {
        return exception switch
        {
            InvalidOperationException ex when ex.Message.Contains("JWT") => true,
            InvalidOperationException ex when ex.Message.Contains("database") => true,
            OutOfMemoryException => true,
            StackOverflowException => true,
            AccessViolationException => true,
            _ => false
        };
    }

    public Dictionary<string, object> GetErrorContext(Exception exception, HttpContext? httpContext = null)
    {
        var context = new Dictionary<string, object>
        {
            ["ExceptionType"] = exception.GetType().Name,
            ["ExceptionMessage"] = exception.Message,
            ["StackTrace"] = _environment.IsDevelopment() ? exception.StackTrace ?? "No stack trace available" : "Stack trace hidden in production"
        };

        if (httpContext != null)
        {
            context["RequestPath"] = httpContext.Request.Path.Value ?? "Unknown";
            context["RequestMethod"] = httpContext.Request.Method;
            context["UserAgent"] = httpContext.Request.Headers.UserAgent.ToString();
            context["RemoteIpAddress"] = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            context["CorrelationId"] = httpContext.TraceIdentifier;
        }

        // Add specific context for different exception types
        switch (exception)
        {
            case DbUpdateException dbEx:
                context["DatabaseError"] = true;
                if (dbEx.InnerException != null)
                {
                    context["InnerExceptionType"] = dbEx.InnerException.GetType().Name;
                    context["InnerExceptionMessage"] = dbEx.InnerException.Message;
                }
                break;

            case AuthenticationException authEx:
                context["AuthenticationError"] = true;
                break;

            case UnauthorizedAccessException:
                context["AuthorizationError"] = true;
                break;

            case TimeoutException:
                context["TimeoutError"] = true;
                break;
        }

        return context;
    }

    private string GetUserFriendlyMessage(Exception exception)
    {
        return exception switch
        {
            ArgumentNullException => "Required information is missing",
            ArgumentException => "Invalid request parameters provided",
            UnauthorizedAccessException => "You don't have permission to perform this action",
            AuthenticationException => "Authentication failed. Please check your credentials",
            KeyNotFoundException => "The requested resource was not found",
            DbUpdateConcurrencyException => "The resource was modified by another user. Please refresh and try again",
            DbUpdateException ex when ex.InnerException?.Message.Contains("UNIQUE constraint failed") == true => 
                "A record with this information already exists",
            DbUpdateException ex when ex.InnerException?.Message.Contains("FOREIGN KEY constraint failed") == true => 
                "Cannot complete operation due to related data constraints",
            DbUpdateException => "Database operation failed. Please try again",
            InvalidOperationException ex when ex.Message.Contains("JWT") => 
                "Authentication system error. Please contact support",
            InvalidOperationException ex when ex.Message.Contains("database") => 
                "Database service is currently unavailable. Please try again later",
            TimeoutException => "The operation took too long to complete. Please try again",
            TaskCanceledException ex when ex.InnerException is TimeoutException => 
                "Request was cancelled due to timeout. Please try again",
            NotSupportedException => "This operation is not currently supported",
            IdentityResultException identityEx => 
                $"Operation failed: {string.Join(", ", identityEx.Errors)}",
            _ => _environment.IsDevelopment() 
                ? $"An error occurred: {exception.Message}" 
                : "An unexpected error occurred. Please try again later"
        };
    }
}