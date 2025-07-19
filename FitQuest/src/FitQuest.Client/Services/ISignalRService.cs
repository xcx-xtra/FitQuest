using Microsoft.AspNetCore.SignalR.Client;

namespace FitQuest.Client.Services;

public interface ISignalRService
{
    HubConnection? Connection { get; }
    bool IsConnected { get; }
    event Action<string>? OnConnectionStateChanged;
    event Action<Exception>? OnConnectionError;
    
    Task StartAsync();
    Task StopAsync();
    Task<bool> TryReconnectAsync();
    void RegisterHandler<T>(string methodName, Action<T> handler);
    void RegisterHandler(string methodName, Action handler);
    Task SendAsync(string methodName, object? arg = null);
}