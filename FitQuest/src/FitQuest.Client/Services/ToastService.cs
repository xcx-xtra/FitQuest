namespace FitQuest.Client.Services;

public class ToastService : IToastService
{
    public event Action<ToastMessage>? OnToastAdded;

    public void ShowSuccess(string message, string? title = null)
    {
        ShowToast(message, title, ToastType.Success);
    }

    public void ShowError(string message, string? title = null)
    {
        ShowToast(message, title, ToastType.Error, 8000); // Longer duration for errors
    }

    public void ShowWarning(string message, string? title = null)
    {
        ShowToast(message, title, ToastType.Warning, 6000);
    }

    public void ShowInfo(string message, string? title = null)
    {
        ShowToast(message, title, ToastType.Info);
    }

    public void ShowError(Exception exception, string? customMessage = null)
    {
        var message = customMessage ?? GetUserFriendlyErrorMessage(exception);
        var title = GetErrorTitle(exception);
        ShowError(message, title);
    }

    private void ShowToast(string message, string? title, ToastType type, int durationMs = 5000)
    {
        var toast = new ToastMessage
        {
            Message = message,
            Title = title,
            Type = type,
            DurationMs = durationMs
        };

        OnToastAdded?.Invoke(toast);
    }

    private string GetUserFriendlyErrorMessage(Exception exception)
    {
        return exception switch
        {
            HttpRequestException httpEx when httpEx.Message.Contains("401") => 
                "Please log in to continue.",
            
            HttpRequestException httpEx when httpEx.Message.Contains("403") => 
                "You don't have permission to perform this action.",
            
            HttpRequestException httpEx when httpEx.Message.Contains("404") => 
                "The requested resource was not found.",
            
            HttpRequestException httpEx when httpEx.Message.Contains("500") => 
                "Server error occurred. Please try again later.",
            
            TaskCanceledException => 
                "Request timed out. Please check your connection.",
            
            ArgumentException => 
                "Invalid input provided. Please check your data.",
            
            InvalidOperationException => 
                "Operation cannot be completed at this time.",
            
            _ => "An unexpected error occurred. Please try again."
        };
    }

    private string GetErrorTitle(Exception exception)
    {
        return exception switch
        {
            HttpRequestException => "Network Error",
            TaskCanceledException => "Timeout Error",
            ArgumentException => "Input Error",
            InvalidOperationException => "Operation Error",
            _ => "Error"
        };
    }
}