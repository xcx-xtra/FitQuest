using FitQuest.Shared.Models;

public interface IMockAuthService {
    Task<User?> GetCurrentUserAsync();
}