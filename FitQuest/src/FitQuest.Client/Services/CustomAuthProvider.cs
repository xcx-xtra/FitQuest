using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

public class CustomAuthProvider : AuthenticationStateProvider
{
    private readonly IMockAuthService _authService;

    public CustomAuthProvider(IMockAuthService authService)
    {
        _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Simulate retrieving the current user from the mock service
        var user = await _authService.GetCurrentUserAsync(); // Replace with actual logic to get the current user
        var identity = new ClaimsIdentity();

        if (user != null)
        {
            identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserName ?? string.Empty) }, "mockService");
        }

        var userPrincipal = new ClaimsPrincipal(identity);
        return new AuthenticationState(userPrincipal);
    }

    public void NotifyAuthChanged(AuthenticationState authState)
    {
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }
}
