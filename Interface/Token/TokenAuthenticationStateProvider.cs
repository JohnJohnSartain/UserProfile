using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Interface.Token;

public class TokenAuthenticationStateProvider : AuthenticationStateProvider
{
    private ILocalToken _localToken;

    public TokenAuthenticationStateProvider(ILocalToken localToken) => _localToken = localToken;

    public async Task Logout()
    {
        await _localToken.RemoveToken();
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task SetTokenAsync()
    {
        await _localToken.SetToken(await GetTokenAsync());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<string> GetTokenAsync() => await _localToken.GetTokenAsync();

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await GetTokenAsync();

        var identity = JwtToken.IsEmptyOrInvalid(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(JwtToken.ParseClaimsFromJwt(token), "jwt");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
}