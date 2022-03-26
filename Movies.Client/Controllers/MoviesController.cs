
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Movies.Client.ApiServices;
using Movies.Client.Models;

namespace Movies.Client.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieApiService _movieApiService;

        public MoviesController(IMovieApiService movieApiService)
        {
            this._movieApiService = movieApiService ?? throw new ArgumentNullException(nameof(movieApiService));
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            await this.LogTokenAndClaims();
            var movies = await this._movieApiService.GetMoviesAsync();
            return this.View(movies);
        }

        public async Task LogTokenAndClaims()
        {
            var identityToken = await this.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);
            Debug.WriteLine($"Identity token: {identityToken}");
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {claim.Type} - Claim value: {claim.Value}");
            }
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return this.NotFound();
            }

            var movie = await this._movieApiService.GetMovieAsync(id.Value);
            if (movie == null)
            {
                return this.NotFound();
            }

            return this.View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Genre,Rating,ReleaseDate,ImageUrl,Owner")] Movie movie)
        {

            if (this.ModelState.IsValid)
            {
                var createdMovie = await this._movieApiService.CreateMovie(movie);
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return this.NotFound();
            }

            var movie = await this._movieApiService.GetMovieAsync(id.Value);
            if (movie == null)
            {
                return this.NotFound();
            }

            return this.View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Genre,Rating,ReleaseDate,ImageUrl,Owner")] Movie movie)
        {
            if (id != movie.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {

                await this._movieApiService.UpdateMovie(movie);

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                return this.NotFound();
            }

            var movie = await this._movieApiService.GetMovieAsync(id);

            if (movie == null)
            {
                return this.NotFound();
            }

            return this.View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            return this.View();
            //await this._movieApiService.DeleteMovie(id.Value);
            //return this.RedirectToAction(nameof(this.Index));
        }

        private async Task<bool> MovieExists(int? id)
        {
            var movie = await this._movieApiService.GetMovieAsync(id.GetValueOrDefault());
            return movie != null;
        }
    }
}
