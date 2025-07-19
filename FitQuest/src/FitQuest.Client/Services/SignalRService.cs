using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace FitQuest.Client.Services;

/// <summary>
/// Service for managing SignalR real-time communication with the FitQuest API.
/// Handles connection management, automatic reconnection, and real-time updates for leaderboards.
/// Implements comprehensive error handling and connection state management.
/// </summary>
public class SignalRService : ISignalRService, IAsyncDisposable
{
    // Dependencies for logging, user notifications, and URL configuration
    private readonly ILogger<SignalRService> _logger;
    private readonly IToastService _toastService;
    private readonly string _hubUrl;
    
    // SignalR connection instance and reconnection management
    private HubConnection? _connection;
    private readonly Timer _reconnectTimer;
    private int _reconnectAttempts = 0;
    
    // Connection retry configuration constants
    private const int MaxReconnectAttempts = 5;
    private const int ReconnectDelayMs = 5000;

    /// <summary>Gets the current SignalR hub connection instance.</summary>
    public HubConnection? Connection => _connection;
    
    /// <summary>Gets whether the SignalR connection is currently established.</summary>
    public bool IsConnected => _connection?.State == HubConnectionState.Connected;

    /// <summary>Event fired when connection state changes (connected, disconnected, etc.).</summary>
    public event Action<string>? OnConnectionStateChanged;
    
    /// <summary>Event fired when connection errors occur.</summary>
    public event Action<Exception>? OnConnectionError;

    /// <summary>
    /// Initializes the SignalR service with required dependencies and configuration.
    /// Sets up the hub URL and prepares connection management.
    /// </summary>
    /// <param name="logger">Logger for connection events and errors</param>
    /// <param name="toastService">Service for displaying user notifications</param>
    /// <param name="configuration">Configuration containing API base URL</param>
    public SignalRService(ILogger<SignalRService> logger, IToastService toastService, IConfiguration configuration)
    {
        _logger = logger;
        _toastService = toastService;
        
        // Build the SignalR hub URL from configuration with proper validation
        var baseUrl = configuration["ApiBaseUrl"] ?? "http://localhost:5124/";
        if (!baseUrl.EndsWith("/"))
        {
            baseUrl += "/";
        }
        _hubUrl = baseUrl + "leaderboardHub";
        
        _logger.LogInformation("SignalR hub URL configured as: {HubUrl}", _hubUrl);
        
        // Initialize reconnection timer (disabled initially)
        _reconnectTimer = new Timer(async _ => await TryReconnectAsync(), null, Timeout.Infinite, Timeout.Infinite);
        
        // Set up the initial connection
        InitializeConnection();
    }

