using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using FitQuest.Client.Services;

public class CustomAuthProvider : AuthenticationStateProvider
{
    private readonly ITokenService _tokenService;

    public CustomAuthProvider(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _tokenService.GetTokenAsync();
        ClaimsIdentity identity = new ClaimsIdentity();

        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                // Validate that the token is actually a JWT
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var claims = jwtToken.Claims.ToList();
                    
                    // Ensure we have a user identifier claim
                    if (claims.Any(c => c.Type == "sub" || c.Type == "nameid" || c.Type == "id" || c.Type == ClaimTypes.NameIdentifier))
                    {
                        identity = new ClaimsIdentity(claims, "jwt");
                    }
                    else
                    {
                        // Token doesn't contain user ID - clear it
                        await _tokenService.ClearTokenAsync();
                    }
                }
                else
                {
                    // Invalid token format - clear it
                    await _tokenService.ClearTokenAsync();
                }
            }
            catch (Exception ex)
            {
                // Invalid token, clear it and return empty identity
                await _tokenService.ClearTokenAsync();
                Console.WriteLine($"Error parsing JWT token: {ex.Message}");
            }
        }

        var userPrincipal = new ClaimsPrincipal(identity);
        return new AuthenticationState(userPrincipal);
    }

    public void NotifyAuthChanged(AuthenticationState authState)
    {
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }
}
