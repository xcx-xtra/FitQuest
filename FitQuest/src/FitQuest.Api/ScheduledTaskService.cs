using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FitQuest.Api.Data; // Adjust the namespace according to your project structure

namespace FitQuest.Api
{
    public class ScheduledTaskService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public ScheduledTaskService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Add logic for scheduled tasks here
                        await ResetDailyPointsAsync();
                        await MarkOverdueGoalsAsync();
                        await UpdateChallengeStatusAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in scheduled task: {ex.Message}");
                }

                // Wait for 24 hours before running again
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task ResetDailyPointsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<FitQuestContext>();

                var users = await dbContext.Users.ToListAsync();
                foreach (var user in users)
                {
                    user.DailyPoints = 0; // Reset daily points
                }

                await dbContext.SaveChangesAsync();
                Console.WriteLine("Daily points reset for all users.");
            }
        }

        private async Task MarkOverdueGoalsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<FitQuestContext>();

                var overdueGoals = await dbContext.DailyGoals
                    .Where(goal => goal.DueDate < DateTime.UtcNow && !goal.IsCompleted)
                    .ToListAsync();

                foreach (var goal in overdueGoals)
                {
                    goal.IsOverdue = true;
                }

                await dbContext.SaveChangesAsync();
                Console.WriteLine("Overdue goals marked.");
            }
        }

        private async Task UpdateChallengeStatusAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<FitQuestContext>();

                var activeChallenges = await dbContext.Challenges
                    .Where(challenge => challenge.EndDate < DateTime.UtcNow && challenge.Status == "Active")
                    .ToListAsync();

                foreach (var challenge in activeChallenges)
                {
                    challenge.Status = "Completed";
                }

                await dbContext.SaveChangesAsync();
                Console.WriteLine("Challenge statuses updated.");
            }
        }
    }
}