    /// <summary>
    /// Initializes the SignalR hub connection with proper configuration and event handlers.
    /// Validates the hub URL and sets up automatic reconnection behavior.
    /// </summary>
    private void InitializeConnection()
    {
        try
        {
            // Validate the hub URL before attempting to create connection
            if (string.IsNullOrWhiteSpace(_hubUrl))
            {
                _logger.LogError("Hub URL is null or empty, cannot initialize SignalR connection");
                return;
            }

            if (!Uri.IsWellFormedUriString(_hubUrl, UriKind.Absolute))
            {
                _logger.LogError("Hub URL is not well-formed: {HubUrl}", _hubUrl);
                return;
            }

            _connection = new HubConnectionBuilder()
                .WithUrl(_hubUrl, options =>
                {
                    // Add authentication token if available
                    // options.AccessTokenProvider = () => Task.FromResult(GetAccessToken());
                })
                .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30) })
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .Build();

            // Handle connection state changes
            _connection.Closed += OnConnectionClosed;
            _connection.Reconnecting += OnReconnecting;
            _connection.Reconnected += OnReconnected;

            _logger.LogInformation("SignalR connection initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize SignalR connection with URL: {HubUrl}", _hubUrl);
            OnConnectionError?.Invoke(ex);
        }
    }

    public async Task StartAsync()
    {
        if (_connection == null)
        {
            InitializeConnection();
        }

        try
        {
            if (_connection!.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync();
                _logger.LogInformation("SignalR connection started successfully");
                _toastService.ShowSuccess("Connected to real-time updates", "Connection Established");
                OnConnectionStateChanged?.Invoke("Connected");
                _reconnectAttempts = 0;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start SignalR connection");
            OnConnectionError?.Invoke(ex);
            _toastService.ShowError("Failed to connect to real-time updates. Some features may not work properly.", "Connection Error");
            
            // Start reconnection attempts
            _reconnectTimer.Change(ReconnectDelayMs, Timeout.Infinite);
        }
    }

    public async Task StopAsync()
    {
        if (_connection != null)
        {
            try
            {
                await _connection.StopAsync();
                _logger.LogInformation("SignalR connection stopped");
                OnConnectionStateChanged?.Invoke("Disconnected");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping SignalR connection");
            }
        }
    }

    public async Task<bool> TryReconnectAsync()
    {
        if (_connection == null || IsConnected || _reconnectAttempts >= MaxReconnectAttempts)
        {
            if (_reconnectAttempts >= MaxReconnectAttempts)
            {
                _logger.LogWarning("Maximum reconnection attempts reached");
                _toastService.ShowWarning("Unable to establish real-time connection after multiple attempts. Some features may be limited.", "Connection Warning");
                OnConnectionStateChanged?.Invoke("Failed");
            }
            return false;
        }

        try
        {
            _reconnectAttempts++;
            _logger.LogInformation("Attempting to reconnect to SignalR hub (attempt {Attempt}/{MaxAttempts})", 
                _reconnectAttempts, MaxReconnectAttempts);

            // Dispose old connection and create new one if needed
            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync();
            }
            
            _logger.LogInformation("SignalR reconnection successful");
            _toastService.ShowSuccess("Reconnected to real-time updates", "Connection Restored");
            OnConnectionStateChanged?.Invoke("Reconnected");
            _reconnectAttempts = 0;
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SignalR reconnection attempt {Attempt} failed", _reconnectAttempts);
            
            if (_reconnectAttempts < MaxReconnectAttempts)
            {
                // Exponential backoff for reconnection attempts
                var delay = Math.Min(ReconnectDelayMs * (int)Math.Pow(2, _reconnectAttempts - 1), 30000);
                _reconnectTimer.Change(delay, Timeout.Infinite);
                _logger.LogInformation("Scheduling next reconnection attempt in {Delay}ms", delay);
            }
            else
            {
                _toastService.ShowError("Unable to reconnect to real-time updates. Please refresh the page to restore full functionality.", "Connection Failed");
                OnConnectionStateChanged?.Invoke("Failed");
            }
            
            return false;
        }
    }

    public void RegisterHandler<T>(string methodName, Action<T> handler)
    {
        _connection?.On(methodName, handler);
    }

    public void RegisterHandler(string methodName, Action handler)
    {
        _connection?.On(methodName, handler);
    }

    public async Task SendAsync(string methodName, object? arg = null)
    {
        if (!IsConnected)
        {
            _logger.LogWarning("Cannot send SignalR message - not connected");
            _toastService.ShowWarning("Real-time connection not available", "Connection Warning");
            return;
        }

        try
        {
            if (arg != null)
            {
                await _connection!.SendAsync(methodName, arg);
            }
            else
            {
                await _connection!.SendAsync(methodName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SignalR message: {MethodName}", methodName);
            OnConnectionError?.Invoke(ex);
            _toastService.ShowError($"Failed to send real-time update: {methodName}", "Communication Error");
        }
    }

    private Task OnConnectionClosed(Exception? exception)
    {
        _logger.LogWarning(exception, "SignalR connection closed");
        OnConnectionStateChanged?.Invoke("Disconnected");
        
        if (exception != null)
        {
            OnConnectionError?.Invoke(exception);
            _toastService.ShowWarning("Real-time connection lost. Attempting to reconnect...", "Connection Lost");
            
            // Start reconnection attempts
            _reconnectTimer.Change(ReconnectDelayMs, Timeout.Infinite);
        }
        
        return Task.CompletedTask;
    }

    private Task OnReconnecting(Exception? exception)
    {
        _logger.LogInformation("SignalR connection reconnecting...");
        OnConnectionStateChanged?.Invoke("Reconnecting");
        return Task.CompletedTask;
    }

    private Task OnReconnected(string? connectionId)
    {
        _logger.LogInformation("SignalR connection reconnected with ID: {ConnectionId}", connectionId);
        OnConnectionStateChanged?.Invoke("Reconnected");
        _toastService.ShowSuccess("Real-time connection restored", "Reconnected");
        _reconnectAttempts = 0;
        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        _reconnectTimer?.Dispose();
        
        if (_connection != null)
        {
            await _connection.DisposeAsync();
        }
    }
}