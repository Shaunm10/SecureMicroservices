using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.Configuration;
using Movies.Client.Models;

namespace Movies.Client.ApiServices;

public class MovieApiService : IMovieApiService
{
    private readonly ServiceUrls _serviceUrlsConfiguration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MovieApiService(
        IOptionsSnapshot<ServiceUrls> serviceUrlsOptionsSnapshot,
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        this._httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        this._serviceUrlsConfiguration = serviceUrlsOptionsSnapshot?.Value ?? throw new ArgumentNullException(nameof(serviceUrlsOptionsSnapshot));
        this._httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public async Task<IEnumerable<Movie>> GetMoviesAsync()
    {
        var movieApi = this.GetProxy();

        var userInfo = await this.GetUserInfoAsync();

        string ownerName = null;
        if (userInfo.UserDictionary.Keys.Contains("given_name"))
        {
            ownerName = userInfo.UserDictionary.FirstOrDefault(x => x.Key == "given_name").Value;
        }

        var serviceMovies = await movieApi.GetMoviesAsync();

        var moviesViewModels = serviceMovies
            .Select(m => this.Map(m))
            .Where(m => ownerName == null || m.Owner?.ToLower() == ownerName.ToLower());

        return moviesViewModels.ToList();
    }

    public async Task<Movie?> GetMovieAsync(int id)
    {
        var proxy = this.GetProxy();
        var m = await proxy.GetMovieAsync(id);
        return this.Map(m);
    }

    public async Task<Movie> CreateMovie(Movie movie)
    {
        var proxy = this.GetProxy();
        var createdMovie = await proxy.PostMovieAsync(this.Map(movie));
        return this.Map(createdMovie);
    }

    public async Task UpdateMovie(Movie movie)
    {
        var proxy = this.GetProxy();
        await proxy.PutMovieAsync(movie.Id.GetValueOrDefault(), this.Map(movie));
    }

    public async Task DeleteMovie(int id)
    {
        var proxy = this.GetProxy();
        await proxy.DeleteMovieAsync(id);
    }

    public async Task<UserInfoViewModel> GetUserInfoAsync()
    {
        var idpClient = this._httpClientFactory.CreateClient(ApiConfigurations.IDPClient);

        // get the MetaData from the discovery document
        var metaDataResponse = await idpClient.GetDiscoveryDocumentAsync();
        if (metaDataResponse.IsError)
        {
            throw new HttpRequestException("Something went wrong while requesting the access token");
        }

        // get the access token from the HttpContext
        var accessToken =
            await this._httpContextAccessor.HttpContext?.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

        // get the userInfo passing the access token to the userInfo endpoint.
        var userInfoResponse = await idpClient.GetUserInfoAsync(new UserInfoRequest
        {
            Address = metaDataResponse.UserInfoEndpoint,
            Token = accessToken
        });

        // if an error occurred
        if (userInfoResponse.IsError)
        {
            throw new HttpRequestException("Something went wrong while getting user info");
        }

        var userInfoDictionary = new Dictionary<string, string>();

        foreach (var claim in userInfoResponse.Claims)
        {
            userInfoDictionary.Add(claim.Type, claim.Value);
        }

        return new UserInfoViewModel(userInfoDictionary);
    }

    private ServiceReferences.MoviesApi GetProxy()
    {
        var httpClient = this._httpClientFactory.CreateClient(ApiConfigurations.MovieClient);

        var movieApi = new ServiceReferences.MoviesApi(this._serviceUrlsConfiguration.MovieApi, httpClient);

        return movieApi;
    }

    private Movie Map(ServiceReferences.Movie movieFromService)
    {
        return new Movie
        {
            Genre = movieFromService.Genre,
            Id = movieFromService.Id,
            ImageUrl = movieFromService.ImageUrl,
            Owner = movieFromService.Owner,
            Rating = movieFromService.Rating,
            ReleaseDate = DateTime.Parse(movieFromService?.ReleaseDate?.ToString()),
            Title = movieFromService.Title
        };
    }

    private ServiceReferences.Movie Map(Movie movie)
    {
        return new ServiceReferences.Movie
        {
            Genre = movie.Genre,
            Id = movie.Id,
            ImageUrl = movie.ImageUrl,
            Owner = movie.Owner,
            Rating = movie.Rating,
            ReleaseDate = DateTimeOffset.Parse(movie.ReleaseDate.ToString()),
            Title = movie.Title
        };
    }
}
