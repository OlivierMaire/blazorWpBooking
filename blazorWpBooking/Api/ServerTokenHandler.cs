// filepath: /home/fuyuki/dev/blazorWpBooking/blazorWpBooking/Api/ServerTokenHandler.cs
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using blazorWpBooking.Components.Account;

public class ServerTokenHandler : DelegatingHandler
{
    private readonly CircuitTokenStore _tokenStore;
    public ServerTokenHandler(CircuitTokenStore tokenStore) => _tokenStore = tokenStore;
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _tokenStore.Token;
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return base.SendAsync(request, cancellationToken);
    }
}