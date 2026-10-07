using Movie.Application.Interfaces;
using Movie.Domain.DTOs;
using Movie.Domain.Interfaces;

namespace Movie.Application.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MovieService(IMovieRepository movieRepository, IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddMovieAsync(CreateMovieDTO movie)
        {
            var movieEntity = new Movie.Domain.Entities.Movie
            {
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                StudioId = movie.StudioId
            };
            await _movieRepository.AddMovieAsync(movieEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<ICollection<MovieDTO>> GetAllMoviesAsync()
        {
            var movies = await _movieRepository.GetAllMoviesAsync();

            return movies.Select(MapToDTO).ToList();
        }

        public async Task<MovieDTO> GetMovieByIDAsync(int id)
        {
            var movie = await _movieRepository.GetMovieByIDAsync(id);

            if (movie == null)
            {
                return null;
            }

            return MapToDTO(movie);
        }

        public async Task<bool> UpdateMovieAsync(UpdateMovieDTO dto)
        {
            var movie = await _movieRepository.GetMovieByIDAsync(dto.Id);

            if (movie == null)
            {
                return false;
            }

            movie.Title = dto.Title;
            movie.ReleaseYear = dto.ReleaseYear;
            movie.StudioId = dto.StudioId;

            await _movieRepository.UpdateMovieAsync(movie);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var result = await _movieRepository.DeleteMovieAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
            }
            return result;
        }

        // ===============================
        // Search (Tasks 1-3)
        // ===============================
        public async Task<ICollection<MovieDTO>> SearchMoviesByStudioAsync(
            int year,
            string studioName,
            int minimumActorCount)
        {
            var movies = await _movieRepository.SearchMoviesByStudioAsync(year, studioName, minimumActorCount);

            return movies.Select(MapToDTO).ToList();
        }

        public async Task<ICollection<MovieDTO>> SearchMoviesByCountryAsync(
            string countryName,
            int minimumYear,
            int maximumActorCount)
        {
            var movies = await _movieRepository.SearchMoviesByCountryAsync(countryName, minimumYear, maximumActorCount);

            return movies.Select(MapToDTO).ToList();
        }

        public async Task<ICollection<MovieDTO>> SearchMoviesAdvancedAsync(
            int fromYear,
            int toYear,
            string countryName,
            string titleText,
            int minimumActorCount)
        {
            // Contains(null) would throw, so treat null as "no title filter"
            var movies = await _movieRepository.SearchMoviesAdvancedAsync(
                fromYear, toYear, countryName, titleText ?? string.Empty, minimumActorCount);

            return movies.Select(MapToDTO).ToList();
        }

        // Entity -> DTO mapping in one place instead of repeating it in every method
        private static MovieDTO MapToDTO(Movie.Domain.Entities.Movie movie)
        {
            return new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                StudioId = movie.StudioId,
                StudioName = movie.Studio.Name
            };
        }
    }
}