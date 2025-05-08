namespace FitQuest.Shared.Models;

public class DailyGoal
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime Date { get; set; } = DateTime.UtcNow;
}