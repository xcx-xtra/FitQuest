using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using FitQuest.Api;
using FitQuest.Shared.Models;
using System.Security.Claims;

namespace FitQuest.Api.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class GoalsController : ControllerBase {
        private readonly FitQuestContext _context;

        public GoalsController(FitQuestContext context) {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGoal(DailyGoal goal) {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId) || userId != goal.UserId)
            {
                return Unauthorized();
            }

            _context.DailyGoals.Add(goal);
            await _context.SaveChangesAsync();
            return Ok(goal);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGoals(int userId) // Change userId to int
        {
            var goals = await _context.DailyGoals.Where(g => g.UserId == userId).ToListAsync(); // Ensure type consistency
            return Ok(goals);
        }
    }
}