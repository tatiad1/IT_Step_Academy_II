using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task<ICollection<Movie.Domain.Entities.Movie>> GetAllMoviesAsync();
        Task AddMovieAsync(Movie.Domain.Entities.Movie movie);

    }
}
