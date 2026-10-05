using Movie.Domain.DTOs;

namespace Movie.Application.Interfaces
{
    public interface IMovieService
    {
        Task<ICollection<MovieDTO>> GetAllMoviesAsync();
        Task AddMovieAsync(CreateMovieDTO movie);
        Task<MovieDTO> GetMovieByIDAsync(int id);

        Task<bool> UpdateMovieAsync(UpdateMovieDTO movie);
        Task<bool> DeleteMovieAsync(int id);

        Task<ICollection<MovieDTO>> SearchMoviesByStudioAsync(
            int year,
            string studioName,
            int minimumActorCount);

        Task<ICollection<MovieDTO>> SearchMoviesByCountryAsync(
            string countryName,
            int minimumYear,
            int maximumActorCount);

        Task<ICollection<MovieDTO>> SearchMoviesAdvancedAsync(
            int fromYear,
            int toYear,
            string countryName,
            string titleText,
            int minimumActorCount);
    }
}