using Microsoft.Extensions.Logging;

namespace FitQuest.Client.Services;

public class ErrorHandlerService : IErrorHandlerService
{
    private readonly ILogger<ErrorHandlerService> _logger;
    private readonly IToastService _toastService;

    public ErrorHandlerService(ILogger<ErrorHandlerService> logger, IToastService toastService)
    {
        _logger = logger;
        _toastService = toastService;
    }

    public void HandleError(Exception exception, string? context = null, bool showToast = true)
    {
        var severity = GetErrorSeverity(exception);
        var contextInfo = string.IsNullOrEmpty(context) ? "" : $" in {context}";
        
        // Log the error with appropriate level
        switch (severity)
        {
            case ErrorSeverity.Low:
                _logger.LogInformation(exception, "Low severity error{Context}", contextInfo);
                break;
            case ErrorSeverity.Medium:
                _logger.LogWarning(exception, "Medium severity error{Context}", contextInfo);
                break;
            case ErrorSeverity.High:
                _logger.LogError(exception, "High severity error{Context}", contextInfo);
                break;
            case ErrorSeverity.Critical:
                _logger.LogCritical(exception, "Critical error{Context}", contextInfo);
                break;
        }

        // Show user notification if requested
        if (showToast)
        {
            var message = GetUserFriendlyMessage(exception);
            var title = GetErrorTitle(exception, context);
            
            switch (severity)
            {
                case ErrorSeverity.Low:
                    _toastService.ShowInfo(message, title);
                    break;
                case ErrorSeverity.Medium:
                    _toastService.ShowWarning(message, title);
                    break;
                case ErrorSeverity.High:
                case ErrorSeverity.Critical:
                    _toastService.ShowError(message, title);
                    break;
            }
        }
    }

    public string GetUserFriendlyMessage(Exception exception)
    {
        if (exception is HttpRequestException httpEx)
        {
            if (httpEx.Message.Contains("401"))
                return "Please log in to continue.";
            if (httpEx.Message.Contains("403"))
                return "You don't have permission to perform this action.";
            if (httpEx.Message.Contains("404"))
                return "The requested resource was not found.";
            if (httpEx.Message.Contains("500"))
                return "Server error occurred. Please try again later.";
            if (httpEx.Message.Contains("502"))
                return "Service temporarily unavailable. Please try again in a moment.";
            if (httpEx.Message.Contains("503"))
                return "Service is currently under maintenance. Please try again later.";
            
            return "Network error occurred. Please check your connection and try again.";
        }

        if (exception is TaskCanceledException)
            return "Request timed out. Please check your connection and try again.";
        if (exception is ArgumentException)
            return "Invalid input provided. Please check your data and try again.";
        if (exception is ArgumentNullException)
            return "Required information is missing. Please check your input.";
        if (exception is InvalidOperationException)
            return "This operation cannot be completed at this time. Please try again.";
        if (exception is UnauthorizedAccessException)
            return "You don't have permission to access this resource.";
        if (exception is NotSupportedException)
            return "This operation is not supported in your current environment.";
        if (exception is FormatException)
            return "The data format is invalid. Please check your input.";

        return "An unexpected error occurred. Please try again or contact support if the problem persists.";
    }

    public bool ShouldRetry(Exception exception)
    {
        if (exception is HttpRequestException httpEx)
        {
            return httpEx.Message.Contains("500") || 
                   httpEx.Message.Contains("502") || 
                   httpEx.Message.Contains("503");
        }

        return exception is TaskCanceledException;
    }

    public ErrorSeverity GetErrorSeverity(Exception exception)
    {
        if (exception is HttpRequestException httpEx)
        {
            if (httpEx.Message.Contains("500") || httpEx.Message.Contains("502") || httpEx.Message.Contains("503"))
                return ErrorSeverity.High;
            if (httpEx.Message.Contains("401") || httpEx.Message.Contains("403") || httpEx.Message.Contains("404"))
                return ErrorSeverity.Medium;
            
            return ErrorSeverity.Medium;
        }

        if (exception is ArgumentException || exception is ArgumentNullException || exception is FormatException)
            return ErrorSeverity.Low;
        if (exception is TaskCanceledException || exception is UnauthorizedAccessException)
            return ErrorSeverity.Medium;
        if (exception is InvalidOperationException || exception is NotSupportedException)
            return ErrorSeverity.High;

        return ErrorSeverity.Critical;
    }

    private string GetErrorTitle(Exception exception, string? context)
    {
        var baseTitle = "Error";
        
        if (exception is HttpRequestException)
            baseTitle = "Network Error";
        else if (exception is TaskCanceledException)
            baseTitle = "Timeout Error";
        else if (exception is ArgumentException)
            baseTitle = "Input Error";
        else if (exception is ArgumentNullException)
            baseTitle = "Missing Data";
        else if (exception is InvalidOperationException)
            baseTitle = "Operation Error";
        else if (exception is UnauthorizedAccessException)
            baseTitle = "Access Denied";
        else if (exception is NotSupportedException)
            baseTitle = "Not Supported";
        else if (exception is FormatException)
            baseTitle = "Format Error";

        return string.IsNullOrEmpty(context) ? baseTitle : $"{baseTitle} - {context}";
    }
}