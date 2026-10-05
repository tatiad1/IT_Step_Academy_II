using Movie.Application.Interfaces;
using Movie.Domain.DTOs;
using Movie.Domain.Interfaces;

namespace Movie.Application.Implementations
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
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
        }

        public async Task<ICollection<MovieDTO>> GetAllMoviesAsync()
        {
            var movies = await _movieRepository.GetAllMoviesAsync();

            var moviesDTO = movies.Select(m => new MovieDTO
            {
                Id = m.Id,
                Title = m.Title,
                ReleaseYear = m.ReleaseYear,
                StudioName = m.Studio.Name,
            }).ToList();

            return moviesDTO;
        }

        public async Task<MovieDTO> GetMovieByIDAsync(int id)
        {
            var movie = await _movieRepository.GetMovieByIDAsync(id);

            return new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                StudioName = movie.Studio.Name
            };
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
            return true;

        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            return await _movieRepository.DeleteMovieAsync(id);
        }
    }
}
