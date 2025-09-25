using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using blazorWpBooking.Data;
using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using WordPressPCL;
using WordPressPCL.Models;
using System.Threading.Tasks;
using System.Text.Json.Nodes;

namespace blazorWpBooking.Components.Account;

// This is a server-side AuthenticationStateProvider that revalidates the security stamp for the connected user
// every 30 minutes an interactive circuit is connected.
internal sealed class WordPressRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        CircuitTokenStore tokenStore,
        WordPressAuthentication wpAuth,
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

        var principal = await BuildClaimsPrincipalFromToken(tokenStore, wpAuth.WordPressApiUrl);



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
    public async Task AuthenticateUser(TokenResponse token)
    {
        tokenStore.SetToken(token);
        var principal = await BuildClaimsPrincipalFromToken(tokenStore, wpAuth.WordPressApiUrl);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));

        // var tokenContent = _jwtSecurityTokenHandler.ReadJwtToken(token);
        // var claims = tokenContent.Claims;
        // var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        // var state = new AuthenticationState(user);
        // NotifyAuthenticationStateChanged(Task.FromResult(state));
    }

    private static async Task<ClaimsPrincipal> BuildClaimsPrincipalFromToken(CircuitTokenStore token, string wordPressApiUrl = "")
    {
        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken? jwt = null;
        try
        {
            jwt = handler.ReadJwtToken(token.Token);
            var claims = jwt.Claims;
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                // principal.Claims.Append(claim);
            }

            //pass the Wordpress REST API base address as string
            var client = new WordPressClient(wordPressApiUrl + "/wp-json/");
            client.Auth.UseBearerAuth(JWTPlugin.JWTAuthByEnriqueChavez);
            client.Auth.SetJWToken(token.Token);

            var jwtData = jwt.Claims.First(c => c.Type == "data").Value;
            string userIdString = JsonNode.Parse(jwtData)?["user"]?["id"]?.GetValue<string>() ?? string.Empty;
            int.TryParse(userIdString, out int userId);
            WordPressPCL.Utility.UsersQueryBuilder queryBuilder = new()
            {
                Include = new List<int> { userId },
                // required for roles to be loaded
                Context = Context.Edit
            };

            var user = await client.Users.QueryAsync(queryBuilder, useAuth: true);
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
