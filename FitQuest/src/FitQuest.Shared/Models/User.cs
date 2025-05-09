using FitQuest.Shared.Models;
using Microsoft.AspNetCore.Identity;

namespace FitQuest.Shared.Models
{
    public class User : IdentityUser<int>
    {
        public List<DailyGoal> DailyGoals { get; set; } = new();
        public int DailyPoints { get; set; }
    }
}