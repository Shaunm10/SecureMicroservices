using Movies.Client.Models;

namespace Movies.Client.ApiServices
{
    public class MovieApiService : IMovieApiService
    {
        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            var movieList = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Genre = "Comics",
                    Title = "asd",
                    Rating = "9.3",
                    ImageUrl = "images/src",
                    ReleaseDate = DateTime.Now,
                    Owner = "sms"
                }
            };
            return await Task.FromResult(movieList);
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
    }
}
