using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using FitQuest.Shared;
using FitQuest.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace FitQuest.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointsController : ControllerBase
    {
        private readonly FitQuestContext _context;
        private readonly IHubContext<LeaderboardHub> _hub;

        public PointsController(FitQuestContext context, IHubContext<LeaderboardHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        [HttpPost]
        public async Task<IActionResult> AwardPoints(PointEvent points)
        {
            _context.Add(points);
            await _context.SaveChangesAsync();

            // Notify leaderboard
            await _hub.Clients.Group("Leaderboard").SendAsync("LeaderboardUpdated");

            return Ok(points);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<PointSummaryDto>> GetUserPoints(int userId)
        {
            var events = await _context.PointEvents
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.DateAwarded)
                .ToListAsync();

            var totalPoints = events.Sum(p => p.Points);

            var summary = new PointSummaryDto
            {
                TotalPoints = totalPoints,
                History = events.Select(e => new PointEventDto
                {
                    Points = e.Points,
                    Description = e.Description ?? string.Empty,
                    Timestamp = e.DateAwarded
                }).ToList()
            };

            return Ok(summary);
        }
    }
}