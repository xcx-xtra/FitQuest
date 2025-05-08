using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;

public class LeaderboardService
{
    private readonly NavigationManager _nav;
    private HubConnection? _hub;

    public event Action? OnLeaderboardUpdated;

    public LeaderboardService(NavigationManager nav)
    {
        _nav = nav;
    }

    public async Task ConnectAsync()
    {
        _hub = new HubConnectionBuilder()
            .WithUrl(_nav.ToAbsoluteUri("/leaderboardHub"))
            .WithAutomaticReconnect()
            .Build();

        _hub.On("LeaderboardUpdated", () =>
        {
            OnLeaderboardUpdated?.Invoke();
        });

        await _hub.StartAsync();
        await _hub.SendAsync("JoinLeaderboard", "some-user-id");
    }
}