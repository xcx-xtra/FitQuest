namespace FitQuest.Shared.Models {
    public class User {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public DateTime LastLogin { get; set; }
        public int TotalSteps { get; set; }
        public int TotalBadgesEarned { get; set; }
        public int TotalGoalsCompleted { get; set; }
    }

    public class DailyGoal {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    public class Badge {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
