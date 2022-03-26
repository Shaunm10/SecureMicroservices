using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Movies.Client.Configuration;

using Movies.Client.Models;
using Newtonsoft.Json;

namespace Movies.Client.ApiServices;

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
        //var httpClient = this._httpClientFactory.CreateClient("MovieAPIClient");

        //var movieApi = new ServiceReferences.MoviesApi(this._serviceUrlsConfiguration.MovieApi, httpClient);

        var movieApi = this.GetProxy();

        var serviceMovies = await movieApi.GetMoviesAsync();

        var test = serviceMovies.Select(m => this.Map(m));

        return test.ToList();

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

    private ServiceReferences.MoviesApi GetProxy()
    {
        var httpClient = this._httpClientFactory.CreateClient("MovieAPIClient");

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
            ReleaseDate = DateTime.Parse(movieFromService.ReleaseDate.ToString()),
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
