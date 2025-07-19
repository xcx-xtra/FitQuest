using FitQuest.Shared.Models;

public interface ILoginService {
    Task<User?> AuthenticateAsync(string username, string password);
}