using Microsoft.AspNetCore.Mvc.RazorPages;
using Movie.Application.Interfaces;
using Movie.Domain.DTOs;

namespace Movie.Web.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly IMovieService _movieService;

        public IndexModel(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public ICollection<MovieDTO> Movies { get; set; } = new List<MovieDTO>();

        public async Task OnGetAsync()
        {
            var movies = await _movieService.GetAllMoviesAsync();

            // newest first, then alphabetical
            Movies = movies
                .OrderByDescending(m => m.ReleaseYear)
                .ThenBy(m => m.Title)
                .ToList();
        }
    }
}