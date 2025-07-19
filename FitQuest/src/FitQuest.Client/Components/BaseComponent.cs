using Microsoft.AspNetCore.Components;
using FitQuest.Client.Services;

namespace FitQuest.Client.Components;

public abstract class BaseComponent : ComponentBase
{
    [Inject] protected IToastService ToastService { get; set; } = default!;
    [Inject] protected ILogger<BaseComponent> Logger { get; set; } = default!;
    [Inject] protected IErrorHandlerService ErrorHandler { get; set; } = default!;

    protected bool IsLoading { get; set; }
    protected string? ErrorMessage { get; set; }

    protected async Task ExecuteWithErrorHandlingAsync(Func<Task> operation, string operationName)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            StateHasChanged();

            await operation();
        }
        catch (Exception ex)
        {
            ErrorMessage = ErrorHandler.GetUserFriendlyMessage(ex);
            ErrorHandler.HandleError(ex, operationName);
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    protected async Task<T?> ExecuteWithErrorHandlingAsync<T>(Func<Task<T>> operation, string operationName)
    {
        try
        {
            IsLoading = true;
            ErrorMessage = null;
            StateHasChanged();

            return await operation();
        }
        catch (Exception ex)
        {
            ErrorMessage = ErrorHandler.GetUserFriendlyMessage(ex);
            ErrorHandler.HandleError(ex, operationName);
            return default;
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    protected void ExecuteWithErrorHandling(Action operation, string operationName)
    {
        try
        {
            ErrorMessage = null;
            operation();
        }
        catch (Exception ex)
        {
            ErrorMessage = ErrorHandler.GetUserFriendlyMessage(ex);
            ErrorHandler.HandleError(ex, operationName);
            StateHasChanged();
        }
    }

    protected T? ExecuteWithErrorHandling<T>(Func<T> operation, string operationName)
    {
        try
        {
            ErrorMessage = null;
            return operation();
        }
        catch (Exception ex)
        {
            ErrorMessage = ErrorHandler.GetUserFriendlyMessage(ex);
            ErrorHandler.HandleError(ex, operationName);
            StateHasChanged();
            return default;
        }
    }
}