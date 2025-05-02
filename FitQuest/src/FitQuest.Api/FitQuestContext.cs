using System;
using Microsoft.EntityFrameworkCore;
using FitQuest.Shared.Models; // adjust if your models live elsewhere
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FitQuest.Shared.Models
{
    public class User : IdentityUser<int>
    {
    }
}

namespace FitQuest.Api
{
    public class FitQuestContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public FitQuestContext(DbContextOptions<FitQuestContext> options)
            : base(options) { }

        public DbSet<DailyGoal> DailyGoals { get; set; }
        public DbSet<Badge> Badges { get; set; }
        // add DbSet<PointEvent>, DbSet<Challenge> as needed
    }
}