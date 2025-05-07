using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FitQuest.Shared.Models;

public class DailyGoal
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = "";

    public string? Description { get; set; }

    public bool IsCompleted { get; set; } = false;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    // Foreign key to User
    [Required]
    public required int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}