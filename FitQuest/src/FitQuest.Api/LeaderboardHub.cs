using Microsoft.AspNetCore.SignalR;

public class LeaderboardHub : Hub
{
    public async Task JoinLeaderboard(string userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Leaderboard");
    }
}