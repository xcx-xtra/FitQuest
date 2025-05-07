using System.Threading.Tasks;
using FitQuest.Shared.Models;

namespace FitQuest.Client.Services
{
    public class MockAuthService : IMockAuthService, ILoginService
    {
        public Task<User?> GetCurrentUserAsync()
        {
            // Return a mock user for testing purposes
            return Task.FromResult<User?>(new User
            {
                Id = 1,
                UserName = "MockUser",
                Email = "mockuser@example.com"
            });
        }

        public Task<User?> AuthenticateAsync(string username, string password)
        {
            // Simulate authentication logic
            if (username == "MockUser" && password == "password")
            {
                return Task.FromResult<User?>(new User
                {
                    Id = 1,
                    UserName = "MockUser",
                    Email = "mockuser@example.com"
                });
            }
            return Task.FromResult<User?>(null);
        }
    }
}