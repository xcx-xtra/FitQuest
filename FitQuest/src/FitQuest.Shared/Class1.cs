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
        public string UserName { get; set; } = string.Empty;
    }

    public class DailyGoalDto {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StepsTarget { get; set; }
        public DateTime Date { get; set; }
    }
}
