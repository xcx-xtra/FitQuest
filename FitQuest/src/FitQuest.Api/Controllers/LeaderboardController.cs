using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitQuest.Api;

[ApiController]
[Route("api/leaderboard")]
public class LeaderboardController : ControllerBase
{
    private readonly FitQuestContext _db;

    public LeaderboardController(FitQuestContext db)
    {
        _db = db;
    }

    [HttpGet("top")]
    public async Task<ActionResult<List<UserScoreDto>>> GetTopUsers()
    {
        var leaderboard = await _db.PointEvents
            .GroupBy(p => p.UserId)
            .Select(g => new UserScoreDto
            {
                UserId = g.Key,
                TotalPoints = g.Sum(x => x.Points)
            })
            .OrderByDescending(x => x.TotalPoints)
            .Take(10)
            .ToListAsync();

        return Ok(leaderboard);
    }
}

public class UserScoreDto
{
    public int UserId { get; set; }
    public int TotalPoints { get; set; }
}