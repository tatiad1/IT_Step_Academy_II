using Microsoft.EntityFrameworkCore;
using Movie.Domain.Interfaces;
using Movie.Infrastructure.Data;

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
        }

        public async Task<ICollection<Domain.Entities.Movie>> GetAllMoviesAsync()
        {
            return await _movieDbContext.Movies
                .Include(m => m.Studio)
                .ToListAsync();
        }

        public async Task<Movie.Domain.Entities.Movie> GetMovieByIDAsync(int id)
        {
            var movieByID = await _movieDbContext.Movies
                .Include(m => m.Studio)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movieByID;
        }

        public async Task UpdateMovieAsync(Domain.Entities.Movie movie)
        {
            _movieDbContext.Movies.Update(movie);
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movieToDelete = await _movieDbContext.Movies.FindAsync(id);

            if (movieToDelete == null)
            {
                return false;
            }

            _movieDbContext.Movies.Remove(movieToDelete);
            return true;
        }

        // ===============================
        // Task 1
        // ===============================
        public async Task<ICollection<Domain.Entities.Movie>> SearchMoviesByStudioAsync(
            int year,
            string studioName,
            int minimumActorCount)
        {
            return await _movieDbContext.Movies
                .Include(m => m.Studio)
                .Where(m => m.ReleaseYear >= year
                         && m.Studio.Name == studioName
                         && m.Actors.Count >= minimumActorCount)
                .OrderByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .ToListAsync();
        }

        // ===============================
        // Task 2
        // ===============================
        public async Task<ICollection<Domain.Entities.Movie>> SearchMoviesByCountryAsync(
            string countryName,
            int minimumYear,
            int maximumActorCount)
        {
            return await _movieDbContext.Movies
                .Include(m => m.Studio)
                .Where(m => m.Studio.Country.Name == countryName
                         && m.ReleaseYear >= minimumYear
                         && m.Actors.Count <= maximumActorCount)
                .OrderBy(m => m.Actors.Count)
                .ThenByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .ToListAsync();
        }

        // ===============================
        // Task 3
        // ===============================
        public async Task<ICollection<Domain.Entities.Movie>> SearchMoviesAdvancedAsync(
            int fromYear,
            int toYear,
            string countryName,
            string titleText,
            int minimumActorCount)
        {
            return await _movieDbContext.Movies
                .Include(m => m.Studio)
                .Where(m => m.ReleaseYear >= fromYear
                         && m.ReleaseYear <= toYear
                         && m.Studio.Country.Name == countryName
                         && m.Title.Contains(titleText)
                         && m.Actors.Count >= minimumActorCount)
                .OrderByDescending(m => m.Actors.Count)
                .ThenByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Studio.Name)
                .ThenBy(m => m.Title)
                .ToListAsync();
        }
    }
}