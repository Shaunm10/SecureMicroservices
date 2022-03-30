using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Movies.Client.HttpHandlers;

public class AuthenticationDelegateHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationDelegateHandler(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor;
    }

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken =
            await this._httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            request.SetBearerToken(accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}