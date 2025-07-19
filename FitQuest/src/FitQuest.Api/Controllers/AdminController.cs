using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitQuest.Shared.Models;
using FitQuest.Api;
using FitQuest.Api.Controllers;
using FitQuest.Api.Services;
using System.IO;

[Route("api/admin")]
public class AdminController : BaseApiController
{
    private readonly FitQuestContext _db;

    public AdminController(
        FitQuestContext db,
        ILogger<AdminController> logger,
        IDatabaseErrorHandler databaseErrorHandler) 
        : base(logger, databaseErrorHandler)
    {
        _db = db;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        return await ExecuteWithErrorHandlingAsync(async () =>
        {
            var users = await _db.Users.ToListAsync();
            Logger.LogInformation("Retrieved {Count} users", users.Count);
            return users;
        }, "GetUsers");
    }

    [HttpPost("badges")]
    public async Task<IActionResult> CreateBadge([FromBody] Badge badge)
    {
        return await ExecuteWithErrorHandlingAsync(async () =>
        {
            if (badge == null)
            {
                throw new ArgumentException("Badge cannot be null");
            }

            _db.Badges.Add(badge);
            await _db.SaveChangesAsync();

            Logger.LogInformation("Badge created with ID {BadgeId}", badge.Id);
            return badge;
        }, "CreateBadge");
    }

    [HttpDelete("badges/{id}")]
    public async Task<IActionResult> DeleteBadge(int id)
    {
        return await ExecuteWithErrorHandlingAsync(async () =>
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid badge ID");
            }

            var badge = await _db.Badges.FindAsync(id);
            if (badge == null)
            {
                throw new KeyNotFoundException($"Badge with ID {id} not found");
            }

            _db.Badges.Remove(badge);
            await _db.SaveChangesAsync();

            Logger.LogInformation("Badge with ID {BadgeId} deleted", id);
            return new { message = "Badge deleted successfully" };
        }, "DeleteBadge");
    }

    [HttpPost("challenges")]
    public async Task<IActionResult> CreateChallenge([FromBody] Challenge challenge)
    {
        return await ExecuteWithErrorHandlingAsync(async () =>
        {
            if (challenge == null)
            {
                throw new ArgumentException("Challenge cannot be null");
            }

            _db.Challenges.Add(challenge);
            await _db.SaveChangesAsync();

            Logger.LogInformation("Challenge created with ID {ChallengeId}", challenge.Id);
            return challenge;
        }, "CreateChallenge");
    }

    [HttpPost("upload-badge-icon")]
    public async Task<IActionResult> UploadBadgeIcon(IFormFile file)
    {
        return await ExecuteWithErrorHandlingAsync(async () =>
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded or file is empty");
            }

            // Validate file type
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".svg" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException($"File type {fileExtension} is not allowed. Allowed types: {string.Join(", ", allowedExtensions)}");
            }

            // Validate file size (max 5MB)
            if (file.Length > 5 * 1024 * 1024)
            {
                throw new ArgumentException("File size cannot exceed 5MB");
            }

            var uploadsDir = Path.Combine("wwwroot", "badge-icons");
            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            Logger.LogInformation("Badge icon uploaded: {FileName}", fileName);
            return new { FilePath = $"/badge-icons/{fileName}" };
        }, "UploadBadgeIcon");
    }

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}