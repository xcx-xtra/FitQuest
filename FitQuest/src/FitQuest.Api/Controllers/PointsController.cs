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
        public async Task<IActionResult> GetUserPoints(int userId)
        {
            var points = await _context.Set<PointEvent>().Where(p => p.UserId == userId).ToListAsync();
            return Ok(points);
        }
    }
}