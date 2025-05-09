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
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims;
                identity = new ClaimsIdentity(claims, "jwt");
            }
            catch
            {
                // Invalid token, return empty identity
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
