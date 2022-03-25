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
        private readonly IHttpClientFactory _httpClientFactory;

        public MovieApiService(
            IOptionsSnapshot<ServiceUrls> serviceUrlsOptionsSnapshot,
            IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this._serviceUrlsConfiguration = serviceUrlsOptionsSnapshot?.Value ?? throw new ArgumentNullException(nameof(serviceUrlsOptionsSnapshot));
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            var httpClient = this._httpClientFactory.CreateClient("MovieAPIClient");

            var movieApi = new ServiceReferences.MoviesApi(this._serviceUrlsConfiguration.MovieApi, httpClient);

            var serviceMovies = await movieApi.GetMoviesAsync();

            var test = serviceMovies.Select(m => new Movie
            {
                Title = m.Title,
                Genre = m.Genre,
                ReleaseDate = DateTime.Parse(m.ReleaseDate.ToString()),
                ImageUrl = m.ImageUrl,
                Owner = m.Owner,
                Id = m.Id,
                Rating = m.Rating
            });

            return test.ToList();
           
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

        //private async Task<TokenResponse> GetTokenResponseAsync()
        //{
        //    var apiClientCredentials = new ClientCredentialsTokenRequest
        //    {
        //        Address = $"{this._openIdConnectConfiguration.Authority}/connect/token",
        //        ClientId = "movieClient", //this._openIdConnectConfiguration.ClientId,
        //        ClientSecret = this._openIdConnectConfiguration.ClientSecret,

        //        // this is the scope our Protected Api requires.
        //        Scope = this._openIdConnectConfiguration.MovieApiScope
        //    };

        //    // create a new HttpClient to talk to our IdentityServer
        //    // TODO: pull from httpClientFactory
        //    var client = new HttpClient();

        //    var disco = await client.GetDiscoveryDocumentAsync(this._openIdConnectConfiguration.Authority);
        //    if (disco.IsError)
        //    {
        //        throw new ApplicationException($"Unable to retrieve discovery document: {disco.Error}");
        //    }

        //    // authenticate and get an access token from Identity Server
        //    var tokenResponse = await client.RequestClientCredentialsTokenAsync(apiClientCredentials);
        //    if (tokenResponse.IsError)
        //    {
        //        throw new ApplicationException($"Unable to retrieve credential response: {tokenResponse.Error}");
        //    }

        //    return tokenResponse;
        //}
    }
}
