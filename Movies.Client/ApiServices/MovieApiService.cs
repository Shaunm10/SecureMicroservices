using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Movies.Client.Configuration;
using Movies.Client.Models;
using Newtonsoft.Json;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        private readonly ServiceUrls _serviceUrlsConfiguration;
        private readonly OpenIdConnect _openIdConnectConfiguration;

        private readonly IHttpClientFactory _httpClientFactory;

        public MovieApiService(
            IOptionsSnapshot<OpenIdConnect> openIdConnectSnapshot, 
            IOptionsSnapshot<ServiceUrls> serviceUrlsOptionsSnapshot,
            IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this._serviceUrlsConfiguration = serviceUrlsOptionsSnapshot?.Value ?? throw new ArgumentNullException(nameof(serviceUrlsOptionsSnapshot));
            this._openIdConnectConfiguration = openIdConnectSnapshot?.Value ?? throw new ArgumentNullException(nameof(openIdConnectSnapshot));
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {

            var httpClient = this._httpClientFactory.CreateClient("MovieAPIClient");

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Movies");

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            return movieList;

            //var httpClient = 

            // get code_access token
            //var tokenResponse = await this.GetTokenResponseAsync();

            // make request to endpoint
            //var apiClient = new HttpClient();
            //apiClient.SetBearerToken(tokenResponse.AccessToken);
            //var apiResponse = await apiClient.GetAsync($"{this._serviceUrlsConfiguration.MovieApi}/api/movies");
            //apiResponse.EnsureSuccessStatusCode();

            //var content = await apiResponse.Content.ReadAsStringAsync();

            //// deserialize the response.
            //List<Movie> movieList = JsonConvert.DeserializeObject<List<Movie>>(content);
            //return movieList;
        }

        public Task<Movie?> GetMovieAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> CreateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> UpdateMovie(Movie movie)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMovie(int id)
        {
            throw new NotImplementedException();
        }

        private async Task<TokenResponse> GetTokenResponseAsync()
        {
            var apiClientCredentials = new ClientCredentialsTokenRequest
            {
                Address = $"{this._openIdConnectConfiguration.Authority}/connect/token",
                ClientId = "movieClient", //this._openIdConnectConfiguration.ClientId,
                ClientSecret = this._openIdConnectConfiguration.ClientSecret,

                // this is the scope our Protected Api requires.
                Scope = this._openIdConnectConfiguration.MovieApiScope
            };

            // create a new HttpClient to talk to our IdentityServer
            // TODO: pull from httpClientFactory
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(this._openIdConnectConfiguration.Authority);
            if (disco.IsError)
            {
                throw new ApplicationException($"Unable to retrieve discovery document: {disco.Error}");
            }

            // authenticate and get an access token from Identity Server
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
            if (tokenResponse.IsError)
            {
                throw new ApplicationException($"Unable to retrieve credential response: {tokenResponse.Error}");
            }

            return tokenResponse;
        }
    }
}
