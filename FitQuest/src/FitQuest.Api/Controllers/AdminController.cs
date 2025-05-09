using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitQuest.Shared.Models;
using FitQuest.Api;
using System.IO;

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
        var users = await _db.Users.ToListAsync();
        return Ok(users);
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

    [HttpPost("upload-badge-icon")]
    public async Task<IActionResult> UploadBadgeIcon(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var filePath = Path.Combine("wwwroot/badge-icons", file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { FilePath = $"/badge-icons/{file.FileName}" });
    }

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}