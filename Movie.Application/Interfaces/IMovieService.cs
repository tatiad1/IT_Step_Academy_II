using Movie.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Application.Interfaces
{
    public interface IMovieService
    {
        Task<ICollection<MovieDTO>> GetAllMoviesAsync();
        Task AddMovieAsync(CreateMovieDTO movie);

        Task<MovieDTO> GetMovieByIDAsync(int id);

        Task<bool> UpdateMovieAsync(UpdateMovieDTO dto);
        Task<bool> DeleteMovieAsync(int id);
    }
}
