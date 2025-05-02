namespace FitQuest.Api.Models
{
    public class PointEvent
    {
        public int Id { get; set; } // Unique identifier for the point event
        public int UserId { get; set; } // ID of the user associated with the point event
        public int Points { get; set; } // Number of points awarded
        public required string EventType { get; set; } // Type of event (e.g., "GoalCompleted", "ChallengeWon")
        public required string Description { get; set; } // Description of the point event
        public DateTime CreatedAt { get; set; } // Timestamp when the point event was created
        public DateTime? UpdatedAt { get; set; } // Timestamp when the point event was last updated (optional)
    }
}