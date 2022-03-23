﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Api.Data;
using Movies.Api.Model;

namespace Movies.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MoviesApiContext _context;

    public MoviesController(MoviesApiContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // GET: api/Movies
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetMovie()
    {
        return await this._context.Movie.ToListAsync();
    }

    // GET: api/Movies/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int? id)
    {
        var movie = await this._context.Movie.FindAsync(id);

        if (movie == null)
        {
            return this.NotFound();
        }

        return movie;
    }

    // PUT: api/Movies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int? id, Movie movie)
    {
        if (id != movie.Id)
        {
            return this.BadRequest();
        }

        this._context.Entry(movie).State = EntityState.Modified;

        try
        {
            await this._context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!this.MovieExists(id))
            {
                return this.NotFound();
            }
            else
            {
                throw;
            }
        }

        return this.NoContent();
    }

    // POST: api/Movies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Movie>> PostMovie(Movie movie)
    {
        this._context.Movie.Add(movie);
        await this._context.SaveChangesAsync();

        return this.CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
    }

    // DELETE: api/Movies/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int? id)
    {
        var movie = await this._context.Movie.FindAsync(id);
        if (movie == null)
        {
            return this.NotFound();
        }

        this._context.Movie.Remove(movie);
        await this._context.SaveChangesAsync();

        return this.NoContent();
    }

    private bool MovieExists(int? id)
    {
        return this._context.Movie.Any(e => e.Id == id);
    }
}