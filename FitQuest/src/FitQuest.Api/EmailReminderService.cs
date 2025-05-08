using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using FitQuest.Shared.Models;
using FitQuest.Api;
using User = FitQuest.Shared.Models.User;

public class EmailReminderService
{
    private readonly FitQuestContext _db;
    private readonly IEmailSender _emailSender;

    public EmailReminderService(FitQuestContext db, IEmailSender emailSender)
    {
        _db = db;
        _emailSender = emailSender;
    }

    public async Task SendReminders()
    {
        var users = await _db.Users
            .Where(static u => u.DailyGoals.Any(static g => !g.IsCompleted))
            .ToListAsync();

        foreach (var user in users)
        {
            // Ensure the 'to' parameter is not null before calling SendEmailAsync.
            if (string.IsNullOrEmpty(user.Email))
            {
                throw new InvalidOperationException("User email cannot be null or empty.");
            }

            await _emailSender.SendEmailAsync(
                user.Email,
                "Reminder: Complete Your FitQuest Goals!",
                "You still have goals to complete today. Keep up the streak!"
            );
        }
    }
}