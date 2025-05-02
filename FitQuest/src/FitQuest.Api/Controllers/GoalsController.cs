using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using FitQuest.Api;
using FitQuest.Shared.Models;

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
            _context.DailyGoals.Add(goal);
            await _context.SaveChangesAsync();
            return Ok(goal);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGoals(int userId) {
            var goals = await _context.DailyGoals.Where(g => g.UserId == userId).ToListAsync();
            return Ok(goals);
        }
    }
}