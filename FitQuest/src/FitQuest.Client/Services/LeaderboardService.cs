using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

public class LeaderboardService : IAsyncDisposable
{
    private readonly NavigationManager _nav;
    private readonly ITokenService _tokenService;
    private HubConnection? _hub;
    private bool _isConnected;

    public event Action? OnLeaderboardUpdated;
    public event Action<Exception>? OnError;

    public LeaderboardService(NavigationManager nav, ITokenService tokenService)
    {
        _nav = nav;
        _tokenService = tokenService;
    }

    public async Task ConnectAsync()
    {
        try
        {
            if (_hub?.State == HubConnectionState.Connected) return;

            var token = await _tokenService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Authentication token not found");
            }

            _hub = new HubConnectionBuilder()
                .WithUrl(_nav.ToAbsoluteUri("/leaderboardHub"), options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult<string?>(token);
                })
                .WithAutomaticReconnect()
                .Build();

            _hub.On("LeaderboardUpdated", () =>
            {
                OnLeaderboardUpdated?.Invoke();
            });

            _hub.Closed += async (error) =>
            {
                _isConnected = false;
                if (error != null)
                {
                    OnError?.Invoke(error);
                }
                await Task.Delay(Random.Shared.Next(0, 5) * 1000);
                try
                {
                    await ConnectAsync();
                }
                catch (Exception ex)
                {
                    OnError?.Invoke(ex);
                }
            };

            await _hub.StartAsync();
            _isConnected = true;
        }
        catch (Exception ex)
        {
            _isConnected = false;
            OnError?.Invoke(ex);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hub != null)
        {
            await _hub.DisposeAsync();
        }
    }
}