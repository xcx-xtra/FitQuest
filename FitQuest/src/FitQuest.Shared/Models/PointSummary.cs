namespace FitQuest.Shared.Models
{
    public class PointEventDto
    {
        public int Points { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    public class PointSummaryDto
    {
        public int TotalPoints { get; set; }
        public List<PointEventDto> History { get; set; } = new();
    }
}