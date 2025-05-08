using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FitQuest.Api;
using System.Threading;
using System.Threading.Tasks;
using DailyGoal = FitQuest.Shared.Models.DailyGoal;

public class DailyGoalResetService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DailyGoalResetService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<FitQuestContext>();

            // Reset all daily goals to incomplete at midnight
            var today = DateTime.UtcNow.Date;
            var goals = await db.DailyGoals.Where(g => g.Date < today).ToListAsync();
            foreach (var g in goals)
            {
                // Updated to use IsCompleted instead of IsComplete.
                g.IsCompleted = false;
                g.Date = today;
            }

            await db.SaveChangesAsync();

            // Run every 24 hours
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}