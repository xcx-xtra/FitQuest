using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitQuest.Shared.Models;
using FitQuest.Api;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly FitQuestContext _db;

    public AdminController(FitQuestContext db)
    {
        _db = db;
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<FitQuest.Shared.Models.User>>> GetUsers()
    {
        return await _db.Users.Cast<FitQuest.Shared.Models.User>().ToListAsync();
    }

    [HttpPost("badges")]
    public async Task<IActionResult> CreateBadge([FromBody] Badge badge)
    {
        _db.Badges.Add(badge);
        await _db.SaveChangesAsync();

        return Ok(badge);
    }

    [HttpDelete("badges/{id}")]
    public async Task<IActionResult> DeleteBadge(int id)
    {
        var badge = await _db.Badges.FindAsync(id);
        if (badge == null) return NotFound();

        _db.Badges.Remove(badge);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("challenges")]
    public async Task<IActionResult> CreateChallenge([FromBody] Challenge challenge)
    {
        _db.Challenges.Add(challenge);
        await _db.SaveChangesAsync();

        return Ok(challenge);
    }
}