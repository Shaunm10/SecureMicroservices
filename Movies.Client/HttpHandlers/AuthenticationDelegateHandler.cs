﻿using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.ApiServices;

namespace Movies.Client.HttpHandlers;

public class AuthenticationDelegateHandler : DelegatingHandler
{
    //private readonly IHttpClientFactory _httpClientFactory;
   // private readonly ClientCredentialsTokenRequest _tokenRequest;
   private readonly IHttpContextAccessor _httpContextAccessor;

    //public AuthenticationDelegateHandler(IHttpClientFactory httpClientFactory, ClientCredentialsTokenRequest tokenRequest)
    //{
    //    this._httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    //    this._tokenRequest = tokenRequest ?? throw new ArgumentNullException(nameof(tokenRequest));
    //}

    public AuthenticationDelegateHandler(IHttpContextAccessor httpContextAccessor)
    {
        this._httpContextAccessor = httpContextAccessor;
    }
    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //var httpClient = this._httpClientFactory.CreateClient(ApiConfigurations.IDPClient);

        //var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(this._tokenRequest);

        //if (tokenResponse.IsError)
        //{
        //    throw new ApplicationException(
        //        $"Unable to call RequestClientCredentialsTokenAsync:{tokenResponse.HttpErrorReason} - {tokenResponse.Error}");
        //}

        //request.SetBearerToken(tokenResponse.AccessToken);

        var accessToken =
            await this._httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            request.SetBearerToken(accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    //protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //{
    //    var httpClient = this._httpClientFactory.CreateClient(ApiConfigurations.IDPClient);

    //    var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(this._tokenRequest);

    //    if (tokenResponse.IsError)
    //    {
    //        throw new ApplicationException(
    //            $"Unable to call RequestClientCredentialsTokenAsync:{tokenResponse.HttpErrorReason} - {tokenResponse.Error}");
    //    }

    //    request.SetBearerToken(tokenResponse.AccessToken);

    //    return await base.SendAsync(request, cancellationToken);
    //}
}