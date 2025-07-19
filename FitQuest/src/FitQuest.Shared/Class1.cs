namespace FitQuest.Shared.Models {
    public class DailyGoalDto {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int StepsTarget { get; set; }
        public DateTime Date { get; set; }
    }
}
