using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using FitQuest.Shared;
using FitQuest.Shared.Models;

namespace FitQuest.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PointsController : ControllerBase
    {
        private readonly FitQuestContext _context;

        public PointsController(FitQuestContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AwardPoints(PointEvent points)
        {
            _context.Add(points);
            await _context.SaveChangesAsync();
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