using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using blazorWpBooking.Data;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;

namespace blazorWpBooking.Components.Account;

// This is a server-side AuthenticationStateProvider that revalidates the security stamp for the connected user
// every 30 minutes an interactive circuit is connected.
internal sealed class WordPressRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        CircuitTokenStore tokenStore,
        IOptions<IdentityOptions> options)
    : RevalidatingServerAuthenticationStateProvider(loggerFactory)
{
    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    // private readonly ILocalStorageService _localStorageService;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {

        var token = tokenStore.Token;
        if (string.IsNullOrWhiteSpace(token))
        {
            return await Task.FromResult(new AuthenticationState(_anonymous));
        }

        var principal = BuildClaimsPrincipalFromToken(tokenStore);
        return await Task.FromResult(new AuthenticationState(principal));


        // var token = await localStorageService.GetItemAsync<string>("accessToken");

        // if (string.IsNullOrEmpty(token))
        // {
        //     return new AuthenticationState(_anonymous);
        // }

        // var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(token);
        // var claims = tokenContent.Claims;
        // var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        // return await Task.FromResult(new AuthenticationState(user));
    }


    public void MarkUserLoggedOut()
    {
        tokenStore.Clear();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }
    public void AuthenticateUser(TokenResponse token)
    {
        tokenStore.SetToken(token);
        var principal = BuildClaimsPrincipalFromToken(tokenStore);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));

        // var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(token);
        // var claims = tokenContent.Claims;
        // var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        // var state = new AuthenticationState(user);
        // NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    private static ClaimsPrincipal BuildClaimsPrincipalFromToken(CircuitTokenStore token)
    {
        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken? jwt = null;
        try
        {
            jwt = handler.ReadJwtToken(token.Token);
        }
        catch
        {
            return new ClaimsPrincipal(new ClaimsIdentity());
        }

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        identity.AddClaim(new Claim(ClaimTypes.Name, token.DisplayName ?? ""));
        identity.AddClaim(new Claim(ClaimTypes.Email, token.Email ?? ""));
        return new ClaimsPrincipal(identity);
    }

    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        // Get the user manager from a new scope to ensure it fetches fresh data
        await using var scope = scopeFactory.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        return await ValidateSecurityStampAsync(userManager, authenticationState.User);
    }

    private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
    {
        var user = await userManager.GetUserAsync(principal);
        if (user is null)
        {
            return false;
        }
        else if (!userManager.SupportsUserSecurityStamp)
        {
            return true;
        }
        else
        {
            var principalStamp = principal.FindFirstValue(options.Value.ClaimsIdentity.SecurityStampClaimType);
            var userStamp = await userManager.GetSecurityStampAsync(user);
            return principalStamp == userStamp;
        }
    }

    public CircuitTokenStore GetTokenStore() => tokenStore;
}
