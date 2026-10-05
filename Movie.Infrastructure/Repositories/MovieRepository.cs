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


        public async Task<Movie.Domain.Entities.Movie> GetMovieByIDAsync(int id)
        {
            //var movieByID = await _movieDbContext.Movies.FindAsync(id);

            var movieByID = await _movieDbContext.Movies
                .Include(m => m.Studio)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movieByID;
        }


        public async Task UpdateMovieAsync(Domain.Entities.Movie movie)
        {
            _movieDbContext.Movies.Update(movie);
            await _movieDbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movieToDelete = await _movieDbContext.Movies.FindAsync(id);

            if (movieToDelete == null)
            {
                return false;
            }

            _movieDbContext.Movies.Remove(movieToDelete);
            await _movieDbContext.SaveChangesAsync();
            return true;
        }

    }
}
