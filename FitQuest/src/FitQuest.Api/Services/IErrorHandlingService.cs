using Microsoft.AspNetCore.Identity;

namespace FitQuest.Api.Services;

/// <summary>
/// Service for handling and categorizing errors throughout the application
/// </summary>
public interface IErrorHandlingService
{
    /// <summary>
    /// Logs an error with appropriate context and returns a user-friendly message
    /// </summary>
    string LogAndGetUserFriendlyMessage(Exception exception, ILogger logger, string? context = null);

    /// <summary>
    /// Handles Identity result errors and throws appropriate exceptions
    /// </summary>
    void HandleIdentityResult(IdentityResult result, string operation);

    /// <summary>
    /// Determines if an exception should be logged as critical
    /// </summary>
    bool IsCriticalError(Exception exception);

    /// <summary>
    /// Gets structured error data for logging
    /// </summary>
    Dictionary<string, object> GetErrorContext(Exception exception, HttpContext? httpContext = null);
}