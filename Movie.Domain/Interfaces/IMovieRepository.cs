using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task<ICollection<Movie.Domain.Entities.Movie>> GetAllMoviesAsync();
        Task AddMovieAsync(Movie.Domain.Entities.Movie movie);

        Task<Movie.Domain.Entities.Movie> GetMovieByIDAsync(int id);

        Task UpdateMovieAsync(Movie.Domain.Entities.Movie movie);
        Task<bool> DeleteMovieAsync(int id);

    }
}
