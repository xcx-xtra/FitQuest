namespace FitQuest.Client.Services;

public interface IToastService
{
    event Action<ToastMessage>? OnToastAdded;
    void ShowSuccess(string message, string? title = null);
    void ShowError(string message, string? title = null);
    void ShowWarning(string message, string? title = null);
    void ShowInfo(string message, string? title = null);
    void ShowError(Exception exception, string? customMessage = null);
}

public class ToastMessage
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Message { get; set; } = string.Empty;
    public string? Title { get; set; }
    public ToastType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int DurationMs { get; set; } = 5000;
}

public enum ToastType
{
    Success,
    Error,
    Warning,
    Info
}