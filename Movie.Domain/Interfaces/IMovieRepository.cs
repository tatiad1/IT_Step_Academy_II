namespace Movie.Domain.Interfaces
{
    public interface IMovieRepository
    {
        Task<ICollection<Movie.Domain.Entities.Movie>> GetAllMoviesAsync();
        Task AddMovieAsync(Movie.Domain.Entities.Movie movie);

        Task<Movie.Domain.Entities.Movie> GetMovieByIDAsync(int id);

        Task UpdateMovieAsync(Movie.Domain.Entities.Movie movie);
        Task<bool> DeleteMovieAsync(int id);

        Task<ICollection<Movie.Domain.Entities.Movie>> SearchMoviesByStudioAsync(
            int year,
            string studioName,
            int minimumActorCount);

        Task<ICollection<Movie.Domain.Entities.Movie>> SearchMoviesByCountryAsync(
            string countryName,
            int minimumYear,
            int maximumActorCount);

        Task<ICollection<Movie.Domain.Entities.Movie>> SearchMoviesAdvancedAsync(
            int fromYear,
            int toYear,
            string countryName,
            string titleText,
            int minimumActorCount);
    }
}