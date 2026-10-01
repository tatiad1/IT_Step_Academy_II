using Microsoft.EntityFrameworkCore;
using Movie.Domain.Interfaces;
using Movie.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieDbContext _movieDbContext;

        public MovieRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }

        public async Task AddMovieAsync(Domain.Entities.Movie movie)
        {
            await _movieDbContext.Movies.AddAsync(movie);
            await _movieDbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Domain.Entities.Movie>> GetAllMoviesAsync()
        {
            return await _movieDbContext.Movies
                .Include(m => m.Studio)
                .ToListAsync();
        }
    }
}
