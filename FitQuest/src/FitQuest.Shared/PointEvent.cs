namespace FitQuest.Shared.Models
{
    public class PointEvent
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Points { get; set; }
        public string? Description { get; set; }
        public DateTime DateAwarded { get; set; }
        public User? User { get; set; }
    }
}