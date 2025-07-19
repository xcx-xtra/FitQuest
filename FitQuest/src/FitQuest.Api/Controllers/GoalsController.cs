using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using FitQuest.Api;
using FitQuest.Api.Services;
using FitQuest.Shared.Models;
using System.Security.Claims;

namespace FitQuest.Api.Controllers {
    public class GoalsController : BaseApiController {
        private readonly FitQuestContext _context;

        public GoalsController(
            FitQuestContext context, 
            ILogger<GoalsController> logger,
            IDatabaseErrorHandler databaseErrorHandler) 
            : base(logger, databaseErrorHandler)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGoal(DailyGoal goal) {
            return await ExecuteWithErrorHandlingAsync(async () =>
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdClaim, out var userId) || userId != goal.UserId)
                {
                    throw new UnauthorizedAccessException("User not authorized to create goal for another user");
                }

                if (goal == null)
                {
                    throw new ArgumentException("Goal cannot be null");
                }

                _context.DailyGoals.Add(goal);
                await _context.SaveChangesAsync();
                
                Logger.LogInformation("Goal created successfully for user {UserId}", userId);
                return goal;
            }, "CreateGoal");
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGoals(int userId)
        {
            return await ExecuteWithErrorHandlingAsync(async () =>
            {
                if (userId <= 0)
                {
                    throw new ArgumentException("Invalid user ID");
                }

                var goals = await _context.DailyGoals
                    .Where(g => g.UserId == userId)
                    .ToListAsync();
                
                Logger.LogInformation("Retrieved {Count} goals for user {UserId}", goals.Count, userId);
                return goals;
            }, "GetGoals");
        }
    }
}