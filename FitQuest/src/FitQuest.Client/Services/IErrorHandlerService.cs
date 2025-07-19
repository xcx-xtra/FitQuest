namespace FitQuest.Client.Services;

public interface IErrorHandlerService
{
    /// <summary>
    /// Handles an exception with appropriate logging and user notification
    /// </summary>
    void HandleError(Exception exception, string? context = null, bool showToast = true);
    
    /// <summary>
    /// Gets a user-friendly error message for an exception
    /// </summary>
    string GetUserFriendlyMessage(Exception exception);
    
    /// <summary>
    /// Determines if an error should be retried automatically
    /// </summary>
    bool ShouldRetry(Exception exception);
    
    /// <summary>
    /// Gets the severity level of an error
    /// </summary>
    ErrorSeverity GetErrorSeverity(Exception exception);
}

public enum ErrorSeverity
{
    Low,
    Medium,
    High,
    Critical
}