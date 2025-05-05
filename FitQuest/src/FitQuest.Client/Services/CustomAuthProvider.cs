using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

public class CustomAuthProvider : AuthenticationStateProvider
{
    private readonly ITokenService tokenService;

    public CustomAuthProvider(ITokenService tokenService) => this.tokenService = tokenService;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await tokenService.GetTokenAsync();
        var identity = new ClaimsIdentity();

        if (!string.IsNullOrWhiteSpace(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var claims = jwt.Claims;
            identity = new ClaimsIdentity(claims, "jwt");
        }

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public void NotifyAuthChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
}